using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameControl
{
    public static void ReopenLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public static void SaveScore(int score, string levelName)
    {
        if (LoadScore(levelName) >= score) return;
        
        PlayerPrefs.SetInt(Library.PlayerPrefsSavedScore + "_" + levelName, score);
        PlayerPrefs.Save();
    }
    
    public static int LoadScore(string levelName)
    {
        var scoreName = Library.PlayerPrefsSavedScore + "_" + levelName;       
        return PlayerPrefs.HasKey(scoreName) ? PlayerPrefs.GetInt(scoreName, 0) : 0;
    }

    public static void CompleteLevel(int score, string levelName, bool isFinalLevel)
    {
        SaveScore(score, levelName + (isFinalLevel ? "" : "Temp"));
    }
}