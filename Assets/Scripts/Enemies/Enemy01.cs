using System;
using UnityEngine;

namespace Enemies
{
    public class Enemy01 : Enemy
    {
        protected override int ScoreOnDeath => 200;
        protected override int ScoreOnHit => 0;
        protected override bool InstantiateAttackByAnimation => true;
        protected override bool HasZumbiMode => false;
        protected override bool HasPatrolMode => false;
        protected override bool HasFixedMode => true;
        protected override bool HasGoToPointsMode => false;

        protected override void InstantiateAttack()
        {
            var newArrow = Instantiate(attack, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
            var script = newArrow.GetComponent<Arrow>();
            
            script.Initialize(gameObject.GetComponent<SpriteRenderer>().flipX, attackVelocity, takeDamage);
        }

        private void Update()
        {
            AttackHandler.ManageAttacks(IsInVision, InstantiateAttack, InstantiateAttackByAnimation);
        }
    }
}
