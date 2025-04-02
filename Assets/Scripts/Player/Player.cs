using System.Collections;
using System.Collections.Generic;
using Resources;
using Respawn;
using UI;
using UnityEngine;

namespace Player
{
    public abstract class Player : MonoBehaviour
    {
        private HudControl _hudControl;
        private PlayerMovement _playerMovement;
        private PlayerHealth _playerHealth;
        private AnimationManager _animationManager;
        private PlayerEnvironmentChecker _playerEnvironmentChecker;
        
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private LayerMask walkLayers;
        [SerializeField] private float angleCapsule;
        [SerializeField] private float jumpForce;
        [SerializeField] private float moveForce;
        [SerializeField] private int lives;
        [SerializeField] private int scoreOnDamage;
        [SerializeField] private float howTimeToNextAttack;
        [SerializeField] private float howTimeToNextJump;
        [SerializeField] private float escJumpSpeed;
        [SerializeField] private float delayJumpTime;
        
        protected Dictionary<int, string> Awards;
        
        private bool _jump;
        private bool _jumpUI;
        private float _horizontal;
        private float _horizontalUI;
        private float _vertical;
        private float _verticalUI;
        private bool _attack;
        private bool _attackUI;
        private float _timerToNextAttack;
        private float _timerToNextJump;
        private SpriteRenderer _spriteRenderer;
        private bool _isInvulnerable;
        private AwardManager _awardManager;
        private Rigidbody2D _rigidbody2D;

        public abstract void AttackFromPlayer();

        protected void Awake()
        {
            ManagePlayerLivesOnAwake();

            _playerEnvironmentChecker = new PlayerEnvironmentChecker(feetPosition, sizeCapsule, walkLayers, angleCapsule, delayJumpTime);
            _animationManager = new AnimationManager(GetComponent<Animator>());
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerMovement = new PlayerMovement(this, escJumpSpeed, _animationManager, _rigidbody2D, _spriteRenderer, howTimeToNextJump);
            _playerHealth = new PlayerHealth(lives, scoreOnDamage);
        }

        protected void Start()
        {
            ManageRespawnStart();
            ManageHudControlStart();
            _awardManager = new AwardManager(Awards, _hudControl);
        }

        private void Update()
        {
            ManageInputs();
        }

        private void FixedUpdate()
        {
            var isGrounded = _playerEnvironmentChecker.CheckIsGrounded();
            
            ManageMovement(isGrounded);

            ManageAttack();

            ManageJump(isGrounded);
        }
        
        private void ManagePlayerLivesOnAwake()
        {
            lives = PlayerPrefs.GetInt("PlayerLives", 0) > 0 ? PlayerPrefs.GetInt("PlayerLives", 0) : lives;
            lives = lives <= 5 ? lives : lives <= 0 ? 1 : 5; // para garantir que não vai ter mais que 5 e nem nascer morto
        }

        private void ManageHudControlStart()
        {
            _hudControl = HudControl.StaticHudControl;

            if (_hudControl)
            {
                _hudControl.SetPlayerLivesInHud(lives);
            }
        }

        private void ManageRespawnStart()
        {
            var respawn = gameObject.GetComponent<PlayerRespawn>();
            respawn.Respawn();
        }
        
        private void ManageInputs()
        {
            _jump = _jumpUI ? _jumpUI : Input.GetButton("Jump");
            _horizontal = _horizontalUI != 0 ? _horizontalUI : Input.GetAxis("Horizontal");
            _attack = _attackUI ? _attackUI : Input.GetButton("Fire1");
            _vertical = 0;
            // vertical = Input.GetAxis("Vertical");
        }

        private void ManageMovement(bool isGrounded)
        {
            if (_horizontal != 0 || _vertical != 0)
            {
                _playerMovement.Move(_horizontal * moveForce, _vertical * moveForce);
                _animationManager.Move();
            }
            else
            {
                if (isGrounded && !_jump)
                {
                    _playerMovement.DontMove();
                }
                
                _animationManager.StopMove();
            }
        }

        private void ManageJump(bool isGrounded)
        {
            ManageJumpActions(isGrounded);
            
            _playerMovement.ManageJumpTimer(isGrounded);
            
            _animationManager.ManageJumpAnimations(isGrounded, _rigidbody2D.velocity.y);
        }

        private void ManageJumpActions(bool isGrounded)
        {
            var isDelayTouchGround = _playerEnvironmentChecker.CheckIsDelayTouchGround();
            
            if (_jump)
            {
                _playerMovement.Jump(jumpForce, isDelayTouchGround, isGrounded);
            }
            else
            {
                if (!isGrounded)
                {
                    _playerMovement.EscJump();
                }
            }
        }

        private void ManageAttack()
        {
            if (_attack)
            {
                _timerToNextAttack = _playerMovement.Attack();
            }

            ManageAttackTimer();
        }

        private void ManageAttackTimer()
        {
            if (_timerToNextAttack != 0)
            {
                if (_timerToNextAttack > 0)
                {
                    _timerToNextAttack -= howTimeToNextAttack * Time.fixedDeltaTime;
                }
                else if(_timerToNextAttack < 0 )
                {
                    _timerToNextAttack = 0;
                }

                var calculatePercent = CalculatePercentToNextAttack();

                if (_hudControl)
                {
                    _hudControl.UpdateImageAttackSize(calculatePercent);
                }
            }
        }

        private float CalculatePercentToNextAttack()
        {
            _playerMovement.SetTimerToNextAttack(_timerToNextAttack);
            
            var calculatePercent = 100 - _timerToNextAttack * 100;

            if (calculatePercent > 100)
            {
                calculatePercent = 100;
            }else if (calculatePercent < 0)
            {
                calculatePercent = 0;
            }

            return calculatePercent;
        }

        public void TakeDamage(int damage)
        {
            if (!_isInvulnerable)
            {
                var newLives = _playerHealth.TakeDamage(damage);
                _hudControl.SetPlayerLivesInHud(newLives);

                if (newLives > 0)
                {
                    StartCoroutine(Damage());
                }
            }
        }

        private IEnumerator Damage()
        {
            _isInvulnerable = true;
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = Color.white;

            for (var i = 0; i < 7; i++)
            {
                _spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.15f);
                _spriteRenderer.enabled = true;
                yield return new WaitForSeconds(0.15f);
            }
            
            _isInvulnerable = false;
        }

        public void GameOver()
        {
            _playerHealth.GameOver();
            _hudControl.SetPlayerLivesInHud(0);
        }

        public void MoveRight()
        {
            _horizontalUI = 1;
        }

        public void MoveRightExit()
        {
            _horizontalUI = 0;
        } 

        public void MoveLeft()
        {
            _horizontalUI = -1;
        }

        public void MoveLeftExit()
        {
            _horizontalUI = 0;
        }

        public void Jump()
        {
            _jumpUI = true;
        }

        public void JumpExit()
        {
            _jumpUI = false;
        }

        public void Attack()
        {
            _attackUI = true;
        }

        public void AttackExit()
        {
            _attackUI = false;
        }

        public void SetRandomAward()
        {
            _awardManager.SetRandomAward();
        }

        private void OnDrawGizmosSelected()
        {
            _playerEnvironmentChecker.OnDrawGizmosSelected();
        }
    }
}