using UnityEngine;

public class GameOver : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = col.gameObject.GetComponent<Player.Player>();
            player.GameOver();
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
}