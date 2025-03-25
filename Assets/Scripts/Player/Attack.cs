using Enemies;
using UnityEngine;

namespace Player
{
    public abstract class Attack : MonoBehaviour
    {
        private int _damage;
        private bool hasDamaged;
        protected GameObject Owner;
        protected float AttackVelocity;
        [SerializeField] private float autoDestructTime;

        public void Initialize(GameObject owner, float attackVelocity, int damage)
        {
            Owner = owner;
            AttackVelocity = attackVelocity;
            _damage = damage;

            if (Owner.CompareTag("Enemy"))
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        private void Update()
        {
            if (autoDestructTime > 0)
            {
                autoDestructTime -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (hasDamaged || !Owner) return; // Evita múltiplos danos

            if (Owner.CompareTag("Player") && col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Ataque do Player atingindo um inimigo
                var enemy = col.gameObject.GetComponentInParent<Enemy>();
                
                if (enemy)
                {
                    hasDamaged = true;
                    enemy.TakeDamage(_damage);
                    Destroy(gameObject);
                }
            }
            else if (Owner.CompareTag("Enemy") && col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Ataque do inimigo atingindo o Player
                var player = col.gameObject.GetComponent<Player>(); // Supondo que o Player tem um script de vida
                
                if (player)
                {
                    hasDamaged = true;
                    player.TakeDamage(_damage);
                }
            }
        }
    }
}