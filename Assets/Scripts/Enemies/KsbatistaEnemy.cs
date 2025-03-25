using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enemies
{
    public class KsbatistaEnemy : Enemy
    {
        
        private PlayerGroundCheck _playerGroundCheck;
        protected override int ScoreOnDeath => 1000;
        protected override int ScoreOnHit => 100;
        protected override void InstantiateAttack()
        {
            var attackPosition = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.1f, transform.position.z);
            
            var newAttack = Instantiate(attack, attackPosition, Quaternion.identity);
            var script = newAttack.GetComponent<Attack01>();

            script.Initialize(gameObject, attackVelocity, takeDamage);
        }

        [Header("Configuração do Movimento")]
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float angleCapsule;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float chaseSpeed = 1.2f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private Transform groundCheckFront;
        [SerializeField] private Transform groundCheckBack;
        [SerializeField] private float groundCheckDistance = 0.5f;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private float wallCheckDistance = 0.3f;

        [Header("Comportamento")]
        [SerializeField] private string nextScene;
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float decisionInterval = 3f;
        [SerializeField] private float jumpCooldown = 10f;
        [SerializeField] private float howTimeToNextAttack;
        
        private GameObject _player;
        private float timerToNextAttack;
        private float _decisionTimer;
        private float _jumpTimer;
        private int _direction = -1; 
        private bool _isChasing = false;
        private bool _isMoving = false;

        private new void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerGroundCheck = new PlayerGroundCheck(feetPosition, sizeCapsule, groundLayer, angleCapsule);
            _decisionTimer = decisionInterval;
            _jumpTimer = jumpCooldown;
            base.Start();
        }

        private new void Update()
        {
            _playerGroundCheck.Update();
            base.Update();
        }
        
        private void FixedUpdate()
        {
            MakeDecision();
            CheckEnvironment();
            Jump();
            ManageVelocityActions();
            ManageTimeToNextAttack();
        }

        private void ManageTimeToNextAttack()
        {
            if (timerToNextAttack != 0)
            {
                if (timerToNextAttack > 0)
                {
                    timerToNextAttack -= howTimeToNextAttack * Time.fixedDeltaTime;
                }
                else if(timerToNextAttack < 0 )
                {
                    timerToNextAttack = 0;
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void MakeDecision()
        {
            _decisionTimer -= Time.fixedDeltaTime;

            if (_decisionTimer <= 0)
            {
                _decisionTimer = decisionInterval;

                var randomChoice = Random.value;

                if (randomChoice < 0.4f)
                {
                    _isMoving = true;
                    Move();
                }
                else if (randomChoice < 0.7f)
                {
                    _isMoving = false;
                    Move();
                }

                if (randomChoice < 0.5f)
                {
                    Attack();
                }
            }
        }

        private void CheckEnvironment()
        {
            // bool isGroundAhead = Physics2D.Raycast(groundCheckFront.position, Vector2.down, groundCheckDistance, groundLayer);
            // bool isGroundBack = Physics2D.Raycast(groundCheckBack.position, Vector2.down, groundCheckDistance, groundLayer);
            bool isWallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * _direction, wallCheckDistance, groundLayer);
            var isGrounded = _playerGroundCheck.GetIsGrounded();

            var distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
            _isChasing = distanceToPlayer <= detectionRange;
            
            // Se encontrar parede ou não tiver chão, vira
            if (isGrounded && (/*!isGroundAhead ||*/ isWallAhead))
            {
                FlipDirection();

                // 🔥 Se depois de virar ainda não tiver chão, para de andar!
                // if (!isGroundBack || !isGroundAhead)
                // {
                //     Rigidbody2D.velocity = Vector2.zero; // Para o movimento
                // }
            }
        }

        private void Move()
        {
            if (!_isMoving)
            {
                if (_playerGroundCheck.GetIsGrounded() && jumpCooldown > 0)
                {
                    if (!LocalAnimator.GetBoolAnimator(Library.IsFalling))
                    {
                        Rigidbody2D.velocity = new Vector2(0, 0);
                    }
                }
        
                LocalAnimator.SetBoolAnimator(Library.IsMoving, false);
        
                return;
            }

            var currentSpeed = _isChasing ? chaseSpeed : moveSpeed;

            // Obtemos a direção do jogador
            float directionToPlayer = _player.transform.position.x > transform.position.x ? 1 : -1;

            // Verificar se o inimigo está no ar (não está no chão)
            var isGrounded = _playerGroundCheck.GetIsGrounded();
            
            if (!isGrounded)
            {
                // Verificar se há chão abaixo enquanto ele se move
                bool isGroundAhead = Physics2D.Raycast(groundCheckFront.position, Vector2.down, groundCheckDistance, groundLayer);

                // Se não houver chão na frente dele, ele vai mudar a direção para evitar cair no buraco
                if (!isGroundAhead)
                {
                    directionToPlayer *= -1; // Inverte a direção, indo para o lado oposto
                }
            }

            // Agora, o inimigo vai se mover em direção ao jogador enquanto estiver no ar ou no chão
            Rigidbody2D.velocity = new Vector2(directionToPlayer * currentSpeed, Rigidbody2D.velocity.y);
            LocalAnimator.SetBoolAnimator(Library.IsMoving, true);
        }

        private void Jump()
        {
            var isGrounded = _playerGroundCheck.GetIsGrounded();

            if (_jumpTimer > 0)
            {
                _jumpTimer -= Time.fixedDeltaTime;
            }

            if (isGrounded && _jumpTimer <= 0)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpForce);
                _jumpTimer = jumpCooldown;
            }
        }

        private float Attack()
        {
            if (timerToNextAttack == 0)
            {
                InstantiateAttack();
                EndAttack();
            }
        
            return timerToNextAttack;
        }
        
        private void EndAttack()
        {
            timerToNextAttack = 1;
        }

        private void ManageVelocityActions()
        {
            var isGrounded = _playerGroundCheck.GetIsGrounded();
            LocalAnimator.SetBoolAnimator(Library.IsJumping, !isGrounded && Rigidbody2D.velocity.y > 0);
            LocalAnimator.SetBoolAnimator(Library.IsFalling, !isGrounded && Rigidbody2D.velocity.y < 0);
        }

        private void FlipDirection()
        {
            _direction *= -1;
            SpriteRenderer.flipX = _direction == -1;

            var groundCheckPos = groundCheckFront.localPosition;
            groundCheckPos.x *= -1;
            groundCheckFront.localPosition = groundCheckPos;

            var wallCheckPos = wallCheck.localPosition;
            wallCheckPos.x *= -1;
            wallCheck.localPosition = wallCheckPos;

            // 🔥 Impede que ele ande imediatamente após virar
            Rigidbody2D.velocity = Vector2.zero; // Para o movimento
        }

        protected override void Defeated()
        {   
            StartCoroutine(HudControl.StaticHudControl.DefeatScene(thisSprite,nextScene)); 
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(groundCheckFront.position, groundCheckFront.position + Vector3.down * groundCheckDistance);
        //     Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * _direction * wallCheckDistance);
        //
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawWireSphere(transform.position, detectionRange);
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawWireSphere(transform.position, attackRange);
        // }
    }
}
