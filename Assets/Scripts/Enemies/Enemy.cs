using System;
using System.Collections.Generic;
using Resources;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected GameObject Player;
        protected Sprite ThisSprite;
        protected AnimationManager AnimationManager;
        protected Rigidbody2D Rigidbody2D;
        protected EnemyMovementController EnemyMovement;
        protected EnemyAttackHandler AttackHandler;
        protected bool IsInVision;
        protected bool IsGrounded;
        
        private Collider2D _collider2D;
        private EnemyHealth _enemyHealth;
        private SpriteRenderer _spriteRenderer;
        private EnemyEnvironmentChecker _enemyEnvironmentChecker;
        private GameObject _currentPoint;
        private bool _isWallAhead;
        private bool _isCliffAhead;
        private int _direction;
        private bool _isWalking;

        [Header("Vida")]
        [SerializeField] private int enemyLives;
        
        [Header("Ataque")]
        [SerializeField] protected float visionDistance;
        [SerializeField] protected GameObject attack;
        [SerializeField] protected float attackVelocity;
        [SerializeField] private float attackFrequency;
        [SerializeField] protected int takeDamage;
        [SerializeField] private float howTimeToNextAttack;
        
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
            
            AnimationManager = new AnimationManager(GetComponent<Animator>());
            _enemyEnvironmentChecker = new EnemyEnvironmentChecker(walkLayers, transform, Player.transform, feetPosition, sizeCapsule, angleCapsule, 0);
            EnemyMovement = new EnemyMovementController(chaseSpeed, Player.transform, transform, Rigidbody2D, AnimationManager, howTimeToNextJump, _spriteRenderer, _collider2D, moveSpeed, jumpForce);
            AttackHandler = new EnemyAttackHandler(attackFrequency, AnimationManager);

            _enemyHealth.StartLivesInHud();
            _direction = EnemyMovement.GetDirection();
        }

        protected void FixedUpdate()
        {
            CheckEnvironment();

            _direction = EnemyMovement.GetDirection();

            MovementMode();
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

        public void TakeDamage(int damage)
        {
            var lives = _enemyHealth.TakeDamage(damage);
            
            if (lives <= 0)
            {
                HudControl.StaticHudControl.AddScore(ScoreOnDeath);
                Defeated();
            }
            else
            {
                HudControl.StaticHudControl.AddScore(ScoreOnHit);
            }
        }

        protected virtual void Defeated()
        {
            Destroy(gameObject);
        }

        private void CheckEnvironment()
        {
            _isCliffAhead = _enemyEnvironmentChecker.IsCliffAhead(_direction);
            _isWallAhead = _enemyEnvironmentChecker.IsWallAhead(_direction);
            IsInVision = _enemyEnvironmentChecker.CheckInVision(visionDistance);
            IsGrounded = _enemyEnvironmentChecker.CheckIsGrounded();
            
            EnemyMovement.UpdateChasingStatus(visionDistance);
        }

        private void PatrolMode()
        {
            EnemyMovement.Walk(_direction);

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
                EnemyMovement.StopWalk(IsGrounded);
                _isWalking = false;
                return;
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

            EnemyMovement.Walk(_direction);
        }

        private void ZumbiMode()
        {
            if (Math.Abs(Player.transform.position.x - transform.position.x) < 0.1)
            {
                EnemyMovement.StopWalk(IsGrounded);
                return;
            }
            
            // Se encontrar parede ou não tiver chão, vira
            if (IsGrounded && (_isCliffAhead || _isWallAhead))
            {
                EnemyMovement.Jump(IsGrounded);
            }
            
            EnemyMovement.FlipToPlayer();
            _direction = EnemyMovement.GetDirection();
            EnemyMovement.Walk(_direction);
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
    }
}