using UnityEngine;

namespace SkillsArena
{
    public interface IInputService : IService
    {
        public abstract Vector2 GetInputPosition();
        public abstract InputLikeKeyboardType GetCurrentKeyWasPressedThisFrame();
        public abstract InputLikeKeyboardType GetCurrentKeyWasReleasedThisFrame();
        public abstract bool LeftMouseOrSameWasPressedThisFrame();
        public abstract bool LeftMouseOrSameWasReleasedThisFrame();
        public abstract void UpdateSomethingIfNeed();
    }
}