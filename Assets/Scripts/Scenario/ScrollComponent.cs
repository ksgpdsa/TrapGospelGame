using UnityEngine;

namespace Scenario
{
    public class ScrollComponent : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private int numberTiles;
        private float _lenght;

        private float _screenStart;

        private void Start()
        {
            _lenght = GetComponent<Renderer>().bounds.size.x;
            var cameraPlayer = FindFirstObjectByType<UnityEngine.Camera>();
            _screenStart = cameraPlayer.transform.position.x - _lenght;
        }

        private void Update()
        {
            transform.position = new Vector3(transform.position.x - scrollSpeed, transform.position.y);

            if (transform.position.x < _screenStart)
            {
                transform.position = new Vector3(_screenStart + _lenght * numberTiles, transform.position.y);
            }
        }
    }
}