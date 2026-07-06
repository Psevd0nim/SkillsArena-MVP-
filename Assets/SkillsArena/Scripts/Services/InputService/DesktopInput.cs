using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillsArena
{
    public class DesktopInput : InputService
    {
        private Mouse _currentMouse;
        private Keyboard _currentKeyboard;

        public DesktopInput()
        {
            _currentMouse = Mouse.current;
            _currentKeyboard = Keyboard.current;
        }

        public override Vector2 GetInputPosition()
        {
            Vector2 currentMousePosOnScreen = _currentMouse.position.value;
            Vector2 mousePosInWorld = Camera.main.ScreenToWorldPoint(currentMousePosOnScreen);
            return mousePosInWorld;
        }

        public override InputLikeKeyboardType GetCurrentKeyWasPressedThisFrame()
        {
            InputLikeKeyboardType inputType = InputLikeKeyboardType.None;

            if (_currentKeyboard.wKey.wasPressedThisFrame || _currentKeyboard.upArrowKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Up;
            if (_currentKeyboard.sKey.wasPressedThisFrame || _currentKeyboard.downArrowKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Down;
            if (_currentKeyboard.aKey.wasPressedThisFrame || _currentKeyboard.leftArrowKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Left;
            if (_currentKeyboard.dKey.wasPressedThisFrame || _currentKeyboard.rightArrowKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Right;
            if(_currentKeyboard.escapeKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Escape;
            if(_currentKeyboard.spaceKey.wasPressedThisFrame)
                inputType = InputLikeKeyboardType.Space;

            return inputType;
        }

        public override InputLikeKeyboardType GetCurrentKeyWasReleasedThisFrame()
        {
            InputLikeKeyboardType inputType = InputLikeKeyboardType.None;

            if (_currentKeyboard.wKey.wasReleasedThisFrame || _currentKeyboard.upArrowKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Up;
            if (_currentKeyboard.sKey.wasReleasedThisFrame || _currentKeyboard.downArrowKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Down;
            if (_currentKeyboard.aKey.wasReleasedThisFrame || _currentKeyboard.leftArrowKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Left;
            if (_currentKeyboard.dKey.wasReleasedThisFrame || _currentKeyboard.rightArrowKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Right;
            if (_currentKeyboard.escapeKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Escape;
            if (_currentKeyboard.spaceKey.wasReleasedThisFrame)
                inputType = InputLikeKeyboardType.Space;

            return inputType;
        }

        public override bool LeftMouseOrSameWasPressedThisFrame()
        {
            return _currentMouse.leftButton.wasPressedThisFrame;
        }

        public override bool LeftMouseOrSameWasReleasedThisFrame()
        {
            return _currentMouse.leftButton.wasReleasedThisFrame;
        }
    }
}