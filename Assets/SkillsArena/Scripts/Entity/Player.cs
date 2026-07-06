using UnityEngine;

namespace SkillsArena
{
    public class Player : Entity, ISaveable
    {
        public SkillCombination SkillCombination => _skillCombination;

        [SerializeField] private SkillCombination _skillCombination;

        private void Awake()
        {
            _skillCombination.OnSkillSet += AfterSkillSet;
            _skillCombination.OnSkillRemove += AfterSkillRemove;
        }

        public void Init(PlayerConfig playerConfig, PlayerData currentPlayerData)
        {
            Init(playerConfig.defaultHealth, currentPlayerData.currentHealth);
        }

        public override void Save()
        {
            PlayerData playerData = new PlayerData(CurrentHealth);
            ServiceLocator.Instance.GetService<GameData>().SetPlayerData(playerData);
        }

        private void AfterSkillSet()
        {
            AudioManager.Instance.PlaySomeSound(SoundType.SetSkill);
        }

        private void AfterSkillRemove()
        {
            
        }
    }
}