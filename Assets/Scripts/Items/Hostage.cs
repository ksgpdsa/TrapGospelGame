using Scene;
using UnityEngine;

namespace Items
{
    public class Hostage : MonoBehaviour
    {
        [SerializeField] private string nextScene;

        private LevelScene _levelScene;

        private void Start()
        {
            _levelScene = FindObjectOfType<LevelScene>();

            _levelScene.SetNextScene(nextScene);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _levelScene.EndScene();
        }
    }
}