using UnityEngine;

namespace Resources
{
    public class FloatingScore : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Vector3 offset;

        private UnityEngine.Camera _mainCamera;

        private void Start()
        {
            _mainCamera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            if (player)
            {
                // Converte a posição do Player (mundo) para posição na UI (Canvas)
                var screenPosition = _mainCamera.WorldToScreenPoint(player.position + offset);
                transform.position = screenPosition;
            }
        }
    }
}