using System;
using System.Collections.Generic;

namespace SkillsArena
{
    [Serializable]
    public class DependencySkillsData
    {
        public Dictionary<int, DependencySkillsOnRound> dependeciesDictionary { get; private set; } = new();

        public void AddDependencySkillsOnRound(int round, DependencySkillsOnRound dependenciesOnRound)
        {
            dependeciesDictionary[round] = dependenciesOnRound;
        }

        public DependencySkillsOnRound GetDependencySkillsOnRound(int round)
        {
            return dependeciesDictionary[round];
        }
    }
}