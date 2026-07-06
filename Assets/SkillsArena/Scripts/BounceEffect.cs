using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkillsArena
{
    public class BounceEffect : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private float _bounceTime = 0.2f;
        [SerializeField] private float _bounceScale = 1.1f;

        private Vector2 _startScale;
        private Coroutine _bounceCoroutine;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Bounce();
        }

        public void Bounce()
        {
            if (_bounceCoroutine != null)
                StopCoroutine(_bounceCoroutine);
            if (gameObject.activeInHierarchy)
                _bounceCoroutine = StartCoroutine(BounceCoroutine());
        }

        private IEnumerator BounceCoroutine()
        {
            transform.localScale = _startScale;

            float halfTime = _bounceTime / 2;
            Vector2 targetScale = _startScale * _bounceScale;

            yield return AnimationScale(_startScale, targetScale, halfTime);
            yield return AnimationScale(targetScale, _startScale, halfTime);
        }

        private IEnumerator AnimationScale(Vector2 startScale, Vector2 targetScale, float duration)
        {
            float currentTime = 0;

            while (currentTime < duration)
            {
                transform.localScale = Vector2.Lerp(startScale, targetScale, currentTime / duration);

                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}
