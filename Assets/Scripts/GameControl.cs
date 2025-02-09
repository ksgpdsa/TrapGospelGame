using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameControl
{
    private const string SavedScore = "SavedScore";
    
    public static void ReopenLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public static void SaveScore(int score, string levelName)
    {
        if (LoadScore(levelName) >= score) return;
        
        PlayerPrefs.SetInt(SavedScore + "_" + levelName, score);
        PlayerPrefs.Save();
    }
    
    public static int LoadScore(string levelName)
    {
        var scoreName = SavedScore + "_" + levelName;       
        return PlayerPrefs.HasKey(scoreName) ? PlayerPrefs.GetInt(scoreName) : 0;
    }
}