namespace Player
{
    using UnityEngine;

    public class PlayerGroundCheck
    {
        private readonly Transform _feetPosition;
        private readonly Vector2 _sizeCapsule;
        private readonly LayerMask _groundLayer;
        private readonly float _angleCapsule;

        public PlayerGroundCheck(Transform feetPosition, Vector2 sizeCapsule, LayerMask groundLayer, float angleCapsule)
        {
            _feetPosition = feetPosition;
            _sizeCapsule = sizeCapsule;
            _groundLayer = groundLayer;
            _angleCapsule = angleCapsule;
        }
        
        private bool _isGrounded;

        public bool GetIsGrounded()
        {
            return _isGrounded;
        }

        public void Update()
        {
            _isGrounded = Physics2D.OverlapCapsule(_feetPosition.position, _sizeCapsule, CapsuleDirection2D.Horizontal, _angleCapsule, _groundLayer);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawCube(_feetPosition.position, _sizeCapsule);
        }
    }
}