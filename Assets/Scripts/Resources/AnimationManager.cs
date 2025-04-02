using UnityEngine;

namespace Resources
{
    public class AnimationManager
    {
        private readonly Animator _animator;

        public AnimationManager(Animator animator)
        {
            _animator = animator;
        }
    
        public void ManageJumpAnimations(bool isGrounded, float velocityY)
        {
            SetBoolAnimator(Library.IsJumping, !isGrounded && velocityY > 0);
            SetBoolAnimator(Library.IsFalling, !isGrounded && velocityY < 0);
        }

        public void Move()
        {
            SetBoolAnimator(Library.IsMoving, true);
        }

        public void StopMove()
        {
            SetBoolAnimator(Library.IsMoving, false);
        }

        public void Attack()
        {
            SetBoolAnimator(Library.Attack, true);
        }

        public void EndAttack()
        {
            SetBoolAnimator(Library.Attack, false);
        }

        public bool GetBoolAnimator(string name)
        {
            return _animator.GetBool(name);
        }

        private void SetBoolAnimator(string name, bool value)
        {
            _animator.SetBool(name, value);
        }
    }
}