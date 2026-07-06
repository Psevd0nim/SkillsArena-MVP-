using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class SoundButton : Button_UI
    {
        [SerializeField] private Image _mainView;
        [SerializeField] private Image _shadowView;
        [SerializeField] private Image _backImage;
        [SerializeField] private Sprite _enableSprite, _disableSprite;
        [SerializeField] private Color _enableColor, _disableColor;
        

        public void SetActive(bool active)
        {
            _mainView.sprite = _shadowView.sprite = active? _enableSprite : _disableSprite;
            _backImage.color = active? _enableColor : _disableColor;
        }
    }
}
