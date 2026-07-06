using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SkillsArena
{
    public class SkillCombination : MonoBehaviour
    {
        public event Action<bool> OnCombinationFilled;
        public event Action OnSkillSet;
        public event Action OnSkillRemove;

        public bool CanPlace => _skillPlaces.Exists(place => place.CurrentActiveSkill == null);
        public int TotalDamage { get; private set; }
        public List<SkillBallForBattle> CurrentSkills => _currentSkills;
        public Transform SkillBallsParent => _skillBallsParent;

        [SerializeField] private TextMeshPro _totalDamageText;
        [SerializeField] private List<SkillPlace> _skillPlaces;
        [SerializeField] private Transform _skillBallsParent;

        private List<SkillBallForBattle> _currentSkills = new List<SkillBallForBattle>();
        private DependencySkillsManager _dependencySkillsManager;
        private LevelData _levelData;

        public void Init(DependencySkillsManager dependencySkillsManager, LevelData levelData)
        {
            _levelData = levelData;
            foreach (SkillPlace skillPlace in _skillPlaces)
            {
                skillPlace.OnSkillSet += AfterSkillSet;
                skillPlace.OnNeedSkillRemove += AfterSkillRemove;
            }
            UpdateDamageView();
            _dependencySkillsManager = dependencySkillsManager;
        }

        public void Clear()
        {
            foreach (SkillPlace skillPlace in _skillPlaces)
            {
                skillPlace.Clear();
            }
            foreach (SkillBallForBattle skillBall in _currentSkills)
            {
                skillBall.DeathRattle();
            }
            _currentSkills.Clear();
            UpdateDamageView();
        }

        public void SetSkillToFirstFreePlace(SkillBallForBattle skillBall, bool needSmoothMove = true)
        {
            foreach (SkillPlace skillPlace in _skillPlaces)
            {
                if (skillPlace.CurrentActiveSkill == null)
                {
                    skillBall.SetToPlace(skillPlace, needSmoothMove);
                    skillPlace.SetSkill(skillBall);
                    return;
                }
            }
        }

        private void UpdateDamageView()
        {
            float totalDamage = 0;
            bool previousBallWasPaired = false;

            for (int i = 0; i < _skillPlaces.Count; i++)
            {
                SkillBallForBattle firstBall = _skillPlaces[i].CurrentActiveSkill;

                if (firstBall == null)
                {
                    previousBallWasPaired = false;
                    continue;
                }

                int firstDamage = GetBallDamage(firstBall);

                SkillBallForBattle secondBall = i < _skillPlaces.Count - 1 ? _skillPlaces[i + 1].CurrentActiveSkill : null;

                if (secondBall != null)
                {
                    int secondDamage = GetBallDamage(secondBall);

                    ElementPair pair = new(
                        firstBall.Skill.SkillElementData.elementType,
                        secondBall.Skill.SkillElementData.elementType);

                    float ratio = _dependencySkillsManager.GetRatioForElementPair(pair, _levelData.CurrentRound);

                    totalDamage += (firstDamage + secondDamage) * ratio;
                    previousBallWasPaired = true;
                }
                else
                {
                    if (!previousBallWasPaired)
                        totalDamage += firstDamage;

                    previousBallWasPaired = false;
                }
            }

            TotalDamage = (int)totalDamage;
            _totalDamageText.text = TotalDamage.ToString();
            _totalDamageText.enabled = _currentSkills.Count > 0;
        }

        private static int GetBallDamage(SkillBallForBattle ball)
        {
            return ball.Skill.SkillElementData.GetDamageByRareType(
                ball.Skill.SkillRareData.skillRareType);
        }

        private void AfterSkillSet(SkillBallForBattle skillBall)
        {
            OnSkillSet?.Invoke();
            _currentSkills.Add(skillBall);
            UpdateDamageView();
            if (!CanPlace)
                OnCombinationFilled?.Invoke(true);
        }

        private void AfterSkillRemove(SkillPlace skillPlace)
        {
            OnSkillRemove?.Invoke();
            _currentSkills.Remove(skillPlace.CurrentActiveSkill);
            skillPlace.Clear();
            UpdateDamageView();
            OnCombinationFilled?.Invoke(false);
        }
    }
}