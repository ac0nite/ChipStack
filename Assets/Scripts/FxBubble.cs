using System.Collections;
using UnityEngine;

public class FxBubble : MonoBehaviour
{
    private ParticleSystem _fx;
    [SerializeField] private AudioClip _destroyClip;
    [SerializeField] private AudioClip _bubbleClip;
    private WaitForSeconds _await;

    private void Awake()
    {
        _fx = GetComponentInChildren<ParticleSystem>();
        _await = new WaitForSeconds(_destroyClip.length);
    }

    void Start()
    {
        AudioManager.Play(_destroyClip.name);
        _fx.Play();

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return _await;
        AudioManager.Play(_bubbleClip.name);
        Destroy(gameObject, _bubbleClip.length);
    }
}
