using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class PrefabsConfig : ScriptableObject
    {
        public SkillBallForCollector skillBallForCollectorPrefab;
        public SkillBallForBattle skillBallForBattlePrefab;
        public GameObject deathBallParticleDefault;
    }
}