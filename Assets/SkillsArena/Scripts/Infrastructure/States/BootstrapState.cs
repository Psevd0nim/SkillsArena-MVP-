using UnityEngine;

namespace SkillsArena
{
    public class BootstrapState : IDefaultState
    {
        private ServiceLocator _serviceLocator;
        private GameStateMachine _gameStateMachine;

        public BootstrapState(ServiceLocator serviceLocator, GameStateMachine gameStateMachine)
        {
            _serviceLocator = serviceLocator;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            _serviceLocator.RegisterService(GetInputService());
            _serviceLocator.RegisterService(new GameFactory());
            _serviceLocator.RegisterService(new SaveAndLoadData());
            _serviceLocator.RegisterService(_serviceLocator.GetService<SaveAndLoadData>().LoadGameData());
        }

        private InputService GetInputService()
        {
            return Application.isMobilePlatform? new MobileInput() : new DesktopInput();
        }

        public void Exit()
        {
            Debug.Log($"Out from {GetType()}");
        }
    }
}