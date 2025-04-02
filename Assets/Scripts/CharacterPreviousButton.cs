using Scene;
using UnityEngine;

public class CharacterPreviousButton : MonoBehaviour
{
    public void Voltar()
    {
        var scene = FindObjectOfType<CharacterSelectScene>();
        scene.PreviousScene();
    }
}