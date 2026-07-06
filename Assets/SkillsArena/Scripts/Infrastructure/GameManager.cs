using UnityEngine;

namespace SkillsArena
{
    public class GameManager
    {
        public GameStateMachine gameStateMachine;

        public GameManager(ICoroutineRunner coroutineRunner)
        {
            Application.targetFrameRate = 144;
            gameStateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), ServiceLocator.Instance);
        }
    }
}