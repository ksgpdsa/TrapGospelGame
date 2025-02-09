using UnityEngine;

namespace Enemies
{
    public class EnemyHealth
    {
        private int _lives;

        public EnemyHealth(int lives)
        {
            _lives = lives;
        }

        public int TakeDamage(int damage)
        {
            _lives -= damage;

            if (_lives <= 0)
            {
                Defeated();
            }
            
            return _lives;
        }

        private void Defeated()
        {
            Debug.Log("Defeated !");
            _lives = 0;
        }
    }
}