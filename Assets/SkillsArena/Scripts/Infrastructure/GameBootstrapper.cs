using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkillsArena
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private static GameBootstrapper _instance;
        private GameManager _gameManager;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            _gameManager = new GameManager(this);
            _gameManager.gameStateMachine.Enter<BootstrapState>();
        }

        private void Start()
        {
            string targetSceneName = Constants.MenuSceneName;
            
            #if UNITY_EDITOR
                targetSceneName = SceneManager.GetActiveScene().name;
            #endif

            _gameManager.gameStateMachine.Enter<LoadLevelState, string, float>(targetSceneName, 0f);
        }
    }
}
