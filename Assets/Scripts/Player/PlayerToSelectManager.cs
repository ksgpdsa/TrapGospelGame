using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerToSelectManager : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private Text valor;
        [SerializeField] private GameObject buttonPurchase;
        [SerializeField] private GameObject buttonChoiceCharacter;
        
        private bool unlocked;

        private void Awake()
        {
            ManageUnlock();
        }

        private void Update()
        {
            ManageUnlock();
        }

        private void ManageUnlock()
        {
            unlocked = PlayerPrefs.GetInt(Library.PlayerPrefsPurchasedCharacter + characterName, 0) == 1;

            if (unlocked)
            {
                buttonChoiceCharacter.SetActive(true);
                buttonPurchase.SetActive(false);
                valor.enabled = false;
            }
            else
            {
                buttonChoiceCharacter.SetActive(false);
                buttonPurchase.SetActive(true);
                valor.enabled = true;
                
            }
        }
    }
}