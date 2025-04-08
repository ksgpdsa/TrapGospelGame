using UnityEngine;

namespace Player.Attacks
{
    public class SingAttack : Attack
    {
        private Rigidbody2D _rigidBodyAttack;
        private SpriteRenderer _spriteAttack;
        private SpriteRenderer _spritePlayer;

        private void Start()
        {
            _spritePlayer = Owner.GetComponent<SpriteRenderer>();
            _spriteAttack = GetComponent<SpriteRenderer>();
            _rigidBodyAttack = GetComponent<Rigidbody2D>();
            
            _spriteAttack.flipX = _spritePlayer.flipX;
            AttackTrajectory();
        }
        
        public override void FlipMovement()
        {
            _spriteAttack.flipX = !_spriteAttack.flipX;
            AttackTrajectory();
        }

        private void AttackTrajectory()
        {
            var direction = !_spriteAttack.flipX ? Vector3.right : Vector3.left;
            _rigidBodyAttack.velocity = direction * AttackVelocity;
        }
    }
}