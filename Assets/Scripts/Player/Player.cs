using System.Collections;
using System.Collections.Generic;
using Resources;
using Respawn;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private LayerMask walkLayers;
        [SerializeField] private float angleCapsule;
        [SerializeField] private float jumpForce;
        [SerializeField] private float moveForce;
        [SerializeField] private int lives;
        [SerializeField] private int scoreOnDamage;
        [SerializeField] private float howTimeToNextAttack;
        [SerializeField] private float escJumpSpeed;
        [SerializeField] private float delayJumpTime;
        [SerializeField] private float jumpByKnockBack = 1f;
        [SerializeField] private Image imageAttackBar;
        [SerializeField] private Image imageJumpBar;
        
        private bool _attack;
        private bool _attackUI;
        private bool _blockJump;
        private bool _canJump;
        private bool _isDelayTouchGround;
        private bool _isGrounded;
        private bool _isInvulnerable;
        private bool _isKnockBack;
        private bool _jump;
        private bool _jumpUI;
        
        private float _countJump;
        private float _horizontal;
        private float _horizontalUI;
        private float _timerToNextAttack;
        private float _vertical;
        private float _verticalUI;
        
        private AnimationManager _animationManager;
        private AwardManager _awardManager;
        private PlayerEnvironmentChecker _playerEnvironmentChecker;
        private PlayerHealth _playerHealth;
        private PlayerMovement _playerMovement;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        protected Dictionary<int, string> Awards;

        protected void Awake()
        {
            ManagePlayerLivesOnAwake();

            _playerEnvironmentChecker = new PlayerEnvironmentChecker(feetPosition, sizeCapsule, walkLayers, angleCapsule, delayJumpTime);
            _animationManager = new AnimationManager(GetComponent<Animator>());
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerMovement = new PlayerMovement(this, escJumpSpeed, _animationManager, _rigidbody2D, _spriteRenderer);
            _playerHealth = new PlayerHealth(lives, scoreOnDamage);
        }

        protected void Start()
        {
            ManageRespawnStart();
            ManageCanJump();
            HudControl.StaticHudControl.SetPlayerLivesInHud(lives);
            _awardManager = new AwardManager(Awards);
        }

        private void Update()
        {
            ManageInputs();
        }

        private void FixedUpdate()
        {
            SetIsGrounded(_playerEnvironmentChecker.CheckIsGrounded());
            SetIsDelayTouchGround(_playerEnvironmentChecker.CheckIsDelayTouchGround());

            ManageMovement();

            ManageAttack();

            ManageJump();
        }

        private void OnDrawGizmosSelected()
        {
            _playerEnvironmentChecker.OnDrawGizmosSelected();
        }

        public abstract void AttackFromPlayer();

        private void ManagePlayerLivesOnAwake()
        {
            lives = PlayerPrefs.GetInt("PlayerLives", 0) > 0 ? PlayerPrefs.GetInt("PlayerLives", 0) : lives;
            lives = lives <= 5 ? lives :
                lives <= 0 ? 1 : 5; // para garantir que não vai ter mais que 5 e nem nascer morto
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

        private void ManageMovement()
        {
            if (_horizontal != 0 || _vertical != 0)
            {
                _playerMovement.Move(_horizontal * moveForce, _vertical * moveForce);
                _animationManager.Move();
            }
            else
            {
                if (!_isKnockBack)
                {
                    if (_isGrounded && !_jump) _playerMovement.DontMove();
                }
                else
                {
                    _isKnockBack = false;
                }

                _animationManager.StopMove();
            }
        }

        private void ManageJump()
        {
            ManageJumpActions();

            _animationManager.ManageJumpAnimations(_isGrounded, _rigidbody2D.velocity.y);
        }

        private void ManageJumpActions()
        {
            if (_jump)
            {
                if (_canJump) _playerMovement.Jump(jumpForce);
            }
            else
            {
                if (!_isGrounded) _playerMovement.EscJump();
            }

            var percent = CalculatePercentToNextJump();
            HudControl.StaticHudControl.UpdateImageSize(percent, imageJumpBar);
        }

        private void ManageAttack()
        {
            if (_attack) _timerToNextAttack = _playerMovement.Attack();

            ManageAttackTimer();
        }

        private void ManageAttackTimer()
        {
            if (_timerToNextAttack != 0)
            {
                if (_timerToNextAttack > 0)
                    _timerToNextAttack -= howTimeToNextAttack * Time.fixedDeltaTime;
                else if (_timerToNextAttack < 0) _timerToNextAttack = 0;

                var calculatePercent = CalculatePercentToNextAttack();

                HudControl.StaticHudControl.UpdateImageSize(calculatePercent, imageAttackBar);
            }
        }

        private float CalculatePercentToNextJump()
        {
            if (!_canJump)
            {
                _countJump = 0;
                return 0;
            }

            if (_countJump > 100) _countJump = 100;

            _countJump += Time.deltaTime * 1000;

            return _countJump;
        }

        private float CalculatePercentToNextAttack()
        {
            _playerMovement.SetTimerToNextAttack(_timerToNextAttack);

            var calculatePercent = 100 - _timerToNextAttack * 100;

            if (calculatePercent > 100)
                calculatePercent = 100;
            else if (calculatePercent < 0) calculatePercent = 0;

            return calculatePercent;
        }

        public void TakeDamage(int damage, float knockBackForce, Vector2 direction)
        {
            if (!_isInvulnerable)
            {
                var newLives = _playerHealth.TakeDamage(damage);

                _playerMovement.KnockBack(knockBackForce, direction, jumpByKnockBack);
                _isKnockBack = true;
                
                if (newLives > 0)
                {
                    HudControl.StaticHudControl.SetPlayerLivesInHud(newLives);
                    StartCoroutine(Damage());
                }
                else
                {   
                    _playerHealth.GameOver();
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

        public bool GetIsInvulnerable()
        {
            return _isInvulnerable;
        }

        private void SetIsGrounded(bool value)
        {
            _isGrounded = value;
            ManageCanJump();
        }

        private void SetIsDelayTouchGround(bool value)
        {
            _isDelayTouchGround = value;
            ManageCanJump();
        }

        public void SetBlockJump(bool value)
        {
            _blockJump = value;
            ManageCanJump();
        }

        private void ManageCanJump()
        {
            _canJump = (_isGrounded || _isDelayTouchGround) && !_blockJump;
        }

        public void GameOver()
        {
            _playerHealth.GameOver();
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
    }
}