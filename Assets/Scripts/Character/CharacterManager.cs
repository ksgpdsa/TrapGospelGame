using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private CharacterDatabase characterDatabase;
        
        [SerializeField] private Text nameText;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private UnityEngine.Camera camera;
        
        private int selectedCharacter;
        
        private void Start()
        {
            UpdateCharacter();
        }

        public void NextCharacter()
        {
            selectedCharacter++;

            if (selectedCharacter >= characterDatabase.CharacterCount)
            {
                selectedCharacter = 0;
            }
            
            UpdateCharacter();
        }

        public void PreviousCharacter()
        {
            selectedCharacter--;

            if (selectedCharacter < 0)
            {
                selectedCharacter = characterDatabase.CharacterCount - 1;
            }
            
            UpdateCharacter();
        }

        private void UpdateCharacter()
        {
            var characterPosition = characterPrefab.transform.position;
            
            // Destroi o personagem anterior (se houver)
            if (characterPrefab != null)
            {
                Destroy(characterPrefab);
            }

            // Obtém o novo personagem do banco de dados
            var character = characterDatabase.GetCharacter(selectedCharacter);
    
            // Instancia o novo personagem na cena
            characterPrefab = Instantiate(character.gameObject, characterPosition, Quaternion.identity);

            // Atualiza o nome do personagem na UI
            nameText.text = character.name;
        }

        public void ChoiceCharacter()
        {
            PlayerPrefs.SetInt(Library.PlayerPrefsSelectedCharacter, selectedCharacter);

            var scene = camera.GetComponent<Scene.Scene>();
            
            scene.EndScene();
        }
    }
}