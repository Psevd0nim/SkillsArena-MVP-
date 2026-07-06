using System;
using System.Collections.Generic;

namespace SkillsArena
{
    [Serializable]
    public class CollectorBallData
    {
        public SkillRareType skillRareType;
        public int count;
        public float timeLive;
        public List<EncodedData> encodeDataList;
    }
}