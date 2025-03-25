using UnityEngine;
using UnityEngine.Purchasing;

public class MonetizationManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetInt(Library.PlayerPrefsPurchasedCharacter + Library.Ksbatista, 1);
    }
    
    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case "characterrenanbatista":
                PlayerPrefs.SetInt(Library.PlayerPrefsPurchasedCharacter + Library.RenanBatista, 1);
                break;
            
            case "charactert3rt":
                PlayerPrefs.SetInt(Library.PlayerPrefsPurchasedCharacter + Library.T3rt, 1);
                break;
        }
        
        PlayerPrefs.Save();
    }
}