using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkillsArena
{
    public class HandSkillsManager_Collector : MonoBehaviour
    {
        public event Action ExcessWasRemoved;

        [SerializeField] private float _minDistance;
        [SerializeField] private Transform _handSkillsParent;
        [SerializeField] private Transform _particlesParent;

        private GameData _gameData;
        private GameFactory _gameFactory;
        private InputService _inputService;
        private List<SkillBallForBattle> _handSkillsList = new List<SkillBallForBattle>();
        private SkillBallForBattle _currentActiveSkillBall;
        private CollectorLevel_UI_Manager _collectorLevelUIManager;
        private bool _needRemoveExcess;

        private List<Skill> _decodedList;

        public void Init(CollectorLevel_UI_Manager collectorLevelUIManager)
        {
            _collectorLevelUIManager = collectorLevelUIManager;
            _gameData = ServiceLocator.Instance.GetService<GameData>();
            _gameFactory = ServiceLocator.Instance.GetService<GameFactory>();
            _inputService = ServiceLocator.Instance.GetService<InputService>();
        }

        private void Update()
        {
            CheckInput();
        }

        public void SpawnCollectedSkills(List<Skill> decodedList)
        {
            _decodedList = decodedList;
            foreach (Skill skill in _decodedList)
            {
                SkillBallForBattle skillBall = _gameFactory.GetSkillBallForBattle(_handSkillsParent);
                skillBall.Init(skill, SkillBallForBattleType.Player);
                _handSkillsList.Add(skillBall);
            }
            SetPoses();
        }

        private void SetPoses()
        {
            List<Vector2> spawnPoints = GetSpawnPoints(_handSkillsList.Count);
            for (int i = 0; i < _handSkillsList.Count; i++)
            {
                _handSkillsList[i].SetTargetPosition(spawnPoints[i]);
            }
        }

        public void NeedRemoveExcess(Action callback)
        {
            _needRemoveExcess = true;
            ExcessWasRemoved += callback;
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

        private void CheckInput()
        {
            if (_inputService == null)
            {
                return;
            }
            
            Vector2 currentInputPos = _inputService.GetInputPosition();
            if (_inputService.LeftMouseOrSameWasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(currentInputPos, Vector2.zero);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.TryGetComponent<SkillBallForBattle>(out SkillBallForBattle skillBall))
                    {
                        SkillTaked(skillBall);
                        break;
                    }
                }
            }

            if (_inputService.LeftMouseOrSameWasReleasedThisFrame())
            {
                if (_currentActiveSkillBall != null)
                {
                    if (_needRemoveExcess)
                    {
                        AfterSkillBallForBattleDeath(_currentActiveSkillBall);
                        _collectorLevelUIManager.UpdateProgressBar(_decodedList.Count);
                        SetPoses();
                        if (_handSkillsList.Count == 9)
                        {
                            _needRemoveExcess = false;
                            ExcessWasRemoved?.Invoke();
                        }
                    }
                    else
                    {
                        _currentActiveSkillBall.SetTargetPosition(_currentActiveSkillBall.TargetPosition);
                    }
                    _currentActiveSkillBall = null;
                }
            }

            if (_currentActiveSkillBall != null)
            {
                _currentActiveSkillBall.transform.position = currentInputPos;
            }
        }

        private void SkillTaked(SkillBallForBattle skillBall)
        {
            AudioManager.Instance.PlaySomeSound(SoundType.TakeSkill);
            _currentActiveSkillBall = skillBall;
            _currentActiveSkillBall.SetActive(true);
            _currentActiveSkillBall.TryStopMove();
        }

        private void AfterSkillBallForBattleDeath(SkillBallForBattle skillBallForBattle)
        {
            GameObject deathBallParticle = _gameFactory.GetDeathBallParticle(_particlesParent);
            deathBallParticle.transform.position = skillBallForBattle.transform.position;
            _handSkillsList.Remove(skillBallForBattle);
            _decodedList.Remove(_currentActiveSkillBall.Skill);
            skillBallForBattle.DeathRattle();
        }

        internal void PrepareToAnim()
        {
            foreach(var skillBall in _handSkillsList)
            {
                skillBall.SetTargetPosition(skillBall.TargetPosition, needSmoothMove: false);
            }
        }
    }
}