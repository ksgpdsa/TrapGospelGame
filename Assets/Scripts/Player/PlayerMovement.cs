using Resources;
using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly AnimationManager _animationManager;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Player _player;
        private readonly float _escJumpSpeed;
        private readonly float _howTimeToNextJump;
        
        private float _timerToNextAttack;
        private float _timerToNextJump;

        public PlayerMovement(Player player, float escJumpSpeed, AnimationManager animationManager, Rigidbody2D rigidbody2D, SpriteRenderer spriteRenderer, float howTimeToNextJump)
        {
            _player = player;
            _animationManager = animationManager;
            _rigidbody2D = rigidbody2D;
            _spriteRenderer = spriteRenderer;
            _escJumpSpeed = escJumpSpeed;
            _howTimeToNextJump = howTimeToNextJump;

            _rigidbody2D.velocity = Vector2.zero;
        }
        
        public void Jump(float jumpForce, bool isDelayTouchGround, bool isGrounded)
        {
            if ((isGrounded || isDelayTouchGround) && _timerToNextJump == 0)
            {
                Move(0, jumpForce);
                _timerToNextJump = _howTimeToNextJump;
            }
        }

        public void EscJump()
        {
            if (_rigidbody2D.velocity.y > 0.5)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * _escJumpSpeed);
            }
        }

        public void ManageJumpTimer(bool isGrounded)
        {
            if (_timerToNextJump != 0)
            {
                if (_timerToNextJump < 0 || isGrounded)
                {
                    _timerToNextJump = 0;
                }
                else if (_timerToNextJump > 0)
                {
                    _timerToNextJump -= Time.fixedDeltaTime;
                }
            }
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
            if (!_animationManager.GetBoolAnimator(Library.IsFalling))
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
        }
    }
}