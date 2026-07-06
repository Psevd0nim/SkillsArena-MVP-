using System;
using TMPro;
using UnityEngine;

namespace SkillsArena
{
    public class BattleLevel_UI_Manager : UI_Manager, IPausable
    {
        public event Action OnNextLevelPressed;
        public event Action<bool> OnPausePressed;
        public event Action OnStartBattlePressed;
        public event Action OnRestartPressed;
        public event Action OnMenuPressed;

        [SerializeField] private GameObject _cantPlayPanel;
        [SerializeField] private DependencyPanel_UI _dependencyPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _startBattleButton;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private HowToPlay_UI _tutorialPanel;
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private TextMeshProUGUI _gameOverStatsText;
        [SerializeField] private Button_UI _nextButton;

        private BattleLevelManager _battleLevelManager;
        private GameData _gameData;
        private InputService _inputService;
        private bool _paused;
        private bool _tutorialEnabled;

        public void Init(GameData gameData, BattleLevelManager battleLevelManager)
        {
            base.Init();
            _gameData = gameData;
            _inputService = ServiceLocator.Instance.GetService<InputService>();
            _battleLevelManager = battleLevelManager;
            _nextButton.OnPressed += NextLevelPressed;

            _tutorialPanel.OnClosed += CloseTutorial;
        }

        private void Update()
        {
            CheckInput();
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
                if (inputType == InputLikeKeyboardType.Escape && _battleLevelManager.BattleLevelStateType != BattleLevelStateType.None
                && _battleLevelManager.BattleLevelStateType != BattleLevelStateType.GameOver)
                {
                    if (!_paused)
                        PausePressed();
                    else
                        ResumePressed();
                }

                if (inputType == InputLikeKeyboardType.Space && _battleLevelManager.BattleLevelStateType == BattleLevelStateType.Prepared && !_paused)
                {
                    StartBattlePressed();
                }

                if (inputType == InputLikeKeyboardType.Space && _battleLevelManager.BattleLevelStateType == BattleLevelStateType.OutOfSkills && !_paused)
                {
                    NextLevelPressed();
                    if (_nextButton.TryGetComponent(out BounceEffect bounceLogic))
                    {
                        bounceLogic.Bounce();
                    }
                }
            }
        }

        public void NextLevelPressed()
        {
            OnNextLevelPressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
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

        public void ShowCantPlayPanel()
        {
            _cantPlayPanel.SetActive(true);
        }

        public void DependencyButtonPressed()
        {
            _dependencyPanel.SetDependencyPanelActive(!_dependencyPanel.IsActive);
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void ShowOrHideStartBattleButton(bool show)
        {
            _startBattleButton.SetActive(show);
        }

        public void StartBattlePressed()
        {
            OnStartBattlePressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void ShowGameOverPanel()
        {
            _gameOverStatsText.text = $"Enemies Defeated: {_gameData.CurrentEnemiesDefeated}";
            _gameOverPanel.SetActive(true);
        }

        public void RestartButtonPressed()
        {
            OnRestartPressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void UpdateCurrentRoundText(int currentRound)
        {
            _roundText.text = $"ROUND {currentRound}";
        }

        public void MenuPressed()
        {
            OnMenuPressed?.Invoke();
            _audioManager.PlaySomeSound(SoundType.ClickButton);
        }

        public void Pause(bool pause)
        {
            _paused = pause;
        }
    }
}
