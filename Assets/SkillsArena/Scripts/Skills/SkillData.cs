namespace SkillsArena
{
    public class SkillData
    {
        public SkillRareType skillRareType;
        public SkillElementType skillElementType;

        public SkillData(SkillRareType skillRareType, SkillElementType skillElementType)
        {
            this.skillRareType = skillRareType;
            this.skillElementType = skillElementType;
        }
    }
}