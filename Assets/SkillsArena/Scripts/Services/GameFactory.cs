using UnityEngine;

namespace SkillsArena
{
    public class GameFactory : IService
    {
        private PrefabsConfig _prefabsConfig;

        public GameFactory()
        {
            _prefabsConfig = Resources.Load<PrefabsConfig>(Constants.PrefabsConfigPath);
        }

        public SkillBallForCollector GetSkillBallForCollector()
        {
            return Object.Instantiate(_prefabsConfig.skillBallForCollectorPrefab);
        }

        public SkillBallForBattle GetSkillBallForBattle(Transform parent)
        {
            return Object.Instantiate(_prefabsConfig.skillBallForBattlePrefab, parent);
        }

        public GameObject GetDeathBallParticle(Transform parent)
        {
            return Object.Instantiate(_prefabsConfig.deathBallParticleDefault);
        }
    }
}