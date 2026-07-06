using System;
using UnityEditor;
using UnityEngine;

namespace SkillsArena
{
    [CustomEditor(typeof(SkillElementConfig))]
    public class SkillElementConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SkillElementConfig skillElementConfig = (SkillElementConfig)target;
            if (skillElementConfig.wantSetDefaultValue)
            {
                if (GUILayout.Button("Set Default Value"))
                {
                    foreach (var skillElement in skillElementConfig.skillElementsList)
                    {
                        skillElement.damageList.Clear();
                        int count = Enum.GetNames(typeof(SkillRareType)).Length;
                        for (int index = 0; index < count; index++)
                        {
                            SkillElementDamageByRareType damageByRareType = new SkillElementDamageByRareType();
                            damageByRareType.rareType = (SkillRareType)index;
                            int currentMultiplyFactor = 1 + skillElementConfig.multiplyFactor * index;
                            damageByRareType.damage = skillElementConfig.firstDamage * currentMultiplyFactor;
                            skillElement.damageList.Add(damageByRareType);
                        }
                    }
                }
            }
        }
    }
}