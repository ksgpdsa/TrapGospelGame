using System.Collections;
using System.Collections.Generic;
using Extensions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HudControl : MonoBehaviour
    {
        [SerializeField] private string levelName;
        [SerializeField] private List<GameObject> hudAwards;
        [SerializeField] private List<GameObject> hudLives;
        [SerializeField] private TextMeshProUGUI hudTime;
        [SerializeField] private TextMeshProUGUI hudScore;
        [SerializeField] private TextMeshProUGUI textScore;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float showDuration;
        [SerializeField] private float jumpScale;

        // Game Over
        [SerializeField] private Sprite iconGameOver;

        // Item Collected
        [SerializeField] private GameObject itemPanel;
        private CanvasGroup _canvasItemGroup;
        private Image _itemImage;

        private TextMeshProUGUI _itemText;
        private Color _originalColor;
        private Vector3 _originalScale;
        public static HudControl StaticHudControl { get; private set; }

        private void Awake()
        {
            if (StaticHudControl == null)
                StaticHudControl = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            GameControl.StaticGameControl.AddScore(0);
            _canvasItemGroup = itemPanel.GetComponent<CanvasGroup>();
            _itemText = itemPanel.GetComponentOnlyInChildren<TextMeshProUGUI>();
            _itemImage = itemPanel.GetComponentOnlyInChildren<Image>();
        }

        private void Start()
        {
            _originalColor = textScore.color;
            _originalColor.a = 0; // Começa invisível
            textScore.color = _originalColor;
            _originalScale = textScore.transform.localScale;
        }

        public void UpdateImageSize(float percentSize, Image imageBar)
        {
            imageBar.fillAmount = percentSize / 100;
        }

        public void ShowAward(string awardName, int score)
        {
            if (awardName != string.Empty)
            {
                var award = hudAwards.Find(x => x.name == awardName);

                var sprite = award.GetComponent<Image>().sprite;

                if (sprite != null)
                {
                    var message = awardName + " coletado !";

                    StartCoroutine(ShowMessage(message, sprite, null));

                    GameControl.StaticGameControl.AddScore(score);
                }

                award.SetActive(true);
            }
        }

        public void SetPlayerLivesInHud(int playerLives)
        {
            for (var i = 0; i < playerLives; i++) hudLives[i].SetActive(true);

            if (hudLives.Count > playerLives) hudLives[playerLives].SetActive(false);
        }

        public IEnumerator ShowNewScore(string newScore)
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
            var targetScale = Mathf.Approximately(targetAlpha, 1f) ? _originalScale * jumpScale : _originalScale;

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

        public void UpdateScoreText(int score)
        {
            hudScore.text = "Score: " + score;
        }

        public void UpdateTimeText(float timeLevel)
        {
            hudTime.text = Mathf.CeilToInt(timeLevel).ToString();
        }

        public IEnumerator MessageGameOver()
        {
            yield return StartCoroutine(ShowMessage("Game Over", iconGameOver, null));
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
            _itemText.text = message;
            _itemImage.sprite = icon;
            itemPanel.SetActive(true);

            Time.timeScale = 0f;

            yield return StartCoroutine(FadeIn(_canvasItemGroup));

            yield return new WaitForSecondsRealtime(2f);

            yield return StartCoroutine(FadeOut(_canvasItemGroup));

            itemPanel.SetActive(false);

            Time.timeScale = 1f;

            if (nextSceneName != null)
            {
                StopAllCoroutines();
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