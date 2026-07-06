using System;

namespace SkillsArena
{
    [Serializable]
    public class PlayerData
    {
        public int currentHealth;

        public PlayerData(int currentHealth)
        {
            this.currentHealth = currentHealth;
        }

        public PlayerData()
        {
            
        }
    }
}