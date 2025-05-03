using Extensions;
using UnityEngine;

namespace Respawn
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField] private int respawnNumber;
        [SerializeField] private Sprite activeRespawnPoint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var childrenSpriteRenderer = gameObject.GetComponentOnlyInChildren<SpriteRenderer>();
                childrenSpriteRenderer.enabled = true;
            }
        }

        public int GetRespawnNumber()
        {
            return respawnNumber;
        }
    }
}