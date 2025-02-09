using UI;

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
            _lives -= damage;
            
            HudControl.StaticHudControl.RemoveScore(_scoreOnDamage);

            if (_lives <= 0)
            {
                GameOver();
            }
            
            return _lives;
        }

        public void GameOver()
        {
            _lives = 0;
            HudControl.StaticHudControl.GameOver();
        }
    }
}