using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed;
        
        private Transform _target;
        
        // Start is called before the first frame update
        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            var startPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            var smoothedPosition = Vector3.Lerp(transform.position, startPosition, Time.fixedDeltaTime * smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
