using UnityEngine;

namespace Scene
{
    public class LevelSelectScene : Scene
    {
        private const string SceneName = Library.LevelSelect;
        private const string PreviousSceneName = Library.CharacterSelect;
        private string _nextSceneName;

        private void Awake()
        {
            PlayerPrefs.SetInt(Library.PlayerPrefsPlayerLives, 5);
        }

        public override void EndScene()
        {
            NextScene(_nextSceneName);
        }

        public void SetNextScene(string sceneName)
        {
            _nextSceneName = sceneName;

            EndScene();
        }

        public void PreviousScene()
        {
            PreviousScene(PreviousSceneName);
        }
    }
}