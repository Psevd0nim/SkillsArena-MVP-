using UnityEngine;

namespace SkillsArena
{
    public class Enemy : Entity, ISaveable
    {
        public SkillCombination SKillCombination => _skillCombination;
        public SkillCombinationData SkillCombinationData { get; private set; }
        public EnemySkillsRateData EnemySkillsRateData { get; private set; }

        [SerializeField] private SkillCombination _skillCombination;

        private ColorType _colorType;

        public void Init(EnemyConfig enemyConfig, EnemyData enemyData)
        {
            SkillCombinationData = enemyData.skillCombinationData;
            EnemySkillsRateData = enemyData.enemySkillsRateData;
            _colorType = enemyData.colorType;
            _view.color = enemyConfig.GetColorByType(_colorType);
            _entityView_UI.SetColorForView(_view.color);
            Init(enemyConfig.defaultHealth, enemyData.currentHealth);
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
            EnemyData enemyData = new EnemyData(CurrentHealth, SkillCombinationData, EnemySkillsRateData, _colorType);
            ServiceLocator.Instance.GetService<GameData>().SetEnemyData(enemyData);
        }
    }
}