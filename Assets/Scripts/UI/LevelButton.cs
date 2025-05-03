using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private string levelNameToUnlock;

        private void Awake()
        {
            if (PlayerPrefs.GetInt(Library.PlayerPrefsComplete + levelNameToUnlock, 0) != 0)
            {
                var button = gameObject.GetComponent<Button>();

                button.enabled = true;
            }
        }
    }
}