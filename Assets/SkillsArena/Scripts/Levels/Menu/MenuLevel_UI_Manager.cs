using System;
using UnityEngine;

namespace SkillsArena
{
    public class MenuLevel_UI_Manager : UI_Manager
    {
        public event Action OnPlayPressed;

        private InputService _inputService;
        private MenuLevelManager _menuLevelManager;

        [SerializeField] private Button_UI _playButton;

        public void Init(MenuLevelManager menuLevelManager)
        {
            base.Init();
            _inputService = ServiceLocator.Instance.GetService<InputService>();
            _menuLevelManager = menuLevelManager;
            _playButton.OnPressed += PlayButtonPressed;
        }

        private void Update()
        {
            CheckInput();
        }

        public void PlayButtonPressed()
        {
            OnPlayPressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        private void CheckInput()
        {
            if (_inputService == null)
            {
                return;
            }

            InputLikeKeyboardType inputType = _inputService.GetCurrentKeyWasReleasedThisFrame();

            if (inputType == InputLikeKeyboardType.Space && _menuLevelManager.MenuLevelStateType == MenuLevelStateType.InMenu)
            {
                PlayButtonPressed();
                if(_playButton.TryGetComponent(out BounceEffect bounceLogic))
                {
                    bounceLogic.Bounce();
                }
            }
        }
    }
}