using UnityEngine;

namespace SkillsArena
{
    public abstract class InputService : IInputService
    {
        public abstract Vector2 GetInputPosition();
        public abstract InputLikeKeyboardType GetCurrentKeyWasPressedThisFrame();
        public abstract InputLikeKeyboardType GetCurrentKeyWasReleasedThisFrame();
        public abstract bool LeftMouseOrSameWasPressedThisFrame();
        public abstract bool LeftMouseOrSameWasReleasedThisFrame();

        public virtual void UpdateSomethingIfNeed()
        {
            
        }
    }
}