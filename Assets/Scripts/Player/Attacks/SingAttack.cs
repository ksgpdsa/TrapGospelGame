using UnityEngine;

namespace Player.Attacks
{
    public class SingAttack : Attack
    {
        private Rigidbody2D _rigidBodyAttack;
        private SpriteRenderer _spriteAttack;
        private SpriteRenderer _spritePlayer;

        private bool _flipX;

        private void Start()
        {
            _spritePlayer = Owner.GetComponent<SpriteRenderer>();
            _spriteAttack = GetComponent<SpriteRenderer>();
            _rigidBodyAttack = GetComponent<Rigidbody2D>();

            _flipX = _spritePlayer.flipX;
            _spriteAttack.flipX = _flipX;
            AttackTrajectory();
        }

        public override void FlipMovement()
        {
            _flipX = !_flipX;
            _spriteAttack.flipX = _flipX;
            AttackTrajectory();
        }

        private void AttackTrajectory()
        {
            var direction = _flipX ? Vector3.left : Vector3.right;
            _rigidBodyAttack.velocity = direction * AttackVelocity;
        }
    }
}