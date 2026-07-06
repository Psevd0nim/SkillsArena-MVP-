using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class SandClock : MonoBehaviour
    {
        public event Action<SandClock> OnTimeEnd;
        public event Action<SandClock> OnLowTime;

        public SkillRareType RareType { get; private set; }

        public Image upImage;
        public Image downImage;

        private float _time;
        private Coroutine _coroutine;

        void Awake()
        {
            upImage.gameObject.SetActive(false);
            downImage.gameObject.SetActive(false);
        }

        public void Init(SandClockData sandClockData)
        {
            RareType = sandClockData.rareType;
            upImage.color = downImage.color = sandClockData.color;
            _time = sandClockData.time;

            upImage.gameObject.SetActive(true);
            downImage.gameObject.SetActive(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SandClockCoroutine());
        }

        private IEnumerator SandClockCoroutine()
        {
            float currentTime = _time;
            upImage.fillAmount = 1;
            downImage.fillAmount = 0;
            while (currentTime > 0)
            {
                float progressValue = currentTime / _time;
                upImage.fillAmount = progressValue;
                downImage.fillAmount = 1 - progressValue;
                currentTime -= Time.deltaTime;
                yield return null;

                if (currentTime <= 3)
                    OnLowTime?.Invoke(this);
            }
            OnTimeEnd?.Invoke(this);
        }
    }
}