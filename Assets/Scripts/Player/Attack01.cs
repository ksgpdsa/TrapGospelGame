using UnityEngine;

namespace Player
{
    public class Attack01 : Attack
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
            var direction = !_spriteAttack.flipX ? Vector3.right : Vector3.left;
            _rigidBodyAttack.velocity = direction * AttackVelocity;
        }
    }
}