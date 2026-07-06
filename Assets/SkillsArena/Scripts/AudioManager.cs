using UnityEngine;

namespace SkillsArena
{
    public class AudioManager : MonoBehaviour, IService
    {
        public bool AudioActive { get; private set; }

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private SoundConfig _soundConfig;

        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AudioActive = PlayerPrefs.GetInt(Constants.SoundVolumeKey, 1) == 1;
            SetAudioStatus(AudioActive);
        }

        public void PlaySomeSound(SoundType type)
        {
            SoundData soundData = _soundConfig.GetSoundDataByType(type);
            _audioSource.PlayOneShot(soundData.sound, soundData.volume);
        }

        public void SetAudioStatus(bool status)
        {
            AudioActive = status;
            AudioListener.volume = AudioActive ? 1 : 0;
        }

        public void SetBackgroundStatus(bool status)
        {
            _backgroundAudioSource.enabled = status;
        }
    }
}