using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var playerHealth = col.gameObject.GetComponent<Player.Player>();
            playerHealth.GameOver();
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
}