using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkillsArena
{
    public class GameData : IService
    {
        public List<SkillData> CollectedSkillsList { get; private set; } = new();
        public EnemyData CurrentEnemyData { get; private set; } = new();
        public PlayerData CurrentPlayerData { get; private set; } = new();
        public DependencySkillsData CurrentDependencySkillsData { get; private set; } = new();
        [JsonProperty] public bool WasInited { get; private set; }
        [JsonProperty] public int CurrentRound { get; private set; }
        [JsonProperty] public int CurrentEnemiesDefeated { get; private set; }

        public void Init(EnemyData enemyData, PlayerData playerData, DependencySkillsData dependencySkillsData)
        {
            CurrentEnemyData = enemyData;
            CurrentPlayerData = playerData;
            CurrentDependencySkillsData = dependencySkillsData;
            WasInited = true;
            CurrentRound = 1;
        }

        public void AddCollectedSkill(SkillData skillData)
        {
            CollectedSkillsList.Add(skillData);
        }

        public void RemoveCollectedSkill(SkillData skillData)
        {
            CollectedSkillsList.Remove(skillData);
        }

        public void SetEnemyData(EnemyData enemyData)
        {
            CurrentEnemyData = enemyData;
        }

        public void SetPlayerData(PlayerData playerData)
        {
            CurrentPlayerData = playerData;
        }

        public void SetDependencySkillsData(DependencySkillsData dependencySkillsData)
        {
            CurrentDependencySkillsData = dependencySkillsData;
        }

        public void SetCurrentRound(int round)
        {
            CurrentRound = round;
        }

        public void IncreaseEnemiesDefeated()
        {
            CurrentEnemiesDefeated++;
        }

        public void Clear()
        {
            CollectedSkillsList.Clear();
            CurrentEnemyData = new EnemyData();
            CurrentPlayerData = new PlayerData();
            CurrentDependencySkillsData = new DependencySkillsData();
            CurrentEnemiesDefeated = 0;
            WasInited = false;
        }
    }
}