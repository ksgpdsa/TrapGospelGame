using System;
using Resources;
using UnityEngine;

namespace Enemies
{
    public class EnemyAttackHandler
    {
        private readonly AnimationManager _animationManager;
        private readonly float _attackFrequency;
        private float _attackTimer = 1;

        public EnemyAttackHandler(float attackFrequency, AnimationManager animationManager)
        {
            _attackFrequency = attackFrequency;
            _animationManager = animationManager;
        } // ReSharper disable Unity.PerformanceAnalysis
        public void ManageAttacks(bool isInVision, Action attackCallback, bool instantiateAttackByAnimation,
            bool attackNow = true)
        {
            if (attackNow && isInVision && _attackTimer <= 0)
            {
                if (!instantiateAttackByAnimation) attackCallback?.Invoke();

                Attack();

                if (!instantiateAttackByAnimation) EndAttack();
            }
            else if (_attackTimer > 0)
            {
                _attackTimer -= _attackFrequency * Time.fixedDeltaTime;
            }
        }

        private void Attack()
        {
            _attackTimer = 0;
            _animationManager.Attack();
        }

        public void EndAttack()
        {
            _attackTimer = 1;
            _animationManager.EndAttack();
        }
    }
}