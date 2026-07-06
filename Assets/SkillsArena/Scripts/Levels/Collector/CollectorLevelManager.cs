using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    public class CollectorLevelManager : LevelManager, IPausable
    {
        public bool IsPaused { get; private set; }
        public CollectorLevelStateType CollectorLevelStateType { get; private set; }
        public Transition_UI transition;
        public Animator handSkillsManagerAnimator;

        [SerializeField] private BoundaryManager _boundaryManager;
        [SerializeField] private CollectorSkillsManager _collectorSkillsManager;
        [SerializeField] private CollectorLevel_UI_Manager _collectorLevelUIManager;
        [SerializeField] private HandSkillsManager_Collector _handSkillsCollectorManager;

        private GameFactory _gameFactory;
        private GameData _gameData;
        private InputService _inputService;

        private List<IPausable> pausables = new();

        public override void Init()
        {
            _gameFactory = ServiceLocator.Instance.GetService<GameFactory>();
            _gameData = ServiceLocator.Instance.GetService<GameData>();
            _inputService = ServiceLocator.Instance.GetService<InputService>();

            _collectorLevelUIManager.Init(this);
            _collectorLevelUIManager.OnPausePressed += SetPauseStatus;
            _collectorLevelUIManager.OnNextPressed += LoadBattleLevel;
            _collectorLevelUIManager.OnMenuPressed += ExitToMenu;

            _collectorLevelUIManager.OnStartPressed += _collectorSkillsManager.LevelLoop;
            _collectorLevelUIManager.OnStartPressed += AfterGameLoopStarted;

            _handSkillsCollectorManager.Init(_collectorLevelUIManager);

            _collectorSkillsManager.Init(_gameFactory, _gameData, _inputService, _boundaryManager, _collectorLevelUIManager, _handSkillsCollectorManager);
            _collectorSkillsManager.OnAllSkillsCollected += AfterAllCollected;
            pausables.Add(_collectorSkillsManager);
            pausables.Add(this);
        }

        public override void StartLevel()
        {
            transition.StartOpenAnim();
            _boundaryManager.SetBoundWalls();
            CheckForTutorial();
            CollectorLevelStateType = CollectorLevelStateType.Start;
        }

        private void Update()
        {
            if (_inputService != null)
            {
                _inputService.UpdateSomethingIfNeed();
            }
        }

        private void CheckForTutorial()
        {
            if (PlayerPrefs.GetInt(Constants.CollectorTutorialName, 0) == 0)
            {
                _collectorLevelUIManager.ShowTutorial();
                PlayerPrefs.SetInt(Constants.CollectorTutorialName, 1);
            }
        }

        private void SetPauseStatus(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
            foreach (var pausable in pausables)
                pausable.Pause(isPaused);
        }

        private void LoadBattleLevel()
        {
            CollectorLevelStateType = CollectorLevelStateType.None;
            ServiceLocator.Instance.GetService<SaveAndLoadData>().SaveGameData();
            transition.StartCloseAnim();
            _handSkillsCollectorManager.PrepareToAnim();
            handSkillsManagerAnimator.enabled = true;
            OnExitLevel.Invoke(this, Constants.BattleLevelSceneName, 1.2f);
        }

        public void Pause(bool pause)
        {
            IsPaused = pause;
        }

        private void ExitToMenu()
        {
            SetPauseStatus(false);
            transition.StartCloseAnim();
            OnExitLevel.Invoke(this, Constants.MenuSceneName, 1.2f);
        }

        private void AfterGameLoopStarted()
        {
            CollectorLevelStateType = CollectorLevelStateType.GameLoop;
        }

        private void AfterAllCollected()
        {
            CollectorLevelStateType = CollectorLevelStateType.End;
        }
    }

    ////I don't think it's the best solution, I added this at the end. State Machine is probably better.
    public enum CollectorLevelStateType
    {
        None, Start, GameLoop, End
    }
}