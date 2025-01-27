using UnityEngine;

namespace Enemies
{
    public class Enemy01 : MonoBehaviour
    {
        private LocalAnimator _localAnimator;
        private GameObject _player;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        
        private float _attackTime;
        private Vector2 _colliderDefault;
        private Vector2 _colliderFlip;

        [SerializeField] private float attackFrequency;
        [SerializeField] private float visionDistance;
        [SerializeField] private float arrowVelocity;
        [SerializeField] private GameObject arrow;

        void Start()
        {
            _localAnimator = new LocalAnimator(GetComponent<Animator>());
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();

            _colliderDefault = new Vector2(_collider2D.offset.x, _collider2D.offset.y);
            _colliderFlip = new Vector2(_collider2D.offset.x * -1, _collider2D.offset.y);
            
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            ManageAttacks();
            FlipToPlayer();
        }

        private void ManageAttacks()
        {
            if (IsInVision() && _attackTime <= 0)
            {
                Attack();
            }
            else if (_attackTime > 0)
            {
                _attackTime -= attackFrequency;
            }
        }

        private bool IsInVision()
        {
            bool result;
            var playerPositionX = _player.transform.position.x;
            var thisPositionX = transform.position.x;

            if (playerPositionX > thisPositionX)
            {
                result = playerPositionX - thisPositionX < visionDistance;
            }
            else
            {
                result = thisPositionX - playerPositionX < visionDistance;
            }
            
            return result;
        }

        private void FlipToPlayer()
        {
            if (IsInVision())
            {
                _spriteRenderer.flipX = _player.transform.position.x < transform.position.x;
                _collider2D.offset = _spriteRenderer.flipX ? _colliderDefault : _colliderFlip;
            }
        }

        void InstantiateAttack()
        {
            var newArrow = Instantiate(arrow, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
            var script = newArrow.GetComponent<Arrow>();
            
            script.Initialize(gameObject, arrowVelocity);
        }

        void Attack()
        {
            _localAnimator.SetBoolAnimator(Library.Attack, true);
        }

        void EndAttack()
        {
            _attackTime = 1;
            _localAnimator.SetBoolAnimator(Library.Attack, false);
        }
    }
}
