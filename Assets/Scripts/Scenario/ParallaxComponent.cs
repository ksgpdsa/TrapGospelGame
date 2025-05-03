using UnityEngine;

namespace Scenario
{
    public class ParallaxComponent : MonoBehaviour
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        [Range(0f, 1f)]
        [SerializeField] private float scrollSpeed;
        [SerializeField] private int numberOfParallax;
        [SerializeField] private float parallaxStrength = 2f;
        
        private float _distance;
        private UnityEngine.Camera _camera;
        private Material _material;
        private Vector3 _startPosition;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _camera = FindObjectOfType<UnityEngine.Camera>();
            _startPosition = _camera.transform.position;
        }

        private void Update()
        {
            if (!_camera) return;
            
            // Calcula o deslocamento da câmera em relação à posição inicial
            var cameraDeltaX = _camera.transform.position.x - _startPosition.x;

            // Camadas com número de parallax maior se movem menos
            var parallaxFactor = Mathf.Pow(1f / Mathf.Max(1f, numberOfParallax), parallaxStrength);

            // Aplica o fator e a velocidade
            var offsetX = cameraDeltaX * scrollSpeed * parallaxFactor;

            // Aplica o offset no material
            _material.SetTextureOffset(MainTex, new Vector2(offsetX, 0));
        }
    }
}