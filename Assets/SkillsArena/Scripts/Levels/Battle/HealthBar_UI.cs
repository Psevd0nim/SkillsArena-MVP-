using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class HealthBar_UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _fillImage;

        private Coroutine _progressBarCoroutine;

        public void UpdateHealthView(int currentHealth, int maxHealth)
        {
            float progress = (float)currentHealth / maxHealth;
            if (_progressBarCoroutine != null)
                StopCoroutine(_progressBarCoroutine);
            _progressBarCoroutine = StartCoroutine(SmoothProgressBar(_fillImage.fillAmount, progress));
            _healthText.text = $"{currentHealth}/{maxHealth}";
        }

        private IEnumerator SmoothProgressBar(float startValue, float endValue)
        {
            float currentValue = 0;
            float progressValue = 0;
            float currentTime = 0;
            float time = 0.2f;
            while (progressValue < 1)
            {
                progressValue = currentTime / time;
                currentValue = Mathf.Lerp(startValue, endValue, progressValue);
                _fillImage.fillAmount = currentValue;
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
