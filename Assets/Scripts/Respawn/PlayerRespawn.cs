using UnityEngine;

namespace Respawn
{
    public class PlayerRespawn : MonoBehaviour
    {
        private Vector3? _respawnPoint;

        private void Start()
        {
            var respawnObject = FindRespawnPoint();
        
            if (respawnObject)
            {
                _respawnPoint = respawnObject.transform.position;
            }
        }
        
        private GameObject FindRespawnPoint()
        {
            // Procura todos os objetos na layer "Respawn"
            var respawnObjects = GameObject.FindGameObjectsWithTag("Respawn");

            if (respawnObjects.Length > 0)
            {
                return respawnObjects[0]; // Retorna o primeiro que encontrar
            }

            return null;
        }

        public void Respawn()
        {
            if (_respawnPoint != null)
            {
                transform.position = _respawnPoint.Value; // Teletransporta o jogador para o ponto salvo
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Respawn"))
            {
                _respawnPoint = other.transform.position; // Atualiza o ponto de respawn
            }
        }
    }
}