using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scenario
{
    public class ScrollComponent : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private int numberTiles;
        
        private float _screenStart;
        private float _lenght;

        private void Start()
        {
            _lenght = GetComponent<TilemapRenderer>().bounds.size.x;
            var cameraPlayer = FindFirstObjectByType<UnityEngine.Camera>();
            _screenStart = cameraPlayer.transform.position.x - _lenght;
        }
    
        // Update is called once per frame
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
