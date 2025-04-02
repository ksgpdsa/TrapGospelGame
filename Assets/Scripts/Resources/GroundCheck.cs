using UnityEngine;

namespace Resources
{
    public class GroundCheck
    {
        private readonly Transform _feetPosition;
        private readonly Vector2 _sizeCapsule;
        private readonly LayerMask _groundLayer;
        private readonly float _angleCapsule;
        
        private bool _isGrounded;
        private float _delayJump;

        public GroundCheck(Transform feetPosition, Vector2 sizeCapsule, LayerMask groundLayer, float angleCapsule)
        {
            _feetPosition = feetPosition;
            _sizeCapsule = sizeCapsule;
            _groundLayer = groundLayer;
            _angleCapsule = angleCapsule;
        }

        public bool GetIsGrounded()
        {
            Update();
            return _isGrounded;
        }

        public bool GetIsDelayTouchGround(float delayJumpTime)
        {
            if (_isGrounded)
            {
                _delayJump = delayJumpTime;
                return true;
            }

            _delayJump -= Time.deltaTime;
            return _delayJump > 0;
        }

        private void Update()
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