using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _buttonUIClicks;
    [SerializeField] private AudioClip _doorSound;
    [SerializeField] private AudioClip _dashboardClicks;
    [SerializeField] private AudioClip _fuelingGas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayUIButtonClicks() {
        if (_buttonUIClicks != null && _audioSource != null)
        {
            _audioSource.volume = 0.6f;
            _audioSource.PlayOneShot(_buttonUIClicks);
        }
    }

    public void PlayDoorSound()
    {
        if (_doorSound != null && _audioSource != null)
        {
            _audioSource.volume = 0.5f;
            _audioSource.PlayOneShot(_doorSound);
        }
    }

    public void PlayDashboardClicks()
    {
        if (_dashboardClicks != null && _audioSource != null)
        {
            _audioSource.volume = 0.6f;
            _audioSource.PlayOneShot(_dashboardClicks);
        }
    }

    private AudioSource _loopAudioSource;

    public void PlayRefuelingSound(bool play)
    {
        if (_loopAudioSource == null)
        {
            _loopAudioSource = gameObject.AddComponent<AudioSource>();
            _loopAudioSource.loop = true;
            _loopAudioSource.playOnAwake = false;
        }

        if (play)
        {
            if (!_loopAudioSource.isPlaying && _fuelingGas != null)
            {
                _loopAudioSource.clip = _fuelingGas;
                _loopAudioSource.Play();
            }
        }
        else
        {
            if (_loopAudioSource.isPlaying)
            {
                _loopAudioSource.Stop();
            }
        }
    }
}