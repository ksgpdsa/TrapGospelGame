using System.Collections.Generic;
using Player.Attacks;
using UnityEngine;

namespace Player
{
    public class T3rt : Player
    {
        [SerializeField] private float attackVelocity;
        [SerializeField] private GameObject attack;

        private readonly Dictionary<int, string> _awards = new()
        {
            { 1, Library.Disc },
            { 2, Library.Mouth }
        };

        private void Awake()
        {
            Awards = new Dictionary<int, string>(_awards);

            base.Awake();
        } // ReSharper disable Unity.PerformanceAnalysis
        public override void AttackFromPlayer()
        {
            var attackObject = Instantiate(attack,
                new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z),
                Quaternion.identity);
            var attackClass = attackObject.gameObject.GetComponent<Attack>();
            attackClass.Initialize(gameObject, attackVelocity, 1);
        }

        private void Start()
        {
            base.Start();
        }
    }
}