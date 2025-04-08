using UnityEngine;

namespace Respawn
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField] private int respawnNumber;
        [SerializeField] private Sprite activeRespawnPoint;

        public int GetRespawnNumber()
        {
            return respawnNumber;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = activeRespawnPoint;
            }
        }
    }
}