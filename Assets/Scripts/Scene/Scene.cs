using UnityEngine.SceneManagement;

namespace Scene
{
    public abstract class Scene
    {
        protected abstract void EndScene();
        
        protected void NextScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    
        
    }
}