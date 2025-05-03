using Player.Attacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Enemies
{
    public abstract class CharacterEnemy : Enemy
    {
        [Header("Comportamento")] [SerializeField]
        private LayerMask playerLayer;

        [SerializeField] protected string nextScene;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float decisionInterval = 1f;

        private float _decisionTimer;

        private IResolvedStyle _rageImage;

        private new void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            _decisionTimer = decisionInterval;
            base.Start();
        }

        private new void Update()
        {
            if (_rageImage != null)
            {
                var color = IsInVision
                    ? new Color(_rageImage.color.r, _rageImage.color.g, _rageImage.color.b, 0.5f)
                    : new Color(_rageImage.color.r, _rageImage.color.g, _rageImage.color.b, 0f);
                UpdateRageImage(color);
            }
        }

        private new void FixedUpdate()
        {
            base.FixedUpdate();
            MakeDecision();
            AnimationManager.ManageJumpAnimations(IsGrounded, Rigidbody2D.velocity.y);
        }

        protected override void InstantiateAttack()
        {
            var attackPosition = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.1f,
                transform.position.z);

            var newAttack = Instantiate(attack, attackPosition, Quaternion.identity);
            var script = newAttack.GetComponent<SingAttack>();

            script.Initialize(gameObject, attackVelocity, takeDamage);
        } // ReSharper disable Unity.PerformanceAnalysis
        private void MakeDecision()
        {
            var attackNow = false;

            _decisionTimer -= Time.fixedDeltaTime;

            if (_decisionTimer <= 0)
            {
                _decisionTimer = decisionInterval;

                var randomChoice = Random.Range(0, decisionInterval);

                if (randomChoice < 0.1f || randomChoice > 0.9f) EnemyMovement.Jump();

                if (randomChoice < 0.5f) attackNow = true;
            }

            AttackHandler.ManageAttacks(IsInVision, InstantiateAttack, InstantiateAttackByAnimation, attackNow);
        }

        private void UpdateRageImage(Color color)
        {
            // rageImage.color = color;
        }
    }
}