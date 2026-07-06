using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkillsArena
{
    public class SkillBallForCollector : MonoBehaviour
    {
        public event Action<SkillBallForCollector> OnDecoded;
        public event Action<SkillBallForCollector> OnAnimEnded;

        public float speed;
        public Rigidbody2D rb;
        public SpriteRenderer viewSprite;
        public SpriteRenderer rareSprite;
        public SpriteRenderer elementSprite;
        public Animator skillBallAnimator;
        public GameObject deathParticle;
        public List<DirectionArrow> directionArrowsList;
        public GameObject directionOutline, touchOutline;

        public Skill Skill { get; private set; }
        public SkillRareType SkillRareType { get; private set; }
        public SkillElementData SkillElementData { get; private set; }
        public SkillRareData SkillRareData { get; private set; }
        public CollectorBallData CollectorBallData { get; private set; }

        private EncodedData _currentEncodedData;
        private Queue<InputLikeKeyboardType> _encodedDirectionQueue = new Queue<InputLikeKeyboardType>();
        private InputLikeKeyboardType _currentTypeFromEncode;
        private DirectionArrow _currentDirectionArrow;
        private int _decodedCount;

        private MaterialPropertyBlock _materialPropertyBlock;
        private Transform _particlesParent;

        private Vector2 _startScale;
        private float _currentTime;
        private float _rndTime;

        private Coroutine _appearCoroutine;

        void Awake()
        {
            directionOutline.SetActive(false);
            touchOutline.SetActive(false);
        }

        public void Init(Skill skill, CollectorBallData collectorBallData, Transform particlesParent, EncodedType encodedType)
        {
            Skill = skill;
            SkillRareData = Skill.SkillRareData;
            SkillElementData = Skill.SkillElementData;
            SkillRareType = SkillRareData.skillRareType;
            rareSprite.sprite = SkillRareData.sprite;
            elementSprite.sprite = SkillElementData.starViewSprite;
            viewSprite.sprite = SkillElementData.mainViewSprite;
            _particlesParent = particlesParent;

            CollectorBallData = collectorBallData;

            SetEncodedLogic(encodedType);
            InitProgressBar();
            RandomShot();
            _rndTime = Random.Range(0.75f, 1.25f);
            _appearCoroutine = StartCoroutine(AppearAnim());
        }

        private void Update()
        {
            if (_currentTime > _rndTime)
            {
                RandomShot();
                _currentTime = 0;
            }
            _currentTime += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (rb.linearVelocity.magnitude > speed)
            {
                rb.linearVelocity.Normalize();
                rb.linearVelocity *= speed / 2;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            RandomShot();
        }

        public void Input(InputLikeKeyboardType inputType)
        {
            if (gameObject.activeSelf == false)
                return;

            if (_currentEncodedData.encodedType == EncodedType.Direction)
            {
                DirectionArrow directionArrow = directionArrowsList.First(x => x.InputType == inputType);
                directionArrow.Pressed();
                if (_currentTypeFromEncode == inputType && _decodedCount < _currentEncodedData.encodedLength)
                {
                    //AudioManager.Instance.PlaySomeSound(SoundType.CaptureSkill);
                    _decodedCount++;
                    if (_encodedDirectionQueue.Count == 0)
                        WasDecoded();
                    else
                        SetCurrentInputType();
                    float endValue = _decodedCount == _currentEncodedData.encodedLength ? 1 : _decodedCount / (float)_currentEncodedData.encodedLength;
                    StartCoroutine(SmoothProgressBar(_materialPropertyBlock.GetFloat("_FillAmount"), endValue));
                }
            }
        }

        public void Input()
        {
            if (gameObject.activeSelf == false)
                return;

            if (_currentEncodedData.encodedType == EncodedType.Touch)
            {
                AudioManager.Instance.PlaySomeSound(SoundType.CaptureSkill);
                _decodedCount++;
                if (_decodedCount == _currentEncodedData.encodedLength)
                    WasDecoded();
                float endValue = _decodedCount == _currentEncodedData.encodedLength ? 1 : _decodedCount / (float)_currentEncodedData.encodedLength;
                StartCoroutine(SmoothProgressBar(_materialPropertyBlock.GetFloat("_FillAmount"), endValue));
            }
        }

        public void StartBlinkAnim()
        {
            skillBallAnimator.SetTrigger("Blink");
        }

        public void DeathRattle()
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity, _particlesParent);
            Destroy(gameObject);
        }

        public void DeathRattle(Transform target)
        {
            if (_appearCoroutine != null)
                StopCoroutine(_appearCoroutine);
            transform.localScale = _startScale / 4;
            StartCoroutine(MoveToTarget(target.position));
        }

        private IEnumerator MoveToTarget(Vector2 targetPosition)
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
            Destroy(gameObject);
        }

        private void RandomShot()
        {
            rb.AddForce(GetRndDirection() * speed, ForceMode2D.Impulse);
        }

        private void InitProgressBar()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            viewSprite.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetFloat("_FillAmount", 0);
            viewSprite.SetPropertyBlock(_materialPropertyBlock);
        }

        private void SetEncodedLogic(EncodedType encodedType)
        {
            EncodedData encodedData = CollectorBallData.encodeDataList.First(x => x.encodedType == encodedType);
            if (encodedData == null)
                throw new Exception($"Didn't created encodedData for encodedType{encodedType}");

            _currentEncodedData = encodedData;

            switch (_currentEncodedData.encodedType)
            {
                case EncodedType.Direction:
                    directionOutline.SetActive(true);
                    for (int i = 0; i < encodedData.encodedLength; i++)
                    {
                        InputLikeKeyboardType rndType = (InputLikeKeyboardType)Random.Range(1, 5);
                        _encodedDirectionQueue.Enqueue(rndType);
                    }
                    SetCurrentInputType();
                    break;
                case EncodedType.Touch:
                    touchOutline.SetActive(true);
                    break;
            }

        }

        private Vector2 GetRndDirection()
        {
            float x = Random.Range(-1, 1f);
            float y = Random.Range(-1, 1f);
            return new Vector2(x, y).normalized;
        }

        private void WasDecoded()
        {
            OnDecoded?.Invoke(this);
        }

        private void SetCurrentInputType()
        {
            _currentTypeFromEncode = _encodedDirectionQueue.Dequeue();
            _currentDirectionArrow?.SetActive(false);
            _currentDirectionArrow = directionArrowsList.First(x => x.InputType == _currentTypeFromEncode);
            _currentDirectionArrow.SetActive(true);
        }

        private IEnumerator SmoothProgressBar(float startValue, float endValue)
        {
            float currentValue = 0;
            float progressValue = 0;
            float currentTime = 0;
            float time = 0.2f;
            while (progressValue < 1)
            {
                progressValue = currentTime / time;
                currentValue = Mathf.Lerp(startValue, endValue, progressValue);
                _materialPropertyBlock.SetFloat("_FillAmount", currentValue);
                viewSprite.SetPropertyBlock(_materialPropertyBlock);
                currentTime += Time.deltaTime;
                yield return null;
            }

            if (endValue == 1)
            {
                OnAnimEnded?.Invoke(this);
            }
        }

        private IEnumerator AppearAnim()
        {
            _startScale = transform.localScale;
            transform.localScale = Vector2.zero;
            float totalTime = 1f;
            float currentPath = 0;
            Vector3 startScale = transform.localScale;
            while (currentPath < 1)
            {
                currentPath += Time.deltaTime / totalTime;
                transform.localScale = Vector3.Lerp(startScale, _startScale, currentPath);
                yield return null;
            }
        }
    }
}