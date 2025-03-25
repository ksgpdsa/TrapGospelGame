using System;
using Scene;
using UnityEngine;

namespace Items
{
    public class Hostage : MonoBehaviour
    {
        [SerializeField] private string nextScene;
        
        private GameObject mainCamera;
        private LevelScene levelScene;
        
        private void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            levelScene = mainCamera.GetComponent<LevelScene>();
                
            levelScene.SetNextScene(nextScene);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                levelScene.EndScene();
            }
        }
    }
}