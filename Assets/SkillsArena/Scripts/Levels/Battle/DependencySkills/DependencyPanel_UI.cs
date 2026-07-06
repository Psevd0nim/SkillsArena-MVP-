using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class DependencyPanel_UI : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        [SerializeField] private Animator _animator;
        [SerializeField] private Vector2 _showPosition;
        [SerializeField] private Vector2 _hidePosition;
        [SerializeField] private float _timeAnim;
        [SerializeField] private LayoutGroup _layoutGroup;

        private RectTransform _rectTransform;
        private Coroutine _coroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = _hidePosition;
            _layoutGroup.enabled = false;
        }

        public void SetDependencyPanelActive(bool status)
        {
            IsActive = status;
            if (IsActive)
                gameObject.SetActive(IsActive);
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SmoothShowAndHide());
        }

        private IEnumerator SmoothShowAndHide()
        {
            Vector2 startPos = _rectTransform.anchoredPosition;
            Vector2 endPos = IsActive ? _showPosition : _hidePosition;
            float currentTime = 0;
            float currentPath = 0;
            float timeAnim = Vector2.Distance(startPos, endPos) / Vector2.Distance(_showPosition, _hidePosition) * _timeAnim;
            while (currentPath < 1)
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, currentPath);
                currentPath = currentTime / timeAnim;
                currentTime += Time.deltaTime;
                yield return null;
            }
            _rectTransform.anchoredPosition = endPos;
            if (!IsActive)
                gameObject.SetActive(false);
        }
    }
}
