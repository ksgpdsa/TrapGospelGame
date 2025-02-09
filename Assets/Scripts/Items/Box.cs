﻿using UnityEngine;

namespace Items
{
    public class Box : MonoBehaviour
    {
        private Animator _animator;
        private Collider2D _collider2D;
        private Player.Player _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player.Player>();
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _animator.enabled = true;
                Destroy(_animator, 1);
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