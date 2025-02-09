using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly LocalAnimator _localAnimator;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly PlayerGroundCheck _playerGroundCheck;
        private readonly Player _player;
        private float _timerToNextAttack;
        private float _timerToNextJump;
        
        public PlayerMovement(Player player, PlayerGroundCheck playerGroundCheck)
        {
            _player = player;
            _rigidbody2D = player.GetComponent<Rigidbody2D>();
            _spriteRenderer = player.GetComponent<SpriteRenderer>();
            _localAnimator = new LocalAnimator(player.GetComponent<Animator>());
            _playerGroundCheck = playerGroundCheck;
            
            _rigidbody2D.velocity = Vector2.zero;
        }
        
        public void Jump(float jumpForce)
        {
            if (_rigidbody2D.velocity.y == 0 && _timerToNextJump == 0)
            {
                Move(0, jumpForce);
                _timerToNextJump = 1;
            }
            else if (_rigidbody2D.velocity.y == 0)
            {
                EndJump();
            }
        }

        private void EndJump()
        {
            _timerToNextJump = 0;
        }
        
        public void SetTimerToNextJump(float newTimer)
        {
            _timerToNextJump = newTimer;
        }

        public void Move(float horizontal, float vertical)
        {
            _spriteRenderer.flipX = horizontal == 0 ? _spriteRenderer.flipX : horizontal < 0;

            horizontal = horizontal == 0 ? _rigidbody2D.velocity.x + horizontal : horizontal;
            vertical = vertical == 0 ? _rigidbody2D.velocity.y + vertical : vertical;
            
            _rigidbody2D.velocity = new Vector2(horizontal, vertical);
        }
        
        public float Attack()
        {
            if (_timerToNextAttack == 0)
            {
                _player.AttackFromPlayer();
                EndAttack();
            }

            return _timerToNextAttack;
        }

        private void EndAttack()
        {
            _timerToNextAttack = 1;
        }

        public void SetTimerToNextAttack(float newTimer)
        {
            _timerToNextAttack = newTimer;
        }

        public void DontMove()
        {
            if (!_localAnimator.GetBoolAnimator(Library.IsFalling))
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
        }

        public void ManageVelocityActions()
        {
            _localAnimator.SetBoolAnimator(Library.IsJumping, _rigidbody2D.velocity.y > 0);
            _localAnimator.SetBoolAnimator(Library.IsFalling, !_playerGroundCheck.GetIsGrounded() && _rigidbody2D.velocity.y < 0);
        }
    }
}