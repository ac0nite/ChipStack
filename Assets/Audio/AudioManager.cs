using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 1.0f)] private float _volume = 1f;
    [SerializeField] private AudioSource _backgrountAudioSource = null;
    [SerializeField] private string _backgrountAudioName;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = _volume;
        StartMusicBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMusicBackground()
    {
        AudioClip clip = Resources.Load<AudioClip>(("Music/" + _backgrountAudioName));
        _backgrountAudioSource.clip = clip;
        _backgrountAudioSource.Play();
    }

    public void StopMusicBackground()
    {
        _backgrountAudioSource.Stop();
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
}
