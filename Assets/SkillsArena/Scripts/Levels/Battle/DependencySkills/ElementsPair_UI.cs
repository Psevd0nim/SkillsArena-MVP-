using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class ElementsPair_UI : MonoBehaviour
    {
        [SerializeField] private Image _firstElementImage;
        [SerializeField] private Image _secondElementImage;
        [SerializeField] private TextMeshProUGUI _ratioText;
        [SerializeField] private Image _ratioImage;
        [SerializeField] private Color _goodColor, _badColor;

        public void Init(Sprite firstSprite, Sprite secondSprite, float ratioValue)
        {
            _firstElementImage.sprite = firstSprite;
            _secondElementImage.sprite = secondSprite;

            _ratioText.text = ratioValue.ToString(CultureInfo.InvariantCulture);
            if (ratioValue > 1)
                _ratioImage.color = _goodColor;
            else if (ratioValue < 1)
                _ratioImage.color = _badColor;
        }
    }
}
