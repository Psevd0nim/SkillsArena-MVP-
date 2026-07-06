using System;
using UnityEngine;

namespace SkillsArena
{
    public class CollectorLevel_UI_Manager : UI_Manager
    {
        public event Action<bool> OnPausePressed;
        public event Action OnNextPressed;
        public event Action OnStartPressed;
        public event Action OnMenuPressed;

        [SerializeField] private SkillsCounter_UI _skillsCounter;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _finalPanel;
        [SerializeField] private GameObject _notFinalPanel;
        [SerializeField] private HowToPlay_UI _tutorialPanel;
        [SerializeField] private Button_UI _startButton;

        private InputService _inputService;
        private CollectorLevelManager _collectorLevelManager;
        private bool _tutorialEnabled;

        private void Update()
        {
            CheckInput();
        }

        public void Init(CollectorLevelManager collectorLevelManager)
        {
            base.Init();
            _collectorLevelManager = collectorLevelManager;
            _inputService = ServiceLocator.Instance.GetService<InputService>();
            _skillsCounter.Init(9);

            _startButton.OnPressed += StartPressed;

            _tutorialPanel.OnClosed += CloseTutorial;
        }

        public void PausePressed()
        {
            OnPausePressed?.Invoke(true);
            _pausePanel.SetActive(true);
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void ResumePressed()
        {
            OnPausePressed?.Invoke(false);
            _pausePanel.SetActive(false);
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void NextPressed()
        {
            OnNextPressed?.Invoke();
            _finalPanel.SetActive(false);
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        private void CheckInput()
        {
            if (_inputService == null)
            {
                return;
            }

            InputLikeKeyboardType inputType = _inputService.GetCurrentKeyWasReleasedThisFrame();

            if (!_tutorialEnabled)
            {
                if (inputType == InputLikeKeyboardType.Escape && _collectorLevelManager.CollectorLevelStateType != CollectorLevelStateType.None)
                {
                    if (!_collectorLevelManager.IsPaused)
                        PausePressed();
                    else
                        ResumePressed();
                }

                if (inputType == InputLikeKeyboardType.Space && _collectorLevelManager.CollectorLevelStateType == CollectorLevelStateType.Start && !_collectorLevelManager.IsPaused)
                {
                    StartPressed();
                }

                if (inputType == InputLikeKeyboardType.Space && _collectorLevelManager.CollectorLevelStateType == CollectorLevelStateType.End && !_collectorLevelManager.IsPaused)
                {
                    NextPressed();
                }
            }
        }

        public void ShowFinalPanel()
        {
            _notFinalPanel.SetActive(false);
            _finalPanel.SetActive(true);
        }

        public void ShowNotFinalPanel()
        {
            _notFinalPanel.SetActive(true);
        }

        public void UpdateProgressBar(int currentCount)
        {
            _skillsCounter.UpdateProgressBar(currentCount);
        }

        public void StartPressed()
        {
            OnStartPressed?.Invoke();
            _startButton.gameObject.SetActive(false);
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void ShowTutorial(bool show = true)
        {
            _tutorialEnabled = show;
            _tutorialPanel.gameObject.SetActive(show);
        }

        public void TutorialPressed()
        {
            _audioManager.PlaySomeSound(SoundType.ClickButton);
            ShowTutorial();
        }

        public void CloseTutorial()
        {
            ShowTutorial(false);
        }

        public void MenuPressed()
        {
            OnMenuPressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }
    }
}
