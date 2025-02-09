using System.Collections;
using UI;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        private LocalAnimator _localAnimator;
        private GameObject _player;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private EnemyHealth _enemyHealth;
        
        private float _attackTime;
        private Vector2 _colliderDefault;
        private Vector2 _colliderFlip;

        [SerializeField] private float attackFrequency;
        [SerializeField] private float visionDistance;
        [SerializeField] private int enemyLives;
        
        [SerializeField] protected float attackVelocity;
        [SerializeField] protected int takeDamage;
        [SerializeField] protected GameObject arrow;
        private Sprite _thisSprite;

        protected abstract int ScoreOnDeath { get; }
        protected abstract void InstantiateAttack();

        private void Awake()
        {
            _enemyHealth = new EnemyHealth(enemyLives);
        }

        private void Start()
        {
            _localAnimator = new LocalAnimator(GetComponent<Animator>());
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            
            _thisSprite = _spriteRenderer.sprite;

            _colliderDefault = new Vector2(_collider2D.offset.x, _collider2D.offset.y);
            _colliderFlip = new Vector2(_collider2D.offset.x * -1, _collider2D.offset.y);
            
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
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

        private void Attack()
        {
            _localAnimator.SetBoolAnimator(Library.Attack, true);
        }

        private void EndAttack()
        {
            _attackTime = 1;
            _localAnimator.SetBoolAnimator(Library.Attack, false);
        }

        public void TakeDamage(int damage)
        {
            var lives = _enemyHealth.TakeDamage(damage);
            
            if (lives <= 0)
            {
                Defeated();
            }
        }

        private void Defeated()
        {
            // StartCoroutine(HudControl.StaticHudControl.DefeatScene(_thisSprite));   
            HudControl.StaticHudControl.AddScore(ScoreOnDeath);
            Destroy(gameObject);
        }
    }
}