using System;
using System.Collections.Generic;

namespace SkillsArena
{
    public class GameStateMachine
    {
        private Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, ServiceLocator serviceLocator)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(serviceLocator, this),
                [typeof(LoadLevelState)] = new LoadLevelState(sceneLoader, this),
                [typeof(StartGameState)] = new StartGameState(this)
            };
        }

        public void Enter<TState>() where TState : class, IDefaultState
        {
            TState currentState = ChangeState<TState>();
            currentState.Enter();
        }

        public void Enter<TState, TPayload, TPayload2>(TPayload payload, TPayload2 payload2) where TState : class, IPayloadedState<TPayload, TPayload2>
        {
            TState currentState = ChangeState<TState>();
            currentState.Enter(payload, payload2);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();

            TState currentState = GetState<TState>();
            _activeState = currentState;

            return currentState;
        }

        private TState GetState<TState>() where TState : class, IState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}