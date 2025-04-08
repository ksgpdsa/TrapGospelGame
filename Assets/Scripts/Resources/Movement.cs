using UnityEngine;

namespace Resources
{
    public abstract class Movement
    {
        private readonly Rigidbody2D _rigidbody2D;

        protected Movement(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public void KnockBack(float force, Vector2 direction, float jumpByKnockBack)
        {
            direction.y += jumpByKnockBack;
            
            _rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}