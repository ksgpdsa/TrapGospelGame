namespace Player
{
    using UnityEngine;

    public class PlayerHealth
    {
        private int _lives;

        public PlayerHealth(int lives)
        {
            _lives = lives;
        }

        public int TakeDamage(int damage)
        {
            _lives -= damage;
            Debug.Log($"Vidas restantes: {_lives}");

            if (_lives <= 0)
            {
                GameOver();
            }
            
            return _lives;
        }

        public void GameOver()
        {
            Debug.Log("Game Over!");
            _lives = 0;
        }
    }
}