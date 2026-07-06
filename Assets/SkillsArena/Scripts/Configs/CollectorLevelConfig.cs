using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class CollectorLevelConfig : ScriptableObject
    {
        public List<CollectorBallData> skillsInCollectorLevel;
    }
}