using System.Collections;
using UnityEngine;

namespace SkillsArena
{
    public class DirectionArrow : MonoBehaviour
    {
        [SerializeField] private InputLikeKeyboardType inputType;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Color _selectedColor;

        public InputLikeKeyboardType InputType => inputType;

        private Vector2 _startScale;

        void Awake()
        {
            _startScale = transform.localScale;
        }

        public void SetActive(bool status)
        {
            _sprite.color = status? _selectedColor : Color.white;
        }

        public void Pressed()
        {
            StartCoroutine(PressAnim());
        }

        private IEnumerator PressAnim()
        {
            float totalTime = 0.1f;
            float currentTime = 0;
            float currentPath = 0;
            Vector2 finalScale = _startScale * 1.1f;
            while (currentTime < totalTime)
            {
                currentPath = currentTime / totalTime;
                transform.localScale = Vector2.Lerp(_startScale, finalScale, currentPath);
                currentTime += Time.deltaTime;
                yield return null;
            }
            currentTime = 0;
            currentPath = 0;
            while (currentTime < totalTime)
            {
                currentPath = currentTime / totalTime;
                transform.localScale = Vector2.Lerp(finalScale, _startScale, currentPath);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}