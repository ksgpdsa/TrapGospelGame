using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources
{
    public abstract class Platform : MonoBehaviour
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private float platformSpeed;
        [SerializeField] private float pausePlatformWaitTime;

        protected bool HorizontalDirection;

        private bool _goingUp = true;
        private float _interpolation;
        private bool _isWaiting;

        private float _randomVariation;

        private void Awake()
        {
            _randomVariation = Random.Range(2f, 4f);
            var thisPosition = HorizontalDirection ? transform.position.x : transform.position.y;
            min = thisPosition - min;
            max = thisPosition + max;
        }

        private void Update()
        {
            if (!_isWaiting)
            {
                // Atualiza o tempo de interpolação baseado na direção
                _interpolation += (_goingUp ? 1 : -1) * platformSpeed * _randomVariation * Time.deltaTime;

                // Aplica SmoothStep para suavizar a desaceleração nos extremos
                var smoothT = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(_interpolation, 1f));

                // Calcula a nova posição entre min e max
                var newDirection = Mathf.Lerp(min, max, smoothT);

                transform.position = HorizontalDirection
                    ? new Vector3(newDirection, transform.position.y, transform.position.z)
                    : new Vector3(transform.position.x, newDirection, transform.position.z);

                // Verifica se chegou ao limite e inicia a pausa
                if ((_goingUp && _interpolation >= 1f) || (!_goingUp && _interpolation <= 0f)) StartCoroutine(WaitAtPoint());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                collision.transform.SetParent(transform); // Faz o player ser filho da plataforma
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
                collision.transform.SetParent(null); // Remove o player da plataforma ao sair
        }

        private IEnumerator WaitAtPoint()
        {
            _isWaiting = true;
            
            _interpolation = _goingUp ? 1f : 0f;
            
            yield return new WaitForSeconds(pausePlatformWaitTime);
            _goingUp = !_goingUp; // Inverte a direção
            _isWaiting = false;
        }
    }
}