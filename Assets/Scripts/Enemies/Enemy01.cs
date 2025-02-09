using UnityEngine;

namespace Enemies
{
    public class Enemy01 : Enemy
    {
        protected override int ScoreOnDeath => 200;

        protected override void InstantiateAttack()
        {
            var newArrow = Instantiate(arrow, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
            var script = newArrow.GetComponent<Arrow>();
            
            script.Initialize(gameObject.GetComponent<SpriteRenderer>().flipX, attackVelocity, takeDamage);
        }
    }
}
