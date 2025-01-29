using System.Collections.Generic;
using UnityEngine;

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
        protected Dictionary<int, string> Awards;
        
        private bool _jump;
        private bool _jumpUI;
        private float _horizontal;
        private float _horizontalUI;
        private float _vertical;
        private float _verticalUI;
        private bool _attack;
        private bool _attackUI;

        protected void Awake()
        {
            lives = lives <= 5 ? lives : lives <= 0 ? 1 : 5; // para garantir que não vai ter mais que 5 e nem nascer morto
            _playerGroundCheck = new PlayerGroundCheck(feetPosition, sizeCapsule, groundLayer, angleCapsule);
            _playerMovement = new PlayerMovement(this, _playerGroundCheck);
            _playerHealth = new PlayerHealth(lives);
            _localAnimator = new LocalAnimator(GetComponent<Animator>());
        }

        void Start()
        {
            _hudControl = HudControl.HUDControl;
            _hudControl.SetPlayerLivesInHud(lives);
        }

        void Update()
        {
            _jump = _jumpUI ? _jumpUI : Input.GetButton("Jump");
            _horizontal = _horizontalUI != 0 ? _horizontalUI : Input.GetAxis("Horizontal");
            _vertical = 0;
            // vertical = Input.GetAxis("Vertical");

            _playerGroundCheck.Update();
        }

        void FixedUpdate()
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
            
            _playerMovement.ManageVelocityActions();
        }

        public void TakeDamage(int damage)
        {
            var newLives = _playerHealth.TakeDamage(damage);
            _hudControl.SetPlayerLivesInHud(newLives);
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