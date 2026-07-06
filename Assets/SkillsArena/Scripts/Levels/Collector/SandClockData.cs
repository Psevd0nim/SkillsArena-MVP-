using UnityEngine;

namespace SkillsArena
{
    public class SandClockData
    {
        public SkillRareType rareType;
        public Color color;
        public float time;

        public SandClockData(SkillRareType rareType, Color color, float time)
        {
            this.rareType = rareType;
            this.color = color;
            this.time = time;
        }
    }
}