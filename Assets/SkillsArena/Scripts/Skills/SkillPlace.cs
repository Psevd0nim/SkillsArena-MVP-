using System;
using UnityEngine;

namespace SkillsArena
{
    public class SkillPlace : MonoBehaviour
    {
        public event Action<SkillBallForBattle> OnSkillSet;
        public event Action<SkillPlace> OnNeedSkillRemove;

        public SkillBallForBattle CurrentActiveSkill { get; private set; }

        public void SetSkill(SkillBallForBattle skillBall)
        {
            CurrentActiveSkill = skillBall;
            OnSkillSet?.Invoke(skillBall);
        }

        public void RemoveSkill()
        {
            OnNeedSkillRemove?.Invoke(this);
        }

        public void Clear()
        {
            CurrentActiveSkill = null;
        }
    }
}