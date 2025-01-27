namespace Player
{
    using UnityEngine;

    public class PlayerGroundCheck : MonoBehaviour
    {
        [SerializeField] private Transform feetPosition;
        [SerializeField] private Vector2 sizeCapsule;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float angleCapsule;

        private bool _isGrounded;

        public bool GetIsGrounded()
        {
            return _isGrounded;
        }

        private void Update()
        {
            _isGrounded = Physics2D.OverlapCapsule(feetPosition.position, sizeCapsule, CapsuleDirection2D.Horizontal, angleCapsule, groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawCube(feetPosition.position, sizeCapsule);
        }
    }
}