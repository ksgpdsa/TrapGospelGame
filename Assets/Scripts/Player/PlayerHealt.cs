using UI;
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
            var newLive = GetPlayerLives() - damage;

            SetPlayerLives(newLive);
            
            HudControl.StaticHudControl.RemoveScore(_scoreOnDamage);

            if (newLive <= 0)
            {
                GameOver();
            }
            
            return newLive;
        }

        public void GameOver()
        {
            SetPlayerLives(0);
            HudControl.StaticHudControl.GameOver();
        }

        private void SetPlayerLives(int newLive)
        {
            _lives = newLive;
            PlayerPrefs.SetInt(Library.PlayerPrefsPlayerLives, _lives);
        }

        public int GetPlayerLives()
        {
            return _lives;
        }
    }
}