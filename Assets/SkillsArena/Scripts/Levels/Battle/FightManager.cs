using System;
using System.Collections;
using UnityEngine;

namespace SkillsArena
{
    public class FightManager : MonoBehaviour
    {
        public event Action OnOutOfSkills;
        public event Action<bool> OnPreparedToBattle;
        public event Action OnStartBattle;

        [SerializeField] private BattleSkillsManager _skillsManager;
        [SerializeField] private BattleLevel_UI_Manager _battleLevel_UI_Manager;

        private Player _player;
        private Enemy _enemy;
        private GameData _gameData;
        private LevelData _levelData;
        private EnemyHub _enemyHub;

        public void Init(BattleLevel_UI_Manager battleLevel_UI_Manager, Player player, EnemyHub enemyHub, LevelData levelData)
        {
            _battleLevel_UI_Manager = battleLevel_UI_Manager;
            battleLevel_UI_Manager.OnStartBattlePressed += StartBattle;

            _levelData = levelData;

            _player = player;
            _player.SkillCombination.OnCombinationFilled += AfterSkillCombinationFilled;

            _enemy = enemyHub.Enemy;
            _enemyHub = enemyHub;

            _gameData = ServiceLocator.Instance.GetService<GameData>();
        }

        public void PrepareToBattle()
        {
            if (_gameData.CollectedSkillsList.Count == 0)
            {
                OnOutOfSkills?.Invoke();
                return;
            }
            _skillsManager.TrySetSkillsToEnemyCombination();
        }

        public void AfterSkillCombinationFilled(bool filled)
        {
            _battleLevel_UI_Manager.ShowOrHideStartBattleButton(filled);
            OnPreparedToBattle?.Invoke(filled);
        }

        private void StartBattle()
        {
            _battleLevel_UI_Manager.ShowOrHideStartBattleButton(false);
            if (_enemyHub.Showing)
                _enemyHub.SmoothShowEnemyHub(false);
            StartCoroutine(BattleCoroutine());
            AudioManager.Instance.PlaySomeSound(SoundType.Fight);
            OnStartBattle?.Invoke();
        }

        private IEnumerator BattleCoroutine()
        {
            _player.StartAnimation(AnimationType.Attack);
            _enemy.StartAnimation(AnimationType.Attack);

            yield return new WaitForSeconds(1f);

            if (_player.SkillCombination.TotalDamage > _enemy.SKillCombination.TotalDamage)
            {
                _enemy.TakeDamage(_player.SkillCombination.TotalDamage - _enemy.SKillCombination.TotalDamage);
                _player.StartAnimation(AnimationType.Idle);
            }
            else if (_player.SkillCombination.TotalDamage < _enemy.SKillCombination.TotalDamage)
            {
                _player.TakeDamage(_enemy.SKillCombination.TotalDamage - _player.SkillCombination.TotalDamage);
                _enemy.StartAnimation(AnimationType.Idle);
            }
            else
            {
                _player.StartAnimation(AnimationType.Idle);
                _enemy.StartAnimation(AnimationType.Idle);
            }

            _skillsManager.Clear();
            _levelData.NextRound();
            ServiceLocator.Instance.GetService<SaveAndLoadData>().SaveGameData();
            PrepareToBattle();
        }
    }
}
