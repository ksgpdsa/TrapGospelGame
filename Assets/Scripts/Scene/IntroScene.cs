namespace Scene
{
    public class IntroScene : Scene
    {
        private const string SceneName = Library.IntroScene;
        private const string NextSceneName = Library.Menu;

        public override void EndScene()
        {
            NextScene(NextSceneName);
        }
    }
}