using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class DependencyRoundParent_UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _roundText;
        [SerializeField] private LayoutGroup _layoutGroup;

        public void SetRoundText(int round)
        {
            _roundText.text = $"ROUND {round}";
        }

        public void SetLayoutGroupActive(bool active)
        {
            _layoutGroup.enabled = active;
        }
    }
}
