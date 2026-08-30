using System;
using UnityEngine;

namespace SkillsArena
{
    public abstract class Entity : MonoBehaviour
    {
        public event Action OnDeath;
        public event Action OnDeathAnimEnd;
        public event Action OnReady;

        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private protected EntityView_UI _entityView_UI;
        [SerializeField] private protected SpriteRenderer _view;

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsReady { get; private set; }

        public void Init(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            UpdateHealthView();
            _view.enabled = true;
            _playerAnimator.ResetControllerState();
            _playerAnimator.SetBool("Death", false);
            IsReady = true;
            OnReady?.Invoke();
        }

        public abstract void Save();

        public void StartAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Attack:
                    _playerAnimator.SetTrigger("Attack");
                    break;
                case AnimationType.Damage:
                    _playerAnimator.SetTrigger("Damage");
                    break;
            }
        }

        public void UpdateHealthView()
        {
            _entityView_UI.UpdateHealthView(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            StartAnimation(AnimationType.Damage);
            CurrentHealth -= damage;
            Save();
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                DeathRattle();
            }
            //UpdateHealthView();
        }

        public void TakeDamageSound()
        {
            AudioManager.Instance.PlaySomeSound(SoundType.TakeDamage);
        }

        private protected virtual void DeathRattle()
        {
            IsReady = false;
            _playerAnimator.SetBool("Death", true);
            OnDeath?.Invoke();
        }

        private protected void DeathAnimEnd()
        {
            _view.enabled = false;
            OnDeathAnimEnd?.Invoke();
        }
    }
}
