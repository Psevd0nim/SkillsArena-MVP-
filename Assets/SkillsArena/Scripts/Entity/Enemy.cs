using UnityEngine;

namespace SkillsArena
{
    public class Enemy : Entity, ISaveable
    {
        public SkillCombination SKillCombination => _skillCombination;
        public SkillCombinationData SkillCombinationData { get; private set; }
        public EnemySkillsRateData EnemySkillsRateData { get; private set; }

        [SerializeField] private SkillCombination _skillCombination;

        public void Init(EnemyConfig enemyConfig, EnemyData enemyData)
        {
            SkillCombinationData = enemyData.skillCombinationData;
            EnemySkillsRateData = enemyData.enemySkillsRateData;
            Init(enemyConfig.defaultHealth, enemyData.currentHealth);
            StartAnimation(AnimationType.Idle);
        }

        public void UpdateSkillCombinationData(SkillCombinationData skillCombinationData)
        {
            SkillCombinationData = skillCombinationData;
            Save();
        }

        public void IncreaseSkillsRateLevel()
        {
            EnemySkillsRateData.IncreaseRateLevel();
            Save();
        }

        public void ClearSkillCombinationData()
        {
            SkillCombinationData = new SkillCombinationData();
        }

        public override void Save()
        {
            EnemyData enemyData = new EnemyData(CurrentHealth, SkillCombinationData, EnemySkillsRateData);
            ServiceLocator.Instance.GetService<GameData>().SetEnemyData(enemyData);
        }
    }
}