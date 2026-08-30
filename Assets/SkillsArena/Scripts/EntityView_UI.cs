using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class EntityView_UI : MonoBehaviour
    {
        [SerializeField] private HealthBar_UI _healthBar_UI;
        [SerializeField] private Image _viewImage;

        public void UpdateHealthView(int currentHealth, int maxHealth)
        {
            _healthBar_UI.UpdateHealthView(currentHealth, maxHealth);
        }

        public void SetColorForView(Color color)
        {
            _viewImage.color = color;
        }
    }
}
