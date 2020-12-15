using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBubble : MonoBehaviour
{
    private AudioSource _audio = null;
    private ParticleSystem _fx = null;

    private void Awake()
    {
        _audio = GetComponentInChildren<AudioSource>();
        _fx = GetComponentInChildren<ParticleSystem>();
        
    }

    void Start()
    {
        _fx.randomSeed = (uint)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        _audio.pitch = Random.Range(0.9f, 1.2f);

        _fx.Play();
        _audio.Play();

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return null;
        Destroy(this.gameObject, _fx.main.duration);
    }
}
