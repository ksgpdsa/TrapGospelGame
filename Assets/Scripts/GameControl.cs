using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private static GameControl _instance;
    private bool _inLevel;
    private string _levelName;
    private int _score;
    private float _timeLevel;

    public static GameControl StaticGameControl
    {
        get
        {
            if (_instance) return _instance;

            var gameObject = new GameObject("GameControl");
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.AddComponent<GameControl>();

            return _instance;
        }
    }

    private void Update()
    {
        if (_inLevel)
        {
            if (_timeLevel > 0)
            {
                _timeLevel -= Time.deltaTime;
                HudControl.StaticHudControl.UpdateTimeText(_timeLevel);
            }
            else
            {
                GameOver();
            }
        }
    }

    public void SetLevelName(string levelName)
    {
        _levelName = levelName;
    }

    public void SetLevelTime(float time)
    {
        _timeLevel = time;
    }

    public void SetInLevel(bool inLevel)
    {
        _inLevel = inLevel;
    }

    public void AddScore(int score)
    {
        StartCoroutine(HudControl.StaticHudControl.ShowNewScore(score.ToString()));
        _score += score;
        HudControl.StaticHudControl.UpdateScoreText(_score);
    }

    public void RemoveScore(int score)
    {
        StartCoroutine(HudControl.StaticHudControl.ShowNewScore("- " + score));

        _score -= score;

        if (_score < 0) _score = 0;

        HudControl.StaticHudControl.UpdateScoreText(_score);
    }

    public int GetScore()
    {
        return _score;
    }

    private void ReopenLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private static void SaveScore(int score, string levelName)
    {
        if (LoadScore(levelName) >= score) return;

        PlayerPrefs.SetInt(Library.PlayerPrefsSavedScore + "_" + levelName, score);
        PlayerPrefs.Save();
    }

    private static int LoadScore(string levelName)
    {
        var scoreName = Library.PlayerPrefsSavedScore + "_" + levelName;
        return PlayerPrefs.HasKey(scoreName) ? PlayerPrefs.GetInt(scoreName, 0) : 0;
    }

    public static void SaveRespawnPoint(int respawnPoint)
    {
        PlayerPrefs.SetInt(Library.PlayerPrefsRespawnPoint, respawnPoint);
        PlayerPrefs.Save();
    }

    public static int LoadRespawnPoint()
    {
        return PlayerPrefs.HasKey(Library.PlayerPrefsRespawnPoint)
            ? PlayerPrefs.GetInt(Library.PlayerPrefsRespawnPoint, 0)
            : 0;
    }

    public static void CompleteLevel(int score, string levelName, bool isFinalLevel)
    {
        ResetRespawnPoints();
        SaveScore(score, levelName + (isFinalLevel ? "" : "Temp"));
    }

    public void BackToMainMenu()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(Library.Menu);
    }

    public void GameOver()
    {
        StopAllCoroutines();
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return StartCoroutine(HudControl.StaticHudControl.MessageGameOver());

        SaveScore(_score, _levelName);
        ReopenLevel();
    }

    public static void ResetRespawnPoints()
    {
        SaveRespawnPoint(0);
    }
}