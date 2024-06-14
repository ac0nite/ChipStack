using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] private float _volume = 1f;
    [SerializeField] private AudioSource _primaryAudioSource = null;
    [SerializeField] private AudioSource _secondaryAudioSource;
    [SerializeField] private string _backgrountAudioPlaying = null;
    [SerializeField] private string _backgrountAudioStart = null;
    [SerializeField] public bool IsSound = false;
    private static AudioManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        AudioListener.volume = _volume;
    }


    public void StartMusicBackgroundPlaying()
    {
        StopMusicBackground();
        AudioClip clip = Resources.Load<AudioClip>(("Music/" + _backgrountAudioPlaying));
        _primaryAudioSource.clip = clip;
        _primaryAudioSource.Play();
    }
    public void StartMusicBackground()
    {
        StopMusicBackground();
        AudioClip clip = Resources.Load<AudioClip>(("Music/" + _backgrountAudioStart));
        _primaryAudioSource.clip = clip;
        _primaryAudioSource.Play();
    }

    public void NoVolumeSound()
    {
        _volume = 0f;
        AudioListener.volume = _volume;
    }

    public void YesVolumeSound()
    {
        _volume = 1f;
        AudioListener.volume = _volume;
    }
    
    public static void PlayOneShot(string clipName)
    {
        _instance._secondaryAudioSource.PlayOneShot(Resources.Load<AudioClip>($"Music/{clipName}"));
    }
    
    public static void Play(string clipName)
    {
        _instance._secondaryAudioSource.clip = Resources.Load<AudioClip>($"Music/{clipName}");
        _instance._secondaryAudioSource.Play();
    }

    public static void Stop()
    {
        _instance._secondaryAudioSource.Stop();
    }

    private void StopMusicBackground()
    {
        _primaryAudioSource.Stop();
    }

    public static void Mute(bool onSound)
    {
        _instance._primaryAudioSource.mute = onSound;
        _instance._secondaryAudioSource.mute = onSound;
    }
}
