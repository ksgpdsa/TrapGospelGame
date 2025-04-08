using Character;
using Respawn;
using UI;
using UnityEngine;

namespace Scene
{
    public class LevelScene : Scene
    {
        [SerializeField] private CharacterDatabase characterDatabase;
        [SerializeField] private string levelName;
        [SerializeField] private string sceneLevelName;
        [SerializeField] private Sprite levelIcon;
        [SerializeField] private bool isFinalLevel;
        [SerializeField] private float timeLevel = 300f;
        
        private string _nextSceneName;

        private void Awake()
        {
            GameControl.StaticGameControl.SetInLevel(true);
            GameControl.StaticGameControl.SetLevelName(sceneLevelName); 
            GameControl.StaticGameControl.SetLevelTime(timeLevel);
            SpawnPlayer();
            StartCoroutine(CoroutineManager.StaticCoroutineManager.RunCoroutine(HudControl.StaticHudControl.NewLevelScene(sceneLevelName, levelIcon)));
        }
        
        private void SpawnPlayer()
        {
            var selectedCharacter = PlayerPrefs.GetInt(Library.PlayerPrefsSelectedCharacter, 0); // Pega o personagem salvo
            var player = characterDatabase.GetCharacter(selectedCharacter).gameObject;
            var playerRespawn = player.GetComponent<PlayerRespawn>();
            
            var respawnPoint = playerRespawn.FindRespawnPoint();

            if (respawnPoint != null && characterDatabase.CharacterCount > selectedCharacter &&
                (levelName == "Level01" || PlayerPrefs.GetInt(Library.PlayerPrefsComplete + levelName, 0) != 0) // libera só se tiver o Level liberado ou se for o Level01
            ) {
                Instantiate(player, respawnPoint.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Nenhum ponto de respawn encontrado ou personagem inválido!");
            }
        }

        public override void EndScene()
        {
            GameControl.StaticGameControl.SetInLevel(false);
            GameControl.CompleteLevel(0, sceneLevelName, isFinalLevel);
            NextScene(_nextSceneName);
        }
        
        public void SetNextScene(string sceneName)
        {
            _nextSceneName = sceneName;
        }
    }
}