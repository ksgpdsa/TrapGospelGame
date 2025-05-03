using Resources;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovementController : Movement
    {
        private readonly AnimationManager _animationManager;
        private readonly float _chaseSpeed;
        private readonly Collider2D _collider2D;
        private readonly Vector2 _colliderDefault;
        private readonly Vector2 _colliderFlip;
        private readonly Transform _enemy;
        private readonly float _jumpForce;
        private readonly float _moveSpeed;
        private readonly Transform _player;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly SpriteRenderer _spriteRenderer;

        private int _direction;
        private bool _isInVision;
        private bool _flipX;

        public EnemyMovementController(float chaseSpeed, Transform player, Transform enemy, Rigidbody2D rigidbody2D, AnimationManager animationManager, SpriteRenderer spriteRenderer, Collider2D collider2D, float moveSpeed, float jumpForce) : base(rigidbody2D)
        {
            _chaseSpeed = chaseSpeed;
            _player = player;
            _enemy = enemy;
            _rigidbody2D = rigidbody2D;
            _animationManager = animationManager;
            _spriteRenderer = spriteRenderer;
            _collider2D = collider2D;
            _moveSpeed = moveSpeed;
            _jumpForce = jumpForce;

            _flipX = _spriteRenderer.flipX;
            _colliderDefault = new Vector2(collider2D.offset.x, collider2D.offset.y);
            _colliderFlip = new Vector2(collider2D.offset.x * -1, collider2D.offset.y);

            FlipToPlayer();
        }

        public void Walk(int direction, bool isInvulnerable)
        {
            if (isInvulnerable) return;

            var currentSpeed = _isInVision ? _chaseSpeed : _moveSpeed;

            SetDirection(direction);
            _rigidbody2D.velocity = new Vector2(direction * currentSpeed, _rigidbody2D.velocity.y);
            _animationManager.Move();
        }

        public void StopWalk()
        {
            if (!_animationManager.GetBoolAnimator(Library.IsFalling)) _rigidbody2D.velocity = new Vector2(0, 0);

            _animationManager.StopMove();
        }

        public void Jump()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
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
            _flipX = _direction == 1;
            _spriteRenderer.flipX = _flipX;
            _collider2D.offset = _flipX ? _colliderDefault : _colliderFlip;
        }

        public void UpdateChasingStatus(float visionDistance)
        {
            var distanceToPlayer = Vector2.Distance(_enemy.transform.position, _player.transform.position);
            _isInVision = distanceToPlayer <= visionDistance;
        }
    }
}