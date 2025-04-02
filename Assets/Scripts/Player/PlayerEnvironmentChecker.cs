using Resources;
using UnityEngine;

namespace Player
{
    public class PlayerEnvironmentChecker : EnvironmentChecker
    {
        private readonly GroundCheck _groundCheck;

        public PlayerEnvironmentChecker(Transform feetPosition, Vector2 sizeCapsule, LayerMask walkLayers, float angleCapsule, float delayJumpTime) : base(feetPosition, sizeCapsule, walkLayers, angleCapsule, delayJumpTime)
        {
        }
    }
}