using System.Collections;
using UnityEngine;

namespace Resources
{
    public abstract class Platform : MonoBehaviour
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private float platformSpeed;
        public float pausePlatformWaitTime;
        protected bool HorizontalDirection;

        private float interpolation;
        private bool goingUp = true;
        private bool isWaiting;

        private void Update()
        {
            if (!isWaiting)
            {
                // Atualiza o tempo de interpolação baseado na direção
                interpolation += (goingUp ? 1 : -1) * platformSpeed * Time.deltaTime;

                // Aplica SmoothStep para suavizar a desaceleração nos extremos
                var smoothT = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(interpolation, 1f));

                // Calcula a nova posição entre min e max
                var newDirection = Mathf.Lerp(min, max, smoothT);
                
                transform.position = HorizontalDirection ? 
                    new Vector3(newDirection, transform.position.y, transform.position.z) :
                    new Vector3(transform.position.x, newDirection, transform.position.z);

                // Verifica se chegou ao limite e inicia a pausa
                if (interpolation >= 1f || interpolation <= 0f)
                {
                    StartCoroutine(WaitAtPoint());
                }
            }
        }

        private IEnumerator WaitAtPoint()
        {
            isWaiting = true;
            yield return new WaitForSeconds(pausePlatformWaitTime);
            goingUp = !goingUp; // Inverte a direção
            isWaiting = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.transform.SetParent(transform); // Faz o player ser filho da plataforma
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.transform.SetParent(null); // Remove o player da plataforma ao sair
            }
        }
    }
}