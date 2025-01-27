using UnityEngine;

namespace Items
{
    public class Box : MonoBehaviour
    {
        private Animator _animator;
        private Collider2D _collider2D;
    
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _collider2D = GetComponent<Collider2D>();
        }
    
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _animator.enabled = true;
                Destroy(_animator, 1);
                Destroy(_collider2D);
                Destroy(this);
            }
        }
    }
}
