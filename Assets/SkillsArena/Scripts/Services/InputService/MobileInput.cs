using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillsArena
{
    public class MobileInput : InputService
    {
        private Touchscreen _touchscreen;
        private Vector2 _startPos;
        private float _minMagnitude = 2;

        public MobileInput()
        {
            _touchscreen = Touchscreen.current;
        }

        public override void UpdateSomethingIfNeed()
        {
            if (LeftMouseOrSameWasPressedThisFrame())
            {
                _startPos = GetInputPosition();
            }
        }

        public override InputLikeKeyboardType GetCurrentKeyWasPressedThisFrame()
        {
            InputLikeKeyboardType inputType = InputLikeKeyboardType.None;
            return inputType;
        }

        public override InputLikeKeyboardType GetCurrentKeyWasReleasedThisFrame()
        {
            InputLikeKeyboardType inputType = InputLikeKeyboardType.None;
            if (LeftMouseOrSameWasReleasedThisFrame())
            {
                Vector2 endPos = GetInputPosition();
                Vector2 delta = endPos - _startPos;
                if (delta.magnitude > _minMagnitude)
                {
                    if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        inputType = delta.x > 0 ? InputLikeKeyboardType.Right : InputLikeKeyboardType.Left;
                    }
                    else
                    {
                        inputType = delta.y > 0 ? InputLikeKeyboardType.Up : InputLikeKeyboardType.Down;
                    }
                }
            }
            return inputType;
        }

        public override Vector2 GetInputPosition()
        {
            Vector2 currentMousePosOnScreen = _touchscreen.primaryTouch.value.position;
            Vector2 touchPosInWorld = Camera.main.ScreenToWorldPoint(currentMousePosOnScreen);
            return touchPosInWorld;
        }

        public override bool LeftMouseOrSameWasPressedThisFrame()
        {
            return _touchscreen.primaryTouch.press.wasPressedThisFrame;
        }

        public override bool LeftMouseOrSameWasReleasedThisFrame()
        {
            return _touchscreen.primaryTouch.press.wasReleasedThisFrame;
        }
    }
}