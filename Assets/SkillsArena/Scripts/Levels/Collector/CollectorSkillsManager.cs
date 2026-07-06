using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace SkillsArena
{
    public class CollectorSkillsManager : MonoBehaviour, IPausable
    {
        public event Action OnAllSkillsCollected;

        [SerializeField] private CollectorLevelConfig _collectorLevelConfig;
        [SerializeField] private SkillRareConfig _skillRareConfig;
        [SerializeField] private SkillElementConfig _skillElementConfig;
        [SerializeField] private List<SandClock> _sandClocksList = new List<SandClock>();
        [SerializeField] private Transform _skillsCounter;

        [Header("Parents")]
        [SerializeField] private Transform _skillBallsParent;
        [SerializeField] private Transform _particlesParent;

        private List<SkillBallForCollector> _skillBallsList = new List<SkillBallForCollector>();
        private List<SkillBallForCollector> _skillBallsDecoded = new List<SkillBallForCollector>();
        private List<Skill> _collectedSkillsList = new();
        private bool _isEndStage;

        private GameFactory _gameFactory;
        private GameData _gameData;
        private InputService _inputService;
        private BoundaryManager _boundaryManager;
        private CollectorLevel_UI_Manager _collectorLevelUIManager;
        private HandSkillsManager_Collector _handSkillsCollectorManager;
        private bool _pause;
        private EncodedType _currentEncodedType = EncodedType.None;

        public void Init(GameFactory gameFactory, GameData gameData, InputService inputService,
        BoundaryManager boundaryManager, CollectorLevel_UI_Manager collectorLevelUIManager, HandSkillsManager_Collector handSkillsCollectorManager)
        {
            _gameFactory = gameFactory;
            _gameData = gameData;
            _inputService = inputService;
            _boundaryManager = boundaryManager;
            _collectorLevelUIManager = collectorLevelUIManager;
            _handSkillsCollectorManager = handSkillsCollectorManager;
        }

        private void Update()
        {
            if (!_isEndStage)
            {
                CheckInput();
                CheckDecodedAndClear();
            }
        }

        public void LevelLoop()
        {
            if (_currentEncodedType == EncodedType.None)
            {
                int rndNum = Random.Range(0, 2);
                switch (rndNum)
                {
                    case 0:
                        _currentEncodedType = EncodedType.Touch;
                        break;
                    case 1:
                        _currentEncodedType = EncodedType.Direction;
                        break;
                }
            }
            else
            {
                _currentEncodedType = _currentEncodedType == EncodedType.Touch ? EncodedType.Direction : EncodedType.Touch;
            }
            FillLevel();
        }

        private void FillLevel()
        {
            for (int i = 0; i < _collectorLevelConfig.skillsInCollectorLevel.Count; i++)
            {
                CollectorBallData collectorBallData = _collectorLevelConfig.skillsInCollectorLevel[i];
                SkillRareData skillRareData = _skillRareConfig.GetSkillRareDataByType(collectorBallData.skillRareType);
                CreateCollectorSkillBalls(collectorBallData.count, skillRareData, collectorBallData);
                SandClock sandClock = _sandClocksList[i];
                sandClock.Init(new SandClockData(skillRareData.skillRareType, skillRareData.color, collectorBallData.timeLive));
                sandClock.OnTimeEnd += AfterSandClockEnded;
                sandClock.OnLowTime += AfterSandClockLowTime;
            }
        }

        private void AfterSandClockLowTime(SandClock sandClock)
        {
            sandClock.OnLowTime -= AfterSandClockLowTime;
            List<SkillBallForCollector> skillsList = _skillBallsList.Where(x => x.SkillRareType == sandClock.RareType).ToList();
            foreach (var skillBall in skillsList)
                skillBall.StartBlinkAnim();
        }

        private void AfterSandClockEnded(SandClock sandClock)
        {
            sandClock.OnTimeEnd -= AfterSandClockEnded;
            List<SkillBallForCollector> skillsForRemove = _skillBallsList.Where(x => x.SkillRareType == sandClock.RareType).ToList();
            foreach (var skillBall in skillsForRemove)
            {
                _skillBallsList.Remove(skillBall);
                skillBall.DeathRattle();
            }
            CheckForNextLoop();
        }

        private void CheckForNextLoop()
        {
            if (_collectedSkillsList.Count < 9 && _skillBallsList.Count == 0)
                LevelLoop();
        }

        private void CreateCollectorSkillBalls(int count, SkillRareData skillBallData, CollectorBallData collectorBallData)
        {
            for (int i = 0; i < count; i++)
            {
                CreateCollectorSkillBall(skillBallData, collectorBallData);
            }
        }

        private void CreateCollectorSkillBall(SkillRareData skillRareData, CollectorBallData collectorBallData)
        {
            SkillBallForCollector skillBall = _gameFactory.GetSkillBallForCollector();
            skillBall.transform.parent = _skillBallsParent;
            SkillElementData skillElementData = _skillElementConfig.GetRandomElement();
            Skill skill = new Skill(skillRareData, skillElementData, new SkillData(skillRareData.skillRareType, skillElementData.elementType));
            skillBall.Init(skill, collectorBallData, _particlesParent, _currentEncodedType);
            skillBall.transform.position = _boundaryManager.GetRandomPositionInsideBoundary();
            _skillBallsList.Add(skillBall);
            skillBall.OnDecoded += AfterSkillDecoded;
        }

        private void AfterSkillDecoded(SkillBallForCollector skillBall)
        {
            _skillBallsDecoded.Add(skillBall);
        }

        private void CheckInput()
        {
            if (_pause || _inputService == null)
                return;

            switch (_currentEncodedType)
            {
                case EncodedType.Direction:
                    {
                        InputLikeKeyboardType inputType = _inputService.GetCurrentKeyWasReleasedThisFrame();
                        if (inputType != InputLikeKeyboardType.Left && inputType != InputLikeKeyboardType.Right && inputType != InputLikeKeyboardType.Up && inputType != InputLikeKeyboardType.Down)
                            inputType = InputLikeKeyboardType.None;

                        if (inputType != InputLikeKeyboardType.None)
                        {
                            foreach (var skillBall in _skillBallsList)
                            {
                                skillBall.Input(inputType);
                            }
                        }
                    }
                    break;
                case EncodedType.Touch:
                    Vector2 currentInputPos = _inputService.GetInputPosition();
                    if (_inputService.LeftMouseOrSameWasPressedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
                    {
                        RaycastHit2D[] hits = Physics2D.RaycastAll(currentInputPos, Vector2.zero);
                        foreach (RaycastHit2D hit in hits)
                        {
                            if (hit.collider.TryGetComponent<SkillBallForCollector>(out SkillBallForCollector skillBall))
                            {
                                skillBall.Input();
                                break;
                            }
                        }
                    }
                    break;
            }

        }

        private void CheckDecodedAndClear()
        {
            if (_skillBallsDecoded.Count > 0)
            {
                foreach (var skillBall in _skillBallsDecoded)
                {
                    AudioManager.Instance.PlaySomeSound(SoundType.FullCollected);
                    _collectedSkillsList.Add(skillBall.Skill);
                    _collectorLevelUIManager.UpdateProgressBar(_collectedSkillsList.Count);
                    _skillBallsList.Remove(skillBall);
                    skillBall.OnAnimEnded += AfterAnimEnded;
                }
                _skillBallsDecoded.Clear();
                CheckForNextLoop();
            }

            CheckForEnd();
        }

        private void CheckForEnd()
        {
            if (_collectedSkillsList.Count >= 9)
            {
                StartEndStage();
            }
        }

        private void AfterAnimEnded(SkillBallForCollector skillBallForCollector)
        {
            skillBallForCollector.DeathRattle(_skillsCounter.transform);
        }

        private void StartEndStage()
        {
            _isEndStage = true;
            foreach (var skillBall in _skillBallsList)
            {
                skillBall.DeathRattle();
            }
            _skillBallsList.Clear();
            _handSkillsCollectorManager.SpawnCollectedSkills(_collectedSkillsList);
            if (_collectedSkillsList.Count > 9)
            {
                _collectorLevelUIManager.ShowNotFinalPanel();
                _handSkillsCollectorManager.NeedRemoveExcess(AfterAllCollected);
            }
            else
                AfterAllCollected();
        }

        private void AfterAllCollected()
        {
            _gameData.CollectedSkillsList.Clear();
            foreach (var skill in _collectedSkillsList)
            {
                _gameData.AddCollectedSkill(skill.SkillData);
            }
            _collectorLevelUIManager.ShowFinalPanel();
            OnAllSkillsCollected?.Invoke();
        }

        public void Pause(bool pause)
        {
            _pause = pause;
        }
    }
}
