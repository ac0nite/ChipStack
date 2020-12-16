using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBubble : MonoBehaviour
{
    private AudioSource _audio = null;
    private ParticleSystem _fx = null;
    [SerializeField] private AudioClip _destroyClip = null;
    [SerializeField] private AudioClip _bubbleClip = null;

    private void Awake()
    {
        _audio = GetComponentInChildren<AudioSource>();
        _fx = GetComponentInChildren<ParticleSystem>();
        
    }

    void Start()
    {
        //_fx.randomSeed = (uint)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        _audio.pitch = Random.Range(0.8f, 1.2f);
        _audio.clip = _destroyClip;
        _fx.Play();
        _audio.Play();

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_audio.clip.length);
        _audio.clip = _bubbleClip;
        _audio.Play();
        // Destroy(this.gameObject, _fx.main.duration);
        Destroy(this.gameObject, _audio.clip.length);
    }
}
