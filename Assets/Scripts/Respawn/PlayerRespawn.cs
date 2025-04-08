using System.Linq;
using UnityEngine;

namespace Respawn
{
    public class PlayerRespawn : MonoBehaviour
    {
        private Vector3? _respawnPoint;

        public GameObject FindRespawnPoint()
        {
            var respawnNumber = GameControl.LoadRespawnPoint();
            
            // Procura todos os objetos na layer "Respawn"
            var respawnObjects = GameObject.FindGameObjectsWithTag("Respawn");

            if (respawnObjects.Length > 0)
            {
                var respawnPointGameObject = (
                    from respawnObject in respawnObjects 
                        let respawnScript = respawnObject.GetComponent<RespawnPoint>()
                        where respawnScript.GetRespawnNumber() == respawnNumber
                    select respawnObject
                ).FirstOrDefault();
                
                _respawnPoint = respawnPointGameObject?.transform.position;
                
                return respawnPointGameObject;
            }

            return null;
        }

        public void Respawn()
        {
            if (_respawnPoint != null)
            {
                var respawnPosition = _respawnPoint.Value;
                respawnPosition.y += 0.7f;
                transform.position = respawnPosition; // Teletransporta o jogador para o ponto salvo
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Respawn"))
            {
                var respawnScript = other.gameObject.GetComponent<RespawnPoint>();
                GameControl.SaveRespawnPoint(respawnScript.GetRespawnNumber());
            }
        }
    }
}