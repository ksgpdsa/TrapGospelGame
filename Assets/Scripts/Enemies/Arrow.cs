using System.Collections;
using Player.Attacks;
using UnityEngine;

namespace Enemies
{
    public class Arrow : Attack
    {
        private float _arrowVelocity;
        private bool flipX;
        private SpriteRenderer _spriteArrow;
        private Rigidbody2D _rigidbodyArrow;

        public override void Initialize(GameObject owner, float arrowVelocity, int damage)
        {
            base.Initialize(owner, arrowVelocity, damage);
            _arrowVelocity = arrowVelocity;
            flipX = !owner.GetComponent<SpriteRenderer>().flipX;
        }
        
        private void Start()
        {
            _rigidbodyArrow = GetComponent<Rigidbody2D>();
            _spriteArrow = GetComponent<SpriteRenderer>();
            
            _spriteArrow.flipX = flipX;
            
            ArrowTrajectory();
        }

        public override void FlipMovement()
        {
            _spriteArrow.flipX = !_spriteArrow.flipX;
            ArrowTrajectory();
        }

        private void ArrowTrajectory()
        {
            var direction = _spriteArrow.flipX ? Vector3.right : Vector3.left;
            var rotationAngle = _spriteArrow.flipX ? 45f : -45f;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
            
            _rigidbodyArrow.velocity = direction * _arrowVelocity;
            StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(RotateArrow(_spriteArrow.flipX ? 45f : -45f, 1f)));
        }

        private IEnumerator RotateArrow(float targetAngle, float duration)
        {
            var elapsed = 0f;
            var startAngle = transform.rotation.eulerAngles.z; // Obtém o ângulo inicial
            var endAngle = startAngle - targetAngle; // Calcula o ângulo final
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var angle = Mathf.Lerp(startAngle, endAngle, elapsed / duration); // Interpola o ângulo
                transform.rotation = Quaternion.Euler(0, 0, angle); // Aplica a rotação
                yield return null;
            }
            
            transform.rotation = Quaternion.Euler(0, 0, endAngle); // Garante que termine no ângulo correto
        }
    }
}