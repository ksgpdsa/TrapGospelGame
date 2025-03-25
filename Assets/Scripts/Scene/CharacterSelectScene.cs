namespace Scene
{
    public class CharacterSelectScene : Scene
    {
        private const string SceneName = Library.CharacterSelect;
        private const string NextSceneName = Library.LevelSelect;
        private const string PreviousSceneName = Library.Menu;

        public override void EndScene()
        {
            NextScene(NextSceneName);
        }

        public void PreviousScene()
        {
            PreviousScene(PreviousSceneName);
        }
    }
}