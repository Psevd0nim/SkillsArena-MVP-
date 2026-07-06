using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class SoundConfig : ScriptableObject
    {
        public List<SoundData> sounds;

        public SoundData GetSoundDataByType(SoundType type)
        {
            foreach (var sound in sounds)
            {
                if (sound.type == type)
                    return sound;
            }
            throw new Exception($"Can't return SoundData by Type: {type}");
        }
    }

    [Serializable]
    public class SoundData
    {
        public SoundType type;
        public AudioClip sound;
        [Range(0, 1)] public float volume = 0.5f;
    }

    public enum SoundType
    {
        ClickButton = 0, 
        Tutorial = 1, 
        TakeSkill = 2, 
        SetSkill = 3, 
        CaptureSkill = 4, 
        Fight = 5, 
        TakeDamage = 6,
        FullCollected = 7,
        GameOver = 8
    }
}