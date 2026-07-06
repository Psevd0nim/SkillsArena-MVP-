using UnityEngine;

namespace SkillsArena
{
    public class StartGameState : IDefaultState
    {
        private GameStateMachine _stateMachine;

        public StartGameState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            LevelContext levelContext = Object.FindAnyObjectByType<LevelContext>();
            LevelManager levelManager = levelContext.levelManager;
            levelManager.OnExitLevel += OnChangeLevel;
            levelManager.Init();
            levelManager.StartLevel();
        }

        private void OnChangeLevel(LevelManager levelManager, string levelName, float time)
        {
            levelManager.OnExitLevel -= OnChangeLevel;
            _stateMachine.Enter<LoadLevelState, string, float>(levelName, time);
        }

        public void Exit()
        {
            Debug.Log($"Out from {GetType()}");
        }
    }
}