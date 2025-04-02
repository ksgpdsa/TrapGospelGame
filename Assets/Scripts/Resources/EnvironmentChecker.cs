using UnityEngine;

namespace Resources
{
    public class EnvironmentChecker
    {
        private readonly GroundCheck _groundCheck;
        private readonly float _delayJumpTime;

        protected EnvironmentChecker(Transform feetPosition, Vector2 sizeCapsule, LayerMask walkLayers, float angleCapsule, float delayJumpTime)
        {
            _delayJumpTime = delayJumpTime;
            
            if (feetPosition != null)
            {
                _groundCheck = new GroundCheck(feetPosition, sizeCapsule, walkLayers, angleCapsule);
            }
        }

        public bool CheckIsGrounded()
        {
            if (_groundCheck != null)
            {
                return _groundCheck.GetIsGrounded();
            }

            return true;
        }

        public bool CheckIsDelayTouchGround()
        {
            return _groundCheck.GetIsDelayTouchGround(_delayJumpTime);
        }
        
        public void OnDrawGizmosSelected()
        {
            if (_groundCheck != null)
            {
                _groundCheck.OnDrawGizmosSelected();
            }
        }
    }
}