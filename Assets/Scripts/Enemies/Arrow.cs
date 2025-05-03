using System.Collections;
using Extensions;
using Player.Attacks;
using UnityEngine;

namespace Enemies
{
    public class Arrow : Attack
    {
        private float _arrowVelocity;
        private Rigidbody2D _rigidbodyArrow;
        private SpriteRenderer _spriteArrow;
        private bool _flipX;

        private void Start()
        {
            _rigidbodyArrow = GetComponent<Rigidbody2D>();
            _spriteArrow = GetComponent<SpriteRenderer>();
            _spriteArrow.flipX = _flipX;

            ArrowTrajectory();
        }

        public override void Initialize(GameObject owner, float arrowVelocity, int damage)
        {
            base.Initialize(owner, arrowVelocity, damage);
            _arrowVelocity = arrowVelocity;
            _flipX = owner.GetComponent<SpriteRenderer>().flipX;
        }

        public override void FlipMovement()
        {
            _flipX = !_flipX;
            
            var spriteEnergyBall = gameObject.GetComponentOnlyInChildren<SpriteRenderer>();
            
            if (spriteEnergyBall)
            {
                spriteEnergyBall.flipX = _flipX;
            }
            
            _spriteArrow.flipX = _flipX;
            
            ArrowTrajectory();
        }

        private void ArrowTrajectory()
        {
            var direction = _flipX ? Vector3.right : Vector3.left;
            var rotationAngle = _flipX ? 45f : -45f;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

            _rigidbodyArrow.velocity = direction * _arrowVelocity;
            StartCoroutine(RotateArrow(_flipX ? 45f : -45f, 1f));
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