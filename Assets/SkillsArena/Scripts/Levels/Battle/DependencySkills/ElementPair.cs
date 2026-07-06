using System;

namespace SkillsArena
{
    [Serializable]
    public struct ElementPair
    {
        public SkillElementType firstElementType;
        public SkillElementType secondElementType;

        public ElementPair(SkillElementType firstElementType, SkillElementType secondElementType)
        {
            this.firstElementType = firstElementType;
            this.secondElementType = secondElementType;
        }

        public override bool Equals(object obj)
        {
            if (obj is not ElementPair other)
                return false;

            bool firstVar =
                firstElementType == other.firstElementType &&
                secondElementType == other.secondElementType;

            bool secondVar =
                firstElementType == other.secondElementType &&
                secondElementType == other.firstElementType;

            return firstVar || secondVar;
        }

        public override int GetHashCode()
        {
            return firstElementType.GetHashCode() + secondElementType.GetHashCode();
        }
    }
}