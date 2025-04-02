using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HudControl : MonoBehaviour
    {
        public static HudControl StaticHudControl { get; private set; }

        [SerializeField] private string levelName;
        [SerializeField] private List<GameObject> hudAwards;
        [SerializeField] private List<GameObject> hudLives;
        [SerializeField] private Text hudTime;
        [SerializeField] private Text hudScore;
        [SerializeField] private float timeLevel;
        [SerializeField] private int itemScore;
        [SerializeField] private Image imageAttackBar;
        [SerializeField] private Text textScore;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float showDuration;
        [SerializeField] private float jumpScale;

        private Color originalColor;
        private Vector3 originalScale;
        
        // Game Over
        [SerializeField] private Sprite iconGameOver;
        
        // Item Collected
        [SerializeField] private GameObject itemPanel;
        [SerializeField] private TextMeshProUGUI itemText;
        [SerializeField] private Image itemImage;
        
        private CanvasGroup canvasItemGroup;
        
        private int _score;

        private void Awake()
        {
            if (StaticHudControl == null)
            {
                StaticHudControl = this;
            }
            else
            {
                Destroy(gameObject);
            }
        
            DontDestroyOnLoad(gameObject);
            
            canvasItemGroup = itemPanel.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            originalColor = textScore.color;
            originalColor.a = 0; // Começa invisível
            textScore.color = originalColor;
            originalScale = textScore.transform.localScale;
            _score = GameControl.LoadScore(levelName + "Temp");
            UpdateScoreText();
        }

        private void Update()
        {
            if (timeLevel > 0)
            {
                timeLevel -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                GameOver();
            }
        }

        public void UpdateImageAttackSize(float percentSize)
        {
            imageAttackBar.fillAmount = percentSize / 100;
        }

        public void ShowAward(string awardName)
        {
            if (awardName != string.Empty)
            {
                var award = hudAwards.Find(x => x.name == awardName);
            
                var sprite = award.GetComponent<Image>().sprite;

                if (sprite != null)
                {
                    var message = awardName + " coletado !";

                    StartCoroutine(ShowMessage(message, sprite, null));
                    
                    AddScore(itemScore);
                }
            
                award.SetActive(true);
            }
        }

        public void SetPlayerLivesInHud(int playerLives)
        {
            for (var i = 0; i < playerLives; i++)
            {
                hudLives[i].SetActive(true);
            }

            if (hudLives.Count > playerLives)
            {
                hudLives[playerLives].SetActive(false);
            }
        }

        public void AddScore(int score)
        {
            StartCoroutine(ShowNewScore(score.ToString()));
            _score += score;
            UpdateScoreText();
        }

        public void RemoveScore(int score)
        {
            StartCoroutine(ShowNewScore("- " + score));
            
            _score -= score;

            if (_score < 0)
            {
                _score = 0;
            }
            
            UpdateScoreText();
        }

        private IEnumerator ShowNewScore(string newScore)
        {
            textScore.text = newScore;
            textScore.enabled = true;
            yield return StartCoroutine(FadeAndJump(1f)); // Fade In + Jump
            yield return new WaitForSeconds(showDuration); // Espera
            yield return StartCoroutine(FadeAndJump(0f)); // Fade Out
            textScore.enabled = false;
            textScore.text = "";
        }
        
        private IEnumerator FadeAndJump(float targetAlpha)
        {
            var elapsedTime = 0f;
            var startAlpha = textScore.color.a;
            var startScale = textScore.transform.localScale;
            var targetScale = Mathf.Approximately(targetAlpha, 1f) ? originalScale * jumpScale : originalScale;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / fadeDuration;

                // Interpolação do Alpha
                var newColor = textScore.color;
                newColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                textScore.color = newColor;

                // Interpolação do Scale (efeito de pulo)
                textScore.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                yield return null;
            }
        }

        private void UpdateScoreText()
        {
            hudScore.text = "Score: " + _score;
        }

        private void UpdateTimeText()
        {
            hudTime.text = Mathf.CeilToInt(timeLevel).ToString();
        }
        
        public void GameOver()
        {
            StartCoroutine(WaitAndReopenLevel());
        }
        
        private IEnumerator WaitAndReopenLevel()
        {
            yield return StartCoroutine(ShowMessage("Game Over", iconGameOver, null));
            
            GameControl.SaveScore(_score, levelName);
            GameControl.ReopenLevel();
        }
        
        public IEnumerator NewLevelScene(string sceneName, Sprite levelIcon)
        {
            yield return StartCoroutine(ShowMessage(sceneName, levelIcon, null));
        }
        
        public IEnumerator DefeatScene(Sprite iconDefeatEnemy, [CanBeNull] string nextSceneName)
        {
            yield return StartCoroutine(ShowMessage("Eliminado", iconDefeatEnemy, nextSceneName));
        }
        
        public IEnumerator UnlockCharacterScene(Sprite iconDefeatEnemy, [CanBeNull] string nextSceneName)
        {
            yield return StartCoroutine(ShowMessage("Personagem Desbloqueado", iconDefeatEnemy, nextSceneName));
        }

        private IEnumerator ShowMessage(string message, [CanBeNull] Sprite icon, [CanBeNull] string nextSceneName)
        {
            itemText.text = message;
            itemImage.sprite = icon;
            itemPanel.SetActive(true);
            
            Time.timeScale = 0f;
            
            yield return StartCoroutine(FadeIn(canvasItemGroup));

            yield return new WaitForSecondsRealtime(2f);

            yield return StartCoroutine(FadeOut(canvasItemGroup));
 
            itemPanel.SetActive(false);
            
            Time.timeScale = 1f;

            if (nextSceneName != null)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        private static IEnumerator FadeIn(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime * 2;
                yield return null;
            }
        }

        private static IEnumerator FadeOut(CanvasGroup canvasGroup)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * 2;
                yield return null;
            }
        }
    }
}