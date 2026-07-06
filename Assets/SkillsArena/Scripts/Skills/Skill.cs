namespace SkillsArena
{
    public class Skill
    {
        public SkillRareData SkillRareData { get; }
        public SkillElementData SkillElementData { get; }
        public SkillData SkillData { get; }

        public Skill(SkillRareData skillRareData, SkillElementData skillElementData, SkillData skillData)
        {
            SkillRareData = skillRareData;
            SkillElementData = skillElementData;
            SkillData = skillData;
        }
    }
}