﻿using UnityEngine;

namespace Items
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private bool flipX;
        
        private Animator _animator;
        private Collider2D _collider2D;
        private Player.Player _player;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.Player>();
            _animator = GetComponent<Animator>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _spriteRenderer.flipX = flipX;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _animator.enabled = true;
                Destroy(_animator, 2);
                Destroy(_collider2D);
            }
        }

        public void SetRandomAward()
        {
            _player.SetRandomAward();
            Destroy(this);
        }
    }
}