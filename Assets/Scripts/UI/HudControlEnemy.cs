using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class HudControlEnemy : MonoBehaviour
    {
        public static HudControlEnemy StaticHudControlEnemy { get; private set; }

        [SerializeField] private List<GameObject> hudLives;
        
        private void Awake()
        {
            if (StaticHudControlEnemy == null)
            {
                StaticHudControlEnemy = this;
            }
            else
            {
                Destroy(gameObject);
            }
        
            DontDestroyOnLoad(gameObject);
        }

        public void SetLivesInHud(int lives)
        {
            for (var i = 0; i < lives; i++)
            {
                hudLives[i].SetActive(true);
            }

            if (hudLives.Count > lives)
            {
                hudLives[lives].SetActive(false);
            }
        }
    }
}