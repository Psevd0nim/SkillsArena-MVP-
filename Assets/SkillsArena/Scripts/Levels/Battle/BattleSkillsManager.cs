using UnityEngine;
using UnityEngine.EventSystems;

namespace SkillsArena
{
    public class BattleSkillsManager : MonoBehaviour, IPausable
    {
        [SerializeField] private HandSkillsManager_Battle _handSkillsBattleManager;
        [SerializeField] private SkillCombination _playerComboSkill;
        [SerializeField] private SkillElementConfig _skillElementConfig;
        [SerializeField] private SkillRareConfig _skillRareConfig;

        private SkillCombination _enemyComboSkill;
        private SkillBallForBattle _currentActiveSkillBall;
        private InputService _inputService;
        private DependencySkillsManager _dependencySkillsManager;
        private GameFactory _gameFactory;
        private GameData _gameData;
        private Enemy _enemy;
        private LevelData _levelData;
        private bool _isPaused;

        public void Init(DependencySkillsManager dependencySkillsManager, Player player, Enemy enemy, LevelData levelData)
        {
            _inputService = ServiceLocator.Instance.GetService<InputService>();
            _dependencySkillsManager = dependencySkillsManager;
            _gameFactory = ServiceLocator.Instance.GetService<GameFactory>();
            _gameData = ServiceLocator.Instance.GetService<GameData>();
            _levelData = levelData;

            _playerComboSkill = player.SkillCombination;
            _playerComboSkill.Init(_dependencySkillsManager, _levelData);
            Init(enemy);
        }

        public void Init(Enemy enemy)
        {
            _enemy = enemy;
            _enemyComboSkill = enemy.SKillCombination;
            _enemyComboSkill.Init(_dependencySkillsManager, _levelData);
        }

        private void Update()
        {
            if (!_isPaused)
                CheckInput();
        }

        public void Clear()
        {
            foreach (var skillBall in _playerComboSkill.CurrentSkills)
            {
                _gameData.RemoveCollectedSkill(skillBall.Skill.SkillData);
            }
            _playerComboSkill.Clear();
            _enemyComboSkill.Clear();
            _enemy.ClearSkillCombinationData();
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
                if (TryGetSkillBallAtPosition(currentInputPos, out SkillBallForBattle skillBall))
                {
                    if (skillBall.SkillBallForBattleType != SkillBallForBattleType.Enemy)
                    {
                        SkillTaked(skillBall);
                    }
                }
            }

            if (_inputService.LeftMouseOrSameWasReleasedThisFrame())
            {
                if (_currentActiveSkillBall != null)
                {
                    bool inputOnSkillPlace = TryGetSkillPlaceAtPosition(currentInputPos, out SkillPlace skillPlace);
                    bool inputOnHandSkillsManager = TryGetHandAtPosition(currentInputPos, out HandSkillsManager_Battle handSkillsBattleManager);
                    switch (_currentActiveSkillBall.SkillStateType)
                    {
                        case SkillStateType.InHand:
                            if (inputOnSkillPlace && skillPlace.CurrentActiveSkill == null)
                            {
                                _currentActiveSkillBall.SetToPlace(skillPlace);
                                skillPlace.SetSkill(_currentActiveSkillBall);
                                _handSkillsBattleManager.RemoveSkillFromHand(_currentActiveSkillBall);
                            }
                            else if (!inputOnHandSkillsManager && _playerComboSkill.CanPlace)
                            {
                                _playerComboSkill.SetSkillToFirstFreePlace(_currentActiveSkillBall);
                                _handSkillsBattleManager.RemoveSkillFromHand(_currentActiveSkillBall);
                            }
                            break;
                        case SkillStateType.OnPlace:
                            if (inputOnHandSkillsManager)
                            {
                                _handSkillsBattleManager.AddSkillToHand(_currentActiveSkillBall);
                                _currentActiveSkillBall.TargetSkillPlace.RemoveSkill();
                                _currentActiveSkillBall.RemovePlace();
                            }
                            else if (inputOnSkillPlace && skillPlace.CurrentActiveSkill == null)
                            {
                                _currentActiveSkillBall.TargetSkillPlace.RemoveSkill();
                                _currentActiveSkillBall.RemovePlace();
                                
                                _currentActiveSkillBall.SetToPlace(skillPlace);
                                skillPlace.SetSkill(_currentActiveSkillBall);
                            }
                            break;
                    }
                    _currentActiveSkillBall.SetTargetPosition(_currentActiveSkillBall.TargetPosition);
                    _currentActiveSkillBall = null;
                    _handSkillsBattleManager.SetHandViewActive(false);
                }
            }

            TryMoveActiveBall(currentInputPos);
        }

        private void SkillTaked(SkillBallForBattle skillBall)
        {
            AudioManager.Instance.PlaySomeSound(SoundType.TakeSkill);
            _currentActiveSkillBall = skillBall;
            _currentActiveSkillBall.SetActive(true);
            _currentActiveSkillBall.TryStopMove();
            _handSkillsBattleManager.SetHandViewActive(true);
        }

        private void TryMoveActiveBall(Vector2 currentInputPos)
        {
            if (_currentActiveSkillBall != null)
            {
                _currentActiveSkillBall.transform.position = currentInputPos;
            }
        }

        private bool TryGetHandAtPosition(Vector2 position, out HandSkillsManager_Battle handSkillsBattleManager)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<HandSkillsManager_Battle>(out HandSkillsManager_Battle hand))
                {
                    handSkillsBattleManager = hand;
                    return true;
                }
            }
            handSkillsBattleManager = null;
            return false;
        }

        private bool TryGetSkillBallAtPosition(Vector2 position, out SkillBallForBattle outSkillBall)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<SkillBallForBattle>(out SkillBallForBattle skillBall))
                {
                    outSkillBall = skillBall;
                    return true;
                }
            }
            outSkillBall = null;
            return false;
        }

        private bool TryGetSkillPlaceAtPosition(Vector2 position, out SkillPlace outskillPlace)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent<SkillPlace>(out SkillPlace skillPlace))
                {
                    outskillPlace = skillPlace;
                    return true;
                }
            }
            outskillPlace = null;
            return false;
        }

        public void TrySetSkillsToEnemyCombination()
        {
            if (_enemy.SkillCombinationData.skills.Count == 0)
            {
                SetRandomSkillsToEnemyCombination();
                SaveAndLoadData saveAndLoadData = ServiceLocator.Instance.GetService<SaveAndLoadData>();
                saveAndLoadData.SaveGameData();
            }
            else
            {
                foreach (var skillData in _enemy.SkillCombinationData.skills)
                {
                    SkillRareData skillRareData = _skillRareConfig.GetSkillRareDataByType(skillData.skillRareType);
                    SkillElementData skillElementData = _skillElementConfig.GetSkillElementDataByType(skillData.skillElementType);
                    Skill skill = new Skill(skillRareData, skillElementData, skillData);
                    SkillBallForBattle skillBallForBattle = _gameFactory.GetSkillBallForBattle(_enemyComboSkill.SkillBallsParent);
                    skillBallForBattle.Init(skill, SkillBallForBattleType.Enemy);
                    _enemyComboSkill.SetSkillToFirstFreePlace(skillBallForBattle, needSmoothMove: false);
                }
            }
        }

        private void SetRandomSkillsToEnemyCombination()
        {
            SkillCombinationData skillCombinationData = new SkillCombinationData();
            for (int i = 0; i < 3; i++)
            {
                SkillRareData skillRareData = _skillRareConfig.GetSkillRareDataByType(_enemy.EnemySkillsRateData.GetRandomSkillRare());
                SkillElementData skillElementData = _skillElementConfig.GetRandomElement();
                SkillData skillData = new SkillData(skillRareData.skillRareType, skillElementData.elementType);

                skillCombinationData.skills.Add(skillData);

                Skill skill = new Skill(skillRareData, skillElementData, skillData);
                SkillBallForBattle skillBallForBattle = _gameFactory.GetSkillBallForBattle(_enemyComboSkill.SkillBallsParent);
                skillBallForBattle.Init(skill, SkillBallForBattleType.Enemy);
                _enemyComboSkill.SetSkillToFirstFreePlace(skillBallForBattle, needSmoothMove: false);
            }
            _enemy.UpdateSkillCombinationData(skillCombinationData);
        }

        public void Pause(bool pause)
        {
            _isPaused = pause;
        }
    }
}