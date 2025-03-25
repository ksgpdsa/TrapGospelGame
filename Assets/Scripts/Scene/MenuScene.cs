namespace Scene
{
    public class MenuScene : Scene
    {
        private const string SceneName = Library.Menu;
        private const string NextSceneName = Library.CharacterSelect;

        public override void EndScene()
        {
            NextScene(NextSceneName);
        }
    }
}