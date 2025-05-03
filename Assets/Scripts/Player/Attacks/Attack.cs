using Enemies;
using UnityEngine;

namespace Player.Attacks
{
    public abstract class Attack : MonoBehaviour
    {
        [SerializeField] private bool isPermanent;
        [SerializeField] private float autoDestructTime;
        [SerializeField] private float knockBackForce;
        private int _damage;
        protected float AttackVelocity;
        private bool _hasDamaged;

        protected GameObject Owner;

        private void Update()
        {
            if (!isPermanent)
            {
                if (autoDestructTime > 0)
                    autoDestructTime -= Time.deltaTime;
                else
                    Destroy(gameObject);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (_hasDamaged || !Owner) return; // Evita múltiplos danos

            if ((Owner.CompareTag("Player") || Owner.CompareTag("Attack")) &&
                col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                TakeDamageOnEnemy(col);
            else if ((Owner.CompareTag("Enemy") || Owner.CompareTag("Attack")) &&
                     col.gameObject.layer == LayerMask.NameToLayer("Player")) TakeDamageOnPlayer(col);
        }

        public abstract void FlipMovement();

        public virtual void Initialize(GameObject owner, float attackVelocity, int damage)
        {
            Owner = owner;
            AttackVelocity = attackVelocity;
            _damage = damage;

            if (Owner.CompareTag("Enemy")) gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public void SetOwner(GameObject owner)
        {
            Owner = owner;
        }

        private void TakeDamageOnPlayer(Collider2D col)
        {
            // Ataque do inimigo atingindo o Player
            var player = col.gameObject.GetComponent<Player>();

            if (player)
            {
                _hasDamaged = true;
                Vector2 direction = player.transform.position - transform.position;
                player.TakeDamage(_damage, knockBackForce, direction);
                Destroy(gameObject);
            }
        }

        private void TakeDamageOnEnemy(Collider2D col)
        {
            // Ataque do Player atingindo um inimigo
            var enemy = col.gameObject.GetComponentInParent<Enemy>();

            if (enemy)
            {
                _hasDamaged = true;
                Vector2 direction = enemy.transform.position - transform.position;
                enemy.TakeDamage(_damage, knockBackForce, direction);
                Destroy(gameObject);
            }
        }
    }
}