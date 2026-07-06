using System;
using UnityEngine;

namespace SkillsArena
{
    [Serializable]
    public class EncodedData
    {
        public EncodedType encodedType;
        [Min(1)] public int encodedLength = 1;
    }
}