using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace SkillsArena
{
    public class SkillBallForBattle : MonoBehaviour
    {
        public SpriteRenderer rareSprite;
        public SpriteRenderer elementSprite;
        public SpriteRenderer viewSprite;
        public SortingGroup sortingGroup;

        public Vector2 TargetPosition { get; private set; }
        public Skill Skill { get; private set; }
        public SkillStateType SkillStateType { get; private set; }
        public SkillPlace TargetSkillPlace { get; private set; }
        public SkillBallForBattleType SkillBallForBattleType { get; private set; }

        private Coroutine _moveToTargetPlaceCoroutine;
        private int _startSortingGroup;

        private void Awake()
        {
            _startSortingGroup = sortingGroup.sortingOrder;
        }

        public void Init(Skill skill, SkillBallForBattleType skillBallForBattleType)
        {
            Skill = skill;
            rareSprite.sprite = skill.SkillRareData.sprite;
            elementSprite.sprite = skill.SkillElementData.starViewSprite;
            viewSprite.sprite = skill.SkillElementData.mainViewSprite;
            SkillStateType = SkillStateType.InHand;
            SkillBallForBattleType = skillBallForBattleType;
        }

        public void SetActive(bool active)
        {
            sortingGroup.sortingOrder = active ? _startSortingGroup + 10 : _startSortingGroup;
        }

        public void SetToPlace(SkillPlace skillPlace, bool needSmoothMove = true)
        {
            TargetSkillPlace = skillPlace;
            SetTargetPosition(TargetSkillPlace.transform.position, needSmoothMove);
            SkillStateType = SkillStateType.OnPlace;
        }

        public void RemovePlace()
        {
            TargetSkillPlace = null;
            SkillStateType = SkillStateType.InHand;
        }

        public void TryStopMove()
        {
            if (_moveToTargetPlaceCoroutine != null)
            {
                StopCoroutine(_moveToTargetPlaceCoroutine);
            }
        }

        public void SetTargetPosition(Vector2 targetPosition, bool needSmoothMove = true)
        {
            TargetPosition = targetPosition;
            if (needSmoothMove)
            {
                if (_moveToTargetPlaceCoroutine != null)
                    StopCoroutine(_moveToTargetPlaceCoroutine);
                _moveToTargetPlaceCoroutine = StartCoroutine(MoveToTargetPlace(targetPosition));
            }
            else
            {
                if (_moveToTargetPlaceCoroutine != null)
                    StopCoroutine(_moveToTargetPlaceCoroutine);
                transform.position = targetPosition;
                SetActive(false);
            }
        }

        public void DeathRattle()
        {
            Destroy(gameObject);
        }

        private IEnumerator MoveToTargetPlace(Vector2 targetPosition)
        {
            float timeMove = 0.5f;
            float currentPath = 0;
            Vector3 startPosition = transform.position;
            while (currentPath < 1)
            {
                currentPath += Time.deltaTime / timeMove;
                transform.position = Vector3.Lerp(startPosition, targetPosition, currentPath);
                yield return null;
            }
            SetActive(false);
        }
    }

    public enum SkillBallForBattleType
    {
        Player, Enemy
    }
}
