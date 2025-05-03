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

        private bool _unlocked;

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
            _unlocked = PlayerPrefs.GetInt(Library.PlayerPrefsPurchasedCharacter + characterName, 0) == 1;

            if (_unlocked)
            {
                buttonChoiceCharacter.SetActive(true);
                buttonPurchase.SetActive(false);
                valor.enabled = false;
            }
            else
            {
                buttonChoiceCharacter.SetActive(false);
                buttonPurchase.SetActive(true);
                // valor.enabled = true;
            }
        }
    }
}