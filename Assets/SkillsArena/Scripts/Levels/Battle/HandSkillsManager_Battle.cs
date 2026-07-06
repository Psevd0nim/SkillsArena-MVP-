using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    public class HandSkillsManager_Battle : MonoBehaviour
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private Transform _handSkillsParent;
        [SerializeField] private Transform _particlesParent;
        [SerializeField] private GameObject _handView;
        [SerializeField] private SkillElementConfig _skillElementConfig;
        [SerializeField] private SkillRareConfig _skillRareConfig;

        private GameData _gameData;
        private GameFactory _gameFactory;
        private List<SkillBallForBattle> _handSkillsList = new List<SkillBallForBattle>();

        void Awake()
        {
            SetHandViewActive(false);
        }

        public void Init()
        {
            _gameData = ServiceLocator.Instance.GetService<GameData>();
            _gameFactory = ServiceLocator.Instance.GetService<GameFactory>();
        }

        public void TrySpawnSkills()
        {
            List<Skill> skills = new();
            foreach (SkillData skillData in _gameData.CollectedSkillsList)
            {
                SkillRareData skillRareData = _skillRareConfig.GetSkillRareDataByType(skillData.skillRareType);
                SkillElementData skillElementData = _skillElementConfig.GetSkillElementDataByType(skillData.skillElementType);
                skills.Add(new Skill(skillRareData, skillElementData, skillData));
            }
            foreach (Skill skill in skills)
            {
                SkillBallForBattle skillBall = _gameFactory.GetSkillBallForBattle(_handSkillsParent);
                skillBall.Init(skill, SkillBallForBattleType.Player);
                _handSkillsList.Add(skillBall);
            }
            SetPoses(false);
        }

        public void RemoveSkillFromHand(SkillBallForBattle skillBall)
        {
            _handSkillsList.Remove(skillBall);
            SetPoses();
        }

        public void AddSkillToHand(SkillBallForBattle skillBall)
        {
            List<SkillBallForBattle> tempList = new List<SkillBallForBattle>(_handSkillsList);
            _handSkillsList.Clear();
            _handSkillsList.Add(skillBall);
            _handSkillsList.AddRange(tempList);
            SetPoses();
        }

        public void SetHandViewActive(bool active)
        {
            _handView.SetActive(active);
        }

        private void SetPoses(bool smoothMove = true)
        {
            List<Vector2> spawnPoints = GetSpawnPoints(_handSkillsList.Count);
            for (int i = 0; i < _handSkillsList.Count; i++)
            {
                _handSkillsList[i].SetTargetPosition(spawnPoints[i], smoothMove);
            }
        }

        private List<Vector2> GetSpawnPoints(int count)
        {
            List<Vector2> spawnPoints = new List<Vector2>();
            float distanceBetweenPoints = _minDistance;
            float startX = _handSkillsParent.position.x - (distanceBetweenPoints * (count - 1) / 2);
            for (int i = 0; i < count; i++)
            {
                spawnPoints.Add(new Vector2(startX + i * distanceBetweenPoints, _handSkillsParent.position.y));
            }
            return spawnPoints;
        }
    }
}