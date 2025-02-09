using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Player
{
    public abstract class Player : MonoBehaviour
    {
        private HudControl _hudControl;
        private PlayerMovement _playerMovement;
        private PlayerHealth _playerHealth;
        private PlayerGroundCheck _playerGroundCheck;
        private LocalAnimator _localAnimator;
        
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float angleCapsule;
        [SerializeField] private float jumpForce;
        [SerializeField] private float moveForce;
        [SerializeField] private int lives;
        [SerializeField] private int scoreOnDamage;
        [SerializeField] private float howTimeToNextAttack;
        [SerializeField] private float howTimeToNextJump;
        
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
        private BoxCollider2D _boxCollider;

        public abstract void AttackFromPlayer();

        protected void Awake()
        {
            lives = lives <= 5 ? lives : lives <= 0 ? 1 : 5; // para garantir que não vai ter mais que 5 e nem nascer morto
            _playerGroundCheck = new PlayerGroundCheck(feetPosition, sizeCapsule, groundLayer, angleCapsule);
            _playerMovement = new PlayerMovement(this, _playerGroundCheck);
            _playerHealth = new PlayerHealth(lives, scoreOnDamage);
            _localAnimator = new LocalAnimator(GetComponent<Animator>());
        }

        protected void Start()
        {
            _hudControl = HudControl.StaticHudControl;
            _hudControl.SetPlayerLivesInHud(lives);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            _jump = _jumpUI ? _jumpUI : Input.GetButton("Jump");
            _horizontal = _horizontalUI != 0 ? _horizontalUI : Input.GetAxis("Horizontal");
            _attack = _attackUI ? _attackUI : Input.GetButton("Fire1");
            _vertical = 0;
            // vertical = Input.GetAxis("Vertical");

            _playerGroundCheck.Update();
        }

        private void FixedUpdate()
        {
            if (_jump)
            {
                _playerMovement.Jump(jumpForce);
            }
            
            if (_horizontal != 0 || _vertical != 0)
            {
                _playerMovement.Move(_horizontal * moveForce, _vertical * moveForce);
                _localAnimator.SetBoolAnimator(Library.IsMoving, true);
            }
            else
            {
                if (_playerGroundCheck.GetIsGrounded() && !_jump)
                {
                    _playerMovement.DontMove();
                }
                
                _localAnimator.SetBoolAnimator(Library.IsMoving, false);
            }

            if (_attack)
            {
                _timerToNextAttack = _playerMovement.Attack();
            }

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
                
                _playerMovement.SetTimerToNextAttack(_timerToNextAttack);

                var calculatePercent = 100 - _timerToNextAttack * 100;

                if (calculatePercent > 100)
                {
                    calculatePercent = 100;
                }else if (calculatePercent < 0)
                {
                    calculatePercent = 0;
                }
                
                HudControl.StaticHudControl.UpdateImageAttackSize(calculatePercent);
            }

            if (_timerToNextJump != 0)
            {
                if (_timerToNextJump > 0)
                {
                    _timerToNextJump -= howTimeToNextJump * Time.fixedDeltaTime;
                }
                else if (_timerToNextJump < 0)
                {
                    _timerToNextJump = 0;
                }
                
                _playerMovement.SetTimerToNextJump(_timerToNextJump);
            }
            
            _playerMovement.ManageVelocityActions();
        }

        public void TakeDamage(int damage)
        {
            var newLives = _playerHealth.TakeDamage(damage);
            _hudControl.SetPlayerLivesInHud(newLives);

            if (newLives > 0)
            {
                StartCoroutine(Damage());
            }
        }

        private IEnumerator Damage()
        {
            _boxCollider.enabled = false;
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
            _boxCollider.enabled = true;
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
            if (Awards.Count > 0)
            {
                var randomAward = GetRandomEntry(Awards);
                
                _hudControl.ShowAward(randomAward.Value);
            }
        }

        private static KeyValuePair<int, string> GetRandomEntry(Dictionary<int, string> dict)
        {
            var keys = new List<int>(dict.Keys);
            
            var random = new System.Random();
            var randomIndex = random.Next(keys.Count);
            
            var randomKey = keys[randomIndex];
            var value = dict[randomKey];
            
            dict.Remove(randomKey);
            
            return new KeyValuePair<int, string>(randomKey, value);
        }
    }
}