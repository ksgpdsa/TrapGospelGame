using TMPro;
using UnityEngine;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private CharacterDatabase characterDatabase;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private UnityEngine.Camera cameraScene;

        private int _selectedCharacter;

        private void Start()
        {
            UpdateCharacter();
        }

        public void NextCharacter()
        {
            _selectedCharacter++;

            if (_selectedCharacter >= characterDatabase.CharacterCount) _selectedCharacter = 0;

            UpdateCharacter();
        }

        public void PreviousCharacter()
        {
            _selectedCharacter--;

            if (_selectedCharacter < 0) _selectedCharacter = characterDatabase.CharacterCount - 1;

            UpdateCharacter();
        }

        private void UpdateCharacter()
        {
            var characterPosition = characterPrefab.transform.position;

            // Destroi o personagem anterior (se houver)
            if (characterPrefab != null) Destroy(characterPrefab);

            // Obtém o novo personagem do banco de dados
            var character = characterDatabase.GetCharacter(_selectedCharacter);

            // Instancia o novo personagem na cena
            characterPrefab = Instantiate(character.gameObject, characterPosition, Quaternion.identity);

            // Atualiza o nome do personagem na UI
            nameText.text = character.name;
        }

        public void ChoiceCharacter()
        {
            PlayerPrefs.SetInt(Library.PlayerPrefsSelectedCharacter, _selectedCharacter);

            var scene = cameraScene.GetComponent<Scene.Scene>();

            scene.EndScene();
        }
    }
}