using Character;
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
        
        private string _nextSceneName;
        private GameObject _player;

        private void Awake()
        {
            SpawnPlayer();
            StartCoroutine(HudControl.StaticHudControl.NewLevelScene(sceneLevelName, levelIcon));
        }
        
        private void SpawnPlayer()
        {
            var selectedCharacter = PlayerPrefs.GetInt(Library.PlayerPrefsSelectedCharacter, 0); // Pega o personagem salvo
            
            var respawnPoint = FindRespawnPoint();

            if (respawnPoint != null && characterDatabase.CharacterCount > selectedCharacter &&
                (levelName == "Level01" || PlayerPrefs.GetInt(Library.PlayerPrefsComplete + levelName, 0) != 0) // libera só se tiver o Level liberado ou se for o Level01
            ) {
                Instantiate(characterDatabase.GetCharacter(selectedCharacter).gameObject, respawnPoint.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Nenhum ponto de respawn encontrado ou personagem inválido!");
            }
        }
        
        private static GameObject FindRespawnPoint()
        {
            var respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            return respawnPoints.Length > 0 ? respawnPoints[0] : null; // Retorna o primeiro encontrado
        }

        public override void EndScene()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _player.GetComponent<Player.Player>();
            
            NextScene(_nextSceneName);
        }
        
        public void SetNextScene(string sceneName)
        {
            _nextSceneName = sceneName;
        }
    }
}