using Player;
using UI;
using UnityEngine;

namespace Enemies
{
    public class KsbatistaEnemy : CharacterEnemy
    {
        protected override int ScoreOnDeath => 1000;
        protected override int ScoreOnHit => 100;
        protected override bool InstantiateAttackByAnimation => false;
        protected override bool HasZumbiMode => false;
        protected override bool HasPatrolMode => false;
        protected override bool HasFixedMode => false;
        protected override bool HasGoToPointsMode => true;
        
        protected override void InstantiateAttack()
        {
            var attackPosition = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.1f, transform.position.z);
            
            var newAttack = Instantiate(attack, attackPosition, Quaternion.identity);
            var script = newAttack.GetComponent<Attack01>();

            script.Initialize(gameObject, attackVelocity, takeDamage);
        }

        protected override void Defeated()
        {
            StartCoroutine(HudControl.StaticHudControl.DefeatScene(ThisSprite,nextScene)); 
        }
    }
}
