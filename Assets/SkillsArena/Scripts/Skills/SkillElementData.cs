using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillsArena
{
    [Serializable]
    public class SkillElementData
    {
        public SkillElementType elementType;
        public Color color;
        public Sprite mainViewSprite;
        public Sprite starViewSprite;
        public List<SkillElementDamageByRareType> damageList;

        public int GetDamageByRareType(SkillRareType rareType)
        {
            return damageList.FirstOrDefault(damageData => damageData.rareType == rareType).damage;
        }
    }
}