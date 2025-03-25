using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] string levelNameToUnlock;

        private void Awake()
        {
            if (PlayerPrefs.GetInt(Library.PlayerPrefsComplete + levelNameToUnlock, 0) != 0)
            {
                var button = gameObject.GetComponent<Button>();
                
                button.enabled = true;
                
                var image = button.GetComponent<Image>();
                
                image.color = new Color(191, 36, 42, 255);
            }
        }
    }
}