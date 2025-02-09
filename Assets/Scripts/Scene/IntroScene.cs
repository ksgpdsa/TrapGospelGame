namespace Scene
{
    public class IntroScene : Scene
    {
        private const string SceneName = Library.IntroScene;
        private const string NextSceneName = Library.Level01;

        protected override void EndScene()
        {
            NextScene(NextSceneName);
        }
    }
}