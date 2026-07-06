using System;
using UnityEngine;

namespace SkillsArena
{
    public abstract class LevelManager : MonoBehaviour
    {
        public Action<LevelManager, string, float> OnExitLevel;

        public abstract void Init();

        public abstract void StartLevel();
    }
}