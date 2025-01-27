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

        public void TakeDamage(int damage)
        {
            _lives -= damage;
            Debug.Log($"Vidas restantes: {_lives}");

            if (_lives <= 0)
            {
                GameOver();
            }
        }

        public void GameOver()
        {
            Debug.Log("Game Over!");
            // todo: Lógica para finalizar o jogo ou reiniciar
        }
    }
}