using System.Collections.Generic;
using UnityEngine;

public class HudControl : MonoBehaviour
{
    public static HudControl HUDControl { get; private set; }
    
    [SerializeField] private List<GameObject> hudAwards;
    [SerializeField] private List<GameObject> hudLives;

    private void Awake()
    {
        if (HUDControl == null)
        {
            HUDControl = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void ShowAward(string awardName)
    {
        if (awardName != string.Empty)
        {
            hudAwards.Find(x => x.name == awardName).SetActive(true);
        }
    }

    public void SetPlayerLivesInHud(int playerLives)
    {
        for (int i = 0; i < playerLives; i++)
        {
            hudLives[i].SetActive(true);
        }

        if (hudLives.Count > playerLives)
        {
            hudLives[playerLives].SetActive(false);
        }
    }
}