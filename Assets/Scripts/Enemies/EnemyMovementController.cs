using Resources;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovementController
    {
        private readonly Transform _player;
        private readonly Transform _enemy;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly AnimationManager _animationManager;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Collider2D _collider2D;
        private readonly float _howTimeToNextJump;
        private readonly float _chaseSpeed;
        private readonly float _moveSpeed;
        private readonly float _jumpForce;
        private readonly Vector2 _colliderDefault;
        private readonly Vector2 _colliderFlip;
        
        private int _direction;
        private float _timerToNextJump;
        private bool _isInVision;

        public EnemyMovementController(float chaseSpeed, Transform player, Transform enemy, Rigidbody2D rigidbody2D, AnimationManager animationManager, float howTimeToNextJump, SpriteRenderer spriteRenderer, Collider2D collider2D, float moveSpeed, float jumpForce)
        {
            _chaseSpeed = chaseSpeed;
            _player = player;
            _enemy = enemy;
            _rigidbody2D = rigidbody2D;
            _animationManager = animationManager;
            _howTimeToNextJump = howTimeToNextJump;
            _spriteRenderer = spriteRenderer;
            _collider2D = collider2D;
            _moveSpeed = moveSpeed;
            _jumpForce = jumpForce;

            _timerToNextJump = 0;
            _colliderDefault = new Vector2(collider2D.offset.x, collider2D.offset.y);
            _colliderFlip = new Vector2(collider2D.offset.x * -1, collider2D.offset.y);
            
            FlipToPlayer();
        }

        public void Walk(int direction)
        {
            var currentSpeed = _isInVision ? _chaseSpeed : _moveSpeed;
            
            SetDirection(direction);
            _rigidbody2D.velocity = new Vector2(direction * currentSpeed, _rigidbody2D.velocity.y);
            _animationManager.Move();
        }

        public void StopWalk(bool isGrounded)
        {
            if (isGrounded && _howTimeToNextJump > 0)
            {
                if (!_animationManager.GetBoolAnimator(Library.IsFalling))
                {
                    _rigidbody2D.velocity = new Vector2(0, 0);
                }
            }
        
            _animationManager.StopMove();
        }

        public void Jump(bool isGrounded)
        {
            if (_timerToNextJump > 0)
            {
                _timerToNextJump -= Time.fixedDeltaTime;
            }

            if (isGrounded && _timerToNextJump <= 0)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                _timerToNextJump = _howTimeToNextJump;
            }
        }
        
        public void FlipDirection()
        {
            SetDirection(_direction *= -1);

            // 🔥 Impede que ele ande imediatamente após virar
            _rigidbody2D.velocity = Vector2.zero; // Para o movimento
        }

        public void FlipToPlayer()
        {
            SetDirection(_player.transform.position.x < _enemy.position.x ? -1 : 1);
        }
        
        private void SetDirection(int direction)
        {
            _direction = direction;
            UpdateSpriteDirection();
        }

        public int GetDirection()
        {
            return _direction;
        }
        
        private void UpdateSpriteDirection()
        {
            _spriteRenderer.flipX = _direction == -1;
            _collider2D.offset = _spriteRenderer.flipX ? _colliderDefault : _colliderFlip;
        }

        public void UpdateChasingStatus(float visionDistance)
        {
            var distanceToPlayer = Vector2.Distance(_enemy.transform.position, _player.transform.position);
            _isInVision = distanceToPlayer <= visionDistance;
        }
    }
}