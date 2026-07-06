using System;
using UnityEngine;

namespace SkillsArena
{
    public class Button_UI : MonoBehaviour
    {
        public event Action OnPressed;
        
        public virtual void Pressed()
        {
            OnPressed?.Invoke();
            AudioManager.Instance.PlaySomeSound(SoundType.ClickButton);
        }
    }
}
