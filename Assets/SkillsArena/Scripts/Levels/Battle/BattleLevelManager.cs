using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    public class BattleLevelManager : LevelManager
    {
        public BattleLevelStateType BattleLevelStateType { get; private set; }

        [SerializeField] private HandSkillsManager_Battle _handSkillsBattleManager;
        [SerializeField] private BattleSkillsManager _skillsManager;
        [SerializeField] private BattleLevel_UI_Manager _battleLevelUIManager;
        [SerializeField] private FightManager _fightManager;
        [SerializeField] private DependencySkillsManager _dependencySkillsManager;
        [SerializeField] private Transition_UI _transition;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private Player _player;
        [SerializeField] private EnemyHub _enemyHub;

        private ServiceLocator _serviceLocator;
        private GameData _gameData;
        private SaveAndLoadData _saveAndLoadDataService;
        private InputService _inputService;
        private bool _gameOver;
        private List<IPausable> _pauseables = new List<IPausable>();

        private LevelData levelData = new LevelData();

        public override void Init()
        {
            _serviceLocator = ServiceLocator.Instance;
            _gameData = _serviceLocator.GetService<GameData>();
            _saveAndLoadDataService = _serviceLocator.GetService<SaveAndLoadData>();
            _inputService = ServiceLocator.Instance.GetService<InputService>();

            if (!_gameData.WasInited)
                InitGameData();

            _dependencySkillsManager.Init(_gameData.CurrentDependencySkillsData);

            _player.Init(_playerConfig, _gameData.CurrentPlayerData);
            _player.OnDeath += GameOver;

            _enemyHub.Enemy.OnDeath += AfterEnemyDeath;
            _enemyHub.EnemyInit(_enemyConfig, _gameData);

            levelData.Init(_battleLevelUIManager, _gameData);

            _fightManager.Init(_battleLevelUIManager, _player, _enemyHub, levelData);
            _fightManager.OnOutOfSkills += OutOfSkills;
            _fightManager.OnPreparedToBattle += AfterPreparedToBattle;
            _fightManager.OnStartBattle += AfterStartBattle;
            
            _handSkillsBattleManager.Init();

            _skillsManager.Init(_dependencySkillsManager, _player, _enemyHub.Enemy, levelData);
            
            _pauseables.Add(_skillsManager);

            _battleLevelUIManager.Init(_gameData, this);
            _battleLevelUIManager.OnNextLevelPressed += LoadCollectorLevelScene;
            _battleLevelUIManager.OnRestartPressed += RestartGame;
            _battleLevelUIManager.OnMenuPressed += ExitToMenu;
            _battleLevelUIManager.OnPausePressed += PauseGame;
            _pauseables.Add(_battleLevelUIManager);
        }

        public override void StartLevel()
        {
            CheckForTutorial();

            if(_gameData.CollectedSkillsList.Count == 0)
            {
                BattleLevelStateType = BattleLevelStateType.OutOfSkills;
                _battleLevelUIManager.ShowCantPlayPanel();
                return;
            }
            BattleLevelStateType = BattleLevelStateType.NeedToPrepare;
            _fightManager.PrepareToBattle();
            _handSkillsBattleManager.TrySpawnSkills();
            _enemyHub.SmoothShowEnemyHub();
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
            if (PlayerPrefs.GetInt(Constants.BattleTutorialName, 0) == 0)
            {
                _battleLevelUIManager.ShowTutorial();
                PlayerPrefs.SetInt(Constants.BattleTutorialName, 1);
            }
        }

        private void AfterEnemyDeath()
        {
            _gameData.IncreaseEnemiesDefeated();
            _enemyHub.AfterEnemyDeath(_enemyConfig, _gameData);
            _saveAndLoadDataService.SaveGameData();
        }

        private void RestartGame()
        {
            BattleLevelStateType = BattleLevelStateType.None;
            _transition.StartCloseAnim();
            OnExitLevel?.Invoke(this, Constants.BattleLevelSceneName, 1.2f);
            AudioManager.Instance.SetBackgroundStatus(true);
        }

        private void LoadCollectorLevelScene()
        {
            BattleLevelStateType = BattleLevelStateType.None;
            _transition.StartCloseAnim();
            OnExitLevel?.Invoke(this, Constants.CollectorLevelSceneName, 1.2f);
        }

        private void ExitToMenu()
        {
            BattleLevelStateType = BattleLevelStateType.None;
            PauseGame(false);
            _transition.StartCloseAnim();
            OnExitLevel?.Invoke(this, Constants.MenuSceneName, 1.2f);
            AudioManager.Instance.SetBackgroundStatus(true);
        }

        private void OutOfSkills()
        {
            if (_gameOver)
            {
                return;
            }
            BattleLevelStateType = BattleLevelStateType.OutOfSkills;
            _battleLevelUIManager.ShowCantPlayPanel();
            _gameData.SetDependencySkillsData(_dependencySkillsManager.GetDependencySkillsDataRandom());
            _dependencySkillsManager.Init(_gameData.CurrentDependencySkillsData);
        }

        private void InitGameData()
        {
            EnemySkillsRateData enemySkillsRateData = new EnemySkillsRateData(_enemyConfig.ratesList);
            EnemyData enemyData = new EnemyData(_enemyConfig.defaultHealth, new SkillCombinationData(), enemySkillsRateData);
            PlayerData playerData = new PlayerData(_playerConfig.defaultHealth);
            _gameData.Init(enemyData, playerData, _dependencySkillsManager.GetDependencySkillsDataRandom());
            _saveAndLoadDataService.SaveGameData();
        }

        private void PauseGame(bool pause)
        {
            Time.timeScale = pause ? 0 : 1;
            foreach (var pauseable in _pauseables)
            {
                pauseable.Pause(pause);
            }
        }

        private void GameOver()
        {
            AudioManager.Instance.PlaySomeSound(SoundType.GameOver);
            AudioManager.Instance.SetBackgroundStatus(false);
            BattleLevelStateType = BattleLevelStateType.GameOver;
            _battleLevelUIManager.ShowGameOverPanel();
            _gameOver = true;
            foreach (var pauseable in _pauseables)
            {
                pauseable.Pause(true);
            }

            _gameData.Clear();
            _saveAndLoadDataService.SaveGameData();
        }

        private void AfterPreparedToBattle(bool prepared)
        {
            BattleLevelStateType = prepared? BattleLevelStateType.Prepared : BattleLevelStateType.NeedToPrepare;
        }

        private void AfterStartBattle()
        {
            BattleLevelStateType = BattleLevelStateType.Battle;
        }
    }

    ////I don't think it's the best solution, I added this at the end. State Machine is probably better.
    public enum BattleLevelStateType
    {
        None, NeedToPrepare, Prepared, Battle, OutOfSkills, GameOver
    }
}