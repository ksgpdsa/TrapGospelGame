using System;
using System.Collections;
using System.Collections.Generic;
using Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Vida")]
        [SerializeField] private int enemyLives;
        
        [Header("Ataque")]
        [SerializeField] protected float visionDistance;
        [SerializeField] protected GameObject attack;
        [SerializeField] protected float attackVelocity;
        [SerializeField] private float attackFrequency;
        [SerializeField] protected int takeDamage;
        [SerializeField] private float howTimeToNextAttack;
        [SerializeField] private float knockBackForceOnTouch = 3f;
        
        [Header("Movimento")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float chaseSpeed = 1.2f;
        [SerializeField] private float howTimeToNextJump = 10f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private LayerMask walkLayers;
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private float angleCapsule;
        [SerializeField] private List<GameObject> points;
        [SerializeField] private float howDistanceToPointPermitted = 1f;
        [SerializeField] private float jumpByKnockBack = 1f;
        
        private Collider2D _collider2D;
        private EnemyHealth _enemyHealth;
        private SpriteRenderer _spriteRenderer;
        private EnemyEnvironmentChecker _enemyEnvironmentChecker;
        private GameObject _currentPoint;
        private Player.Player _playerScript;
        private bool _isWallAhead;
        private bool _isCliffAhead;
        private int _direction;
        private bool _isWalking;
        private bool _blockJump;
        private float _timerToNextJump;

        protected GameObject Player;
        protected Sprite ThisSprite;
        protected AnimationManager AnimationManager;
        protected Rigidbody2D Rigidbody2D;
        protected EnemyMovementController EnemyMovement;
        protected EnemyAttackHandler AttackHandler;
        protected bool IsInVision;
        protected bool IsGrounded;
        protected bool CanJump;

        protected abstract int ScoreOnDeath { get; }
        protected abstract int ScoreOnHit { get; }
        protected abstract void InstantiateAttack();
        protected abstract bool InstantiateAttackByAnimation { get; }
        protected abstract bool HasZumbiMode { get; }
        protected abstract bool HasPatrolMode { get; }
        protected abstract bool HasFixedMode { get; }
        protected abstract bool HasGoToPointsMode { get; }

        private void Awake()
        {
            _enemyHealth = new EnemyHealth(enemyLives);
        }

        protected void Start()
        {
            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _collider2D = gameObject.GetComponent<Collider2D>();
            
            ThisSprite = _spriteRenderer.sprite;
            Player = GameObject.FindGameObjectWithTag("Player");
            _playerScript = Player.GetComponent<Player.Player>();
            
            AnimationManager = new AnimationManager(GetComponent<Animator>());
            _enemyEnvironmentChecker = new EnemyEnvironmentChecker(walkLayers, transform, Player.transform, feetPosition, sizeCapsule, angleCapsule, 0);
            EnemyMovement = new EnemyMovementController(chaseSpeed, Player.transform, transform, Rigidbody2D, AnimationManager, _spriteRenderer, _collider2D, moveSpeed, jumpForce);
            AttackHandler = new EnemyAttackHandler(attackFrequency, AnimationManager);

            _enemyHealth.StartLivesInHud();
            _direction = EnemyMovement.GetDirection();
            ManageCanJump();
        }

        protected void FixedUpdate()
        {
            CheckEnvironment();

            ManageTimerToNextJump();

            _direction = EnemyMovement.GetDirection();

            MovementMode();
        }

        private void ManageTimerToNextJump()
        {
            if (_timerToNextJump > 0)
            {
                _timerToNextJump -= Time.fixedDeltaTime;
            }
        }

        private void MovementMode()
        {
            // Obtemos a direção do jogador
            if (IsInVision)
            {
                if (HasZumbiMode)
                {
                    ZumbiMode();
                }
                else if (HasFixedMode)
                {
                    FixedMode();
                }
            }
            else 
            {
                if (HasPatrolMode)
                {
                    PatrolMode();
                }
                else if(HasFixedMode)
                {
                    FixedMode();
                } 
                else if (HasGoToPointsMode && points != null && points.Count > 0)
                {
                    if (!_isWalking)
                    {
                        _currentPoint = GetRandomPoint(); // Escolhe um novo ponto apenas quando necessário
                    }

                    GoToPointsMode(_currentPoint);
                }
            }
        }

        private GameObject GetRandomPoint()
        {
            var randomPoint = Random.Range(0, points.Count);
            return points[randomPoint];
        }


        public void EndAttack()
        {
            AttackHandler.EndAttack();
        }

        public void TakeDamage(int damage, float knockBackForce, Vector2 knockBackDirection)
        {
            var lives = _enemyHealth.TakeDamage(damage);

            EnemyMovement.KnockBack(knockBackForce, knockBackDirection, jumpByKnockBack);
            
            if (lives <= 0)
            {
                GameControl.StaticGameControl.AddScore(ScoreOnDeath);
                Defeated(knockBackForce);
            }
            else
            {
                GameControl.StaticGameControl.AddScore(ScoreOnHit);
            }
        }

        protected virtual void Defeated(float knockBackForce)
        {
            // StartCoroutine(WaitKnockBack(knockBackForce)));
            Destroy(gameObject);
        }

        protected IEnumerator WaitKnockBack(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }

        private void CheckEnvironment()
        {
            _isCliffAhead = _enemyEnvironmentChecker.IsCliffAhead(_direction);
            _isWallAhead = _enemyEnvironmentChecker.IsWallAhead(_direction);
            IsInVision = _enemyEnvironmentChecker.CheckInVision(visionDistance);
            SetIsGrounded(_enemyEnvironmentChecker.CheckIsGrounded());
            
            EnemyMovement.UpdateChasingStatus(visionDistance);
        }

        private void PatrolMode()
        {
            EnemyMovement.Walk(_direction, _playerScript.GetIsInvulnerable());

            // Se encontrar parede ou não tiver chão, vira
            if (IsGrounded && (_isCliffAhead || _isWallAhead))
            {
                FlipByObstacle();
            }
        }

        private void FlipByObstacle()
        {
            EnemyMovement.FlipDirection();
            _direction = EnemyMovement.GetDirection();
        }

        private void GoToPointsMode(GameObject point)
        {
            if (point == null) return;
    
            var distanceToPoint = Vector2.Distance(transform.position, point.transform.position);
    
            // Se já chegou ao ponto, para e escolhe outro
            if (distanceToPoint < howDistanceToPointPermitted)
            {
                if (IsGrounded && howTimeToNextJump > 0)
                {
                    EnemyMovement.StopWalk();
                    _isWalking = false;
                    return;
                }
            }

            // Se não está andando, define a direção e começa a andar
            if (!_isWalking)
            {
                _direction = point.transform.position.x > transform.position.x ? 1 : -1;
                _isWalking = true;
            }

            // Se encontrar parede ou não tiver chão, vira
            if (IsGrounded && (_isCliffAhead || _isWallAhead))
            {
                FlipByObstacle();
                return;
            }

            EnemyMovement.Walk(_direction, _playerScript.GetIsInvulnerable());
        }
        
        // Método para atacar o jogador e se afastar
        private void AttackPlayer()
        {
            Vector2 direction = transform.position - Player.transform.position;
            _playerScript.TakeDamage(takeDamage, knockBackForceOnTouch, direction);
        }

        private void ZumbiMode()
        {
            if (Math.Abs(Player.transform.position.x - transform.position.x) < 0.1)
            {
                if (IsGrounded && howTimeToNextJump > 0)
                {
                    EnemyMovement.StopWalk();
                }
                
                if (Math.Abs(Player.transform.position.y - transform.position.y) < _collider2D.transform.localScale.y)
                {
                    AttackPlayer();
                }
                
                return;
            }
            
            // Se encontrar parede ou não tiver chão, Pula
            if (CanJump && (_isCliffAhead || _isWallAhead))
            {
                EnemyMovement.Jump();
                _timerToNextJump = howTimeToNextJump;
            }
            
            EnemyMovement.FlipToPlayer();
            _direction = EnemyMovement.GetDirection();
            EnemyMovement.Walk(_direction, _playerScript.GetIsInvulnerable());
        }

        private void FixedMode()
        {
            EnemyMovement.FlipToPlayer();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, visionDistance);
        }

        private void SetIsGrounded(bool value)
        {
            IsGrounded = value;
            ManageCanJump();
        }

        public void SetBlockJump(bool value)
        {
            _blockJump = value;
            ManageCanJump();
        }
        
        private void ManageCanJump()
        {
            CanJump = IsGrounded && _timerToNextJump <= 0 && !_blockJump;
        }
    }
}