using UnityEngine;

namespace Player
{
    public class PlayerHealth
    {
        private readonly int _scoreOnDamage;
        private int _lives;

        public PlayerHealth(int lives, int scoreOnDamage)
        {
            _lives = lives;
            _scoreOnDamage = scoreOnDamage;
        }

        public int TakeDamage(int damage)
        {
            var newLive = _lives - damage;

            SetPlayerLives(newLive);
            
            GameControl.StaticGameControl.RemoveScore(_scoreOnDamage);
            
            return newLive;
        }

        public void GameOver()
        {
            SetPlayerLives(0);
            GameControl.StaticGameControl.GameOver();
        }

        private void SetPlayerLives(int newLive)
        {
            _lives = newLive;
            PlayerPrefs.SetInt(Library.PlayerPrefsPlayerLives, _lives);
        }
    }
}