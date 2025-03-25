using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public abstract class Scene : MonoBehaviour
    {
        public abstract void EndScene();
        
        protected static void NextScene(string sceneName)
        {
            // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName);
        }

        protected static void PreviousScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName);
        }
    }
}