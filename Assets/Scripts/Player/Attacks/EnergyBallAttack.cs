using Enemies;
using Extensions;
using UnityEngine;

namespace Player.Attacks
{
    public class EnergyBallAttack : Attack
    {
        public void Start()
        {
            // Encontra todas as bolas de energia existentes na cena
            var allEnergyBalls = FindObjectsOfType<EnergyBallAttack>();
            var hasOther = allEnergyBalls.Length > 1;
            var hasOtherOfThisOwner = false;

            if (hasOther)
            {
                foreach (var energyBall in allEnergyBalls)
                    if (energyBall.Owner == Owner)
                    {
                        Destroy(energyBall.gameObject);
                        hasOtherOfThisOwner = true;
                    }
            }
            else
            {
                transform.SetParent(Owner.gameObject.transform);
                transform.position = Vector3.zero + Owner.transform.position;
            }

            if (Owner.CompareTag("Player"))
            {
                var player = Owner.GetComponent<Player>();

                player.SetBlockJump(!hasOtherOfThisOwner);
            }
            else if (Owner.CompareTag("Enemy"))
            {
                var enemy = Owner.GetComponent<Enemy>();
                enemy.SetBlockJump(!hasOtherOfThisOwner);
            }
        }

        private void OnDestroy()
        {
            if (Owner.CompareTag("Player"))
            {
                var player = Owner.GetComponent<Player>();

                player.SetBlockJump(false);
            }
            else if (Owner.CompareTag("Enemy"))
            {
                var enemy = Owner.GetComponent<Enemy>();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Attack"))
            {
                var attack = col.gameObject;
                var attackScript = attack.GetComponent<Attack>();
                
                var energy = attack.GetComponentOnlyInChildren<SpriteRenderer>();
                if (energy)
                {
                    energy.enabled = true;
                }

                attackScript.FlipMovement();
                attackScript.SetOwner(Owner);
            }

            base.OnTriggerEnter2D(col);
        }

        public override void FlipMovement()
        {
            Destroy(gameObject);
        }
    }
}