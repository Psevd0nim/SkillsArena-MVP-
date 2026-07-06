using System.Collections;
using UnityEngine;

namespace SkillsArena
{
    public class EnemyHub : EntityHub
    {
        public Enemy Enemy => _enemy;
        public bool Showing => _showCoroutine != null;

        [SerializeField] private Enemy _enemy;
        [SerializeField] private Vector2 _startPosEnemyHub;
        [SerializeField] private Vector2 _endPosEnemyHub;
        [SerializeField] private float _timeAnim;

        private Coroutine _showCoroutine;

        public void EnemyInit(EnemyConfig enemyConfig, GameData gameData)
        {
            _enemy.Init(enemyConfig, gameData.CurrentEnemyData);
            transform.position = _startPosEnemyHub;
        }

        public void AfterEnemyDeath(EnemyConfig enemyConfig, GameData gameData)
        {
            _enemy.IncreaseSkillsRateLevel();
            EnemyData enemyData = new EnemyData(enemyConfig.defaultHealth, new SkillCombinationData(), gameData.CurrentEnemyData.enemySkillsRateData);
            gameData.SetEnemyData(enemyData);
            EnemyInit(enemyConfig, gameData);
            SmoothShowEnemyHub();
        }

        private IEnumerator SmoothShowEnemyHubCoroutine()
        {
            Animator enemyAnimator = _enemy.GetComponent<Animator>();
            enemyAnimator.SetBool("Run", true);
            float currentTime = 0;
            float currentPath = 0;
            float timeAnim = _timeAnim;
            while (currentPath < 1)
            {
                transform.position = Vector2.Lerp(_startPosEnemyHub, _endPosEnemyHub, currentPath);
                currentPath = currentTime / timeAnim;
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.position = _endPosEnemyHub;
            enemyAnimator.SetBool("Run", false);
            _showCoroutine = null;
        }

        public void SmoothShowEnemyHub(bool smooth = true)
        {
            if (smooth)
                _showCoroutine = StartCoroutine(SmoothShowEnemyHubCoroutine());
            else
            {
                if (_showCoroutine != null)
                {
                    StopCoroutine(_showCoroutine);
                }
                transform.position = _endPosEnemyHub;
                Animator enemyAnimator = _enemy.GetComponent<Animator>();
                enemyAnimator.SetBool("Run", false);
            }
        }
    }
}
