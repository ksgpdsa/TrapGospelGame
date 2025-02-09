using Enemies;
using UnityEngine;

namespace Player
{
    public abstract class Attack : MonoBehaviour
    {
        private int _damage;
        private bool hasDamaged;
        protected GameObject Player;
        protected float AttackVelocity;
        [SerializeField] private float autoDestructTime;
        
        public void Initialize(GameObject player, float attackVelocity, int damage)
        {
            Player = player;
            AttackVelocity = attackVelocity;
            _damage = damage;
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
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") && !hasDamaged)
            {
                var enemy = col.gameObject.GetComponentInParent<Enemy>();
                
                if (!enemy)
                {
                    return;
                }
                
                hasDamaged = true;
                enemy.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}