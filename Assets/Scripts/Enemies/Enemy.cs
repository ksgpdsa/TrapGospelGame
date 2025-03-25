using UI;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        private GameObject _player;
        private Collider2D _collider2D;
        private EnemyHealth _enemyHealth;
        
        protected Rigidbody2D Rigidbody2D;
        protected LocalAnimator LocalAnimator;
        protected SpriteRenderer SpriteRenderer;
        protected Sprite thisSprite;
        
        private float _attackTime;
        private Vector2 _colliderDefault;
        private Vector2 _colliderFlip;

        [SerializeField] private float attackFrequency;
        [SerializeField] private float visionDistance;
        [SerializeField] private int enemyLives;
        
        [SerializeField] protected float attackVelocity;
        [SerializeField] protected int takeDamage;
        [SerializeField] protected GameObject attack;

        protected abstract int ScoreOnDeath { get; }
        protected abstract int ScoreOnHit { get; }
        protected abstract void InstantiateAttack();

        private void Awake()
        {
            _enemyHealth = new EnemyHealth(enemyLives);
        }

        protected void Start()
        {
            LocalAnimator = new LocalAnimator(GetComponent<Animator>());
            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _collider2D = gameObject.GetComponent<Collider2D>();
            
            thisSprite = SpriteRenderer.sprite;

            _colliderDefault = new Vector2(_collider2D.offset.x, _collider2D.offset.y);
            _colliderFlip = new Vector2(_collider2D.offset.x * -1, _collider2D.offset.y);
            
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        protected void Update()
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
                SpriteRenderer.flipX = _player.transform.position.x < transform.position.x;
                _collider2D.offset = SpriteRenderer.flipX ? _colliderDefault : _colliderFlip;
            }
        }

        private void Attack()
        {
            LocalAnimator.SetBoolAnimator(Library.Attack, true);
        }

        private void EndAttack()
        {
            _attackTime = 1;
            LocalAnimator.SetBoolAnimator(Library.Attack, false);
        }

        public void TakeDamage(int damage)
        {
            var lives = _enemyHealth.TakeDamage(damage);
            
            if (lives <= 0)
            {
                Defeated();
            }
            else
            {
                HudControl.StaticHudControl.AddScore(ScoreOnHit);
            }
        }

        protected virtual void Defeated()
        {
            // StartCoroutine(HudControl.StaticHudControl.DefeatScene(_thisSprite));   
            HudControl.StaticHudControl.AddScore(ScoreOnDeath);
            Destroy(gameObject);
        }
    }
}