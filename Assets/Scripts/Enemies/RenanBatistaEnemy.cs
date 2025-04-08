using System.Collections;
using Player.Attacks;
using UI;
using UnityEngine;

namespace Enemies
{
    public class RenanBatistaEnemy : CharacterEnemy
    {
        protected override int ScoreOnDeath => 2000;
        protected override int ScoreOnHit => 50;
        protected override bool InstantiateAttackByAnimation => false;
        protected override bool HasZumbiMode => true;
        protected override bool HasPatrolMode => true;
        protected override bool HasFixedMode => false;
        protected override bool HasGoToPointsMode => false;
        
        protected override void InstantiateAttack()
        {
            var attackPosition = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.1f, transform.position.z);
            
            var newAttack = Instantiate(attack, attackPosition, Quaternion.identity);
            var script = newAttack.GetComponent<Attack>();

            script.Initialize(gameObject, attackVelocity, takeDamage);
        }

        protected override void Defeated(float knockBackForce)
        {
            PlayerPrefs.SetInt(Library.PlayerPrefsPurchasedCharacter + Library.RenanBatista, 1);
            PlayerPrefs.SetInt(Library.PlayerPrefsComplete + Library.Level01, 1);
            var unlockedCharacter = PlayerPrefs.GetInt(Library.PlayerPrefsPurchasedCharacter + Library.RenanBatista, 0) == 1;

            // StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(WaitKnockBack(knockBackForce)));
            
            if (!unlockedCharacter)
            {
                StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(HudControl.StaticHudControl.DefeatScene(ThisSprite, null)));
                StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(HudControl.StaticHudControl.UnlockCharacterScene(ThisSprite, nextScene)));
            }
            else
            {
                StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(HudControl.StaticHudControl.DefeatScene(ThisSprite, nextScene)));
            }
        }
    }
}
