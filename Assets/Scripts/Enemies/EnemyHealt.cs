using UI;

namespace Enemies
{
    public class EnemyHealth
    {
        private int _lives;

        public EnemyHealth(int lives)
        {
            _lives = lives;
        }

        public void StartLivesInHud()
        {
            if (HudControlEnemy.StaticHudControlEnemy != null)
            {
                HudControlEnemy.StaticHudControlEnemy.SetLivesInHud(_lives);
            }
        }

        public int TakeDamage(int damage)
        {
            _lives -= damage;

            if (HudControlEnemy.StaticHudControlEnemy != null)
            {
                HudControlEnemy.StaticHudControlEnemy.SetLivesInHud(_lives);
            }

            if (_lives <= 0)
            {
                Defeated();
            }
            
            return _lives;
        }

        private void Defeated()
        {
            _lives = 0;
        }
    }
}