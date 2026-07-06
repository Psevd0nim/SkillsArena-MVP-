using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class SkillsCounter_UI : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI _counterText;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _incorrectColor;

        private Coroutine _progressBarCoroutine;
        private int _maxCount;

        public void Init(int maxCount)
        {
            _maxCount = maxCount;
            UpdateProgressBar(0);
        }

        public void UpdateProgressBar(int currentCount)
        {
            _counterText.text = $"{currentCount}/{_maxCount}";
            float progress = (float)currentCount / _maxCount;
            if (_progressBarCoroutine != null)
                StopCoroutine(_progressBarCoroutine);
            _progressBarCoroutine = StartCoroutine(SmoothProgressBar(progressBar.fillAmount, progress));

            if (progress == 1)
                _counterText.color = _correctColor;
            else if (progress > 1)
                _counterText.color = _incorrectColor;
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
                progressBar.fillAmount = currentValue;
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}