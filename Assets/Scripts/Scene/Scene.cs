using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public abstract class Scene : MonoBehaviour
    {
        public abstract void EndScene();
        
        protected void NextScene(string sceneName)
        {
            StopAllCoroutines();
            // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            GameControl.ResetRespawnPoints();
            SceneManager.LoadScene(sceneName);
        }

        protected void PreviousScene(string sceneName)
        {
            StopAllCoroutines();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName);
        }
    }
}