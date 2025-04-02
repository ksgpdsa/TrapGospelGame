using System.Collections;
using Player;
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
            var script = newAttack.GetComponent<Attack01>();

            script.Initialize(gameObject, attackVelocity, takeDamage);
        }

        protected override void Defeated()
        {
            PlayerPrefs.SetInt(Library.PlayerPrefsPurchasedCharacter + Library.RenanBatista, 1);
            PlayerPrefs.SetInt(Library.PlayerPrefsComplete + Library.Level01, 1);
            StartCoroutine(PlayBothCoroutines());
        }
        
        private IEnumerator PlayBothCoroutines()
        {
            var unlockedCharacter = PlayerPrefs.GetInt(Library.PlayerPrefsPurchasedCharacter + Library.RenanBatista, 0) == 1;

            if (!unlockedCharacter)
            {
                yield return StartCoroutine(HudControl.StaticHudControl.DefeatScene(ThisSprite, null));
                yield return StartCoroutine(HudControl.StaticHudControl.UnlockCharacterScene(ThisSprite, nextScene));
            }
            else
            {
                yield return StartCoroutine(HudControl.StaticHudControl.DefeatScene(ThisSprite, nextScene));
            }
        }
    }
}
