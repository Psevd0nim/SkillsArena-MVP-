using UnityEngine;

namespace SkillsArena
{
    public abstract class UI_Manager : MonoBehaviour
    {
        [SerializeField] private SoundButton _soundButton;
        private protected AudioManager _audioManager;

        public virtual void Init()
        {
            _audioManager = AudioManager.Instance;
            _soundButton.OnPressed += AfterSoundButtonPressed;
            _soundButton.SetActive(_audioManager.AudioActive);
        }

        private void AfterSoundButtonPressed()
        {
            _audioManager.SetAudioStatus(!_audioManager.AudioActive);
            _soundButton.SetActive(_audioManager.AudioActive);
        }
    }
} 