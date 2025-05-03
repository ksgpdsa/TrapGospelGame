using UnityEngine;

namespace Character
{
    public class ChoiceCharacter : MonoBehaviour
    {
        public void Choice()
        {
            var characterManager = FindObjectOfType<CharacterManager>();

            characterManager.ChoiceCharacter();
        }
    }
}