using System;
using UnityEngine;

namespace SkillsArena
{
    public abstract class Entity : MonoBehaviour
    {
        public event Action OnDeath; 

        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private HealthBar_UI _healthBarUI;
        [SerializeField] private SpriteRenderer _view;

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public void Init(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            UpdateHealthView();
            _view.enabled = true;
        }

        public abstract void Save();

        public void StartAnimation(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Attack:
                    _playerAnimator.SetTrigger("Attack");
                    break;
                case AnimationType.Idle:
                    _playerAnimator.SetTrigger("Idle");
                    break;
                case AnimationType.Damage:
                    _playerAnimator.SetTrigger("Damage");
                    break;
            }
        }

        public void UpdateHealthView()
        {
            _healthBarUI.UpdateHealthView(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            AudioManager.Instance.PlaySomeSound(SoundType.TakeDamage);
            CurrentHealth -= damage;
            Save();
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                DeathRattle();
            }
            else
            {
                StartAnimation(AnimationType.Damage);
            }
            UpdateHealthView();
        }

        private protected virtual void DeathRattle()
        {
            _view.enabled = false;
            OnDeath?.Invoke();
        }
    }
}
