using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Arrow : MonoBehaviour
    {
        private float _arrowVelocity;
        private int _damage;
        private bool hasDamaged;
        private bool flipX;
        
        public void Initialize(bool enemyFlipX, float arrowVelocity, int damage)
        {
            _arrowVelocity = arrowVelocity;
            _damage = damage;
            
            flipX = !enemyFlipX;
        }
        
        private void Start()
        {
            var rigidbodyArrow = GetComponent<Rigidbody2D>();
            var spriteArrow = GetComponent<SpriteRenderer>();
            
            spriteArrow.flipX = flipX;
            
            var direction = spriteArrow.flipX ? Vector3.right : Vector3.left;
            
            var rotationAngle = spriteArrow.flipX ? 45f : -45f;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
            
            rigidbodyArrow.velocity = direction * _arrowVelocity;
            StartCoroutine(RotateArrow(spriteArrow.flipX ? 45f : -45f, 1f));
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player") && !hasDamaged)
            {
                var player = col.gameObject.GetComponent<Player.Player>();
                
                if (!player)
                {
                    return;
                }
                
                hasDamaged = true;
                player.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}