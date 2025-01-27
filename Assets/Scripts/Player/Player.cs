using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerHealth _playerHealth;
        private PlayerGroundCheck _playerGroundCheck;
        private LocalAnimator _localAnimator;
        
        [SerializeField] private float jumpForce;
        [SerializeField] private float moveForce;
        [SerializeField] private int lives;
        
        private bool _jump;
        private float _horizontal;
        private float _vertical;

        void Awake()
        {
            _playerMovement = new PlayerMovement(this);
            _playerHealth = new PlayerHealth(lives);
            _localAnimator = new LocalAnimator(GetComponent<Animator>());
            _playerGroundCheck = GetComponent<PlayerGroundCheck>();
        }

        void Update()
        {
            _jump = Input.GetButton("Jump");
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = 0;
            // vertical = Input.GetAxis("Vertical");
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
            _playerHealth.TakeDamage(damage);
        }

        public void GameOver()
        {
            _playerHealth.GameOver();
        }
    }
}