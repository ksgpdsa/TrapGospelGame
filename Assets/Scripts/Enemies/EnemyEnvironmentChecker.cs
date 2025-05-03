using Resources;
using UnityEngine;

namespace Enemies
{
    public class EnemyEnvironmentChecker : EnvironmentChecker
    {
        private const float GroundCheckDistance = 0.5f;
        private const float WallCheckDistance = 0.2f;

        private readonly Transform _enemy;
        private readonly GroundCheck _groundCheck;
        private readonly Transform _player;
        private readonly LayerMask _walkLayers;
        private int _direction;

        public EnemyEnvironmentChecker(LayerMask walkLayers, Transform enemy, Transform player, Transform feetPosition,
            Vector2 sizeCapsule, float angleCapsule, float delayJumpTime) : base(feetPosition, sizeCapsule, walkLayers,
            angleCapsule, delayJumpTime)
        {
            _walkLayers = walkLayers;
            _enemy = enemy;
            _player = player;
        }

        public bool IsCliffAhead(int direction)
        {
            for (var xOffset = -0.4f; xOffset <= 0.4f; xOffset += 0.2f)
            {
                var checkPosition = new Vector2(_enemy.position.x + direction * GroundCheckDistance + xOffset,
                    _enemy.position.y);
                var hit = Physics2D.Raycast(checkPosition, Vector2.down, 1f, _walkLayers);
                Debug.DrawLine(checkPosition, checkPosition + Vector2.down * 1f, Color.magenta);

                if (hit.collider != null) return false;
            }

            return true;
        }

        public bool IsWallAhead(int direction)
        {
            var checkPosition = new Vector2(_enemy.position.x, _enemy.position.y);
            var directionVector = direction == 1 ? Vector2.right : Vector2.left;

            var hit = Physics2D.Raycast(checkPosition, directionVector, WallCheckDistance, _walkLayers);
            Debug.DrawLine(checkPosition, checkPosition + directionVector * WallCheckDistance, Color.red);

            return hit.collider != null;
        }

        public bool CheckInVision(float visionDistance)
        {
            return Mathf.Abs(_player.position.x - _enemy.position.x) < visionDistance &&
                   Mathf.Abs(_player.position.y - _enemy.position.y) < visionDistance;
        }
    }
}