using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ChangeTextAnimation : MonoBehaviour
{
    [SerializeField] private Text _scoreTxt = null;
    [SerializeField] private String _prefix = null;
    private int _from = 0;
    private int _to = 0;
    private int _step = 1;
    private AudioSource _win = null;
    private readonly WaitForSeconds _wait007Second = new WaitForSeconds(0.07f);
    private readonly WaitForSeconds _wait015Second = new WaitForSeconds(0.15f);
    private readonly WaitForSeconds _wait08Second = new WaitForSeconds(0.8f);

    private void Awake()
    {
        _win = GetComponent<AudioSource>();
        _scoreTxt.text = $"{_prefix}{_from}";
    }

    public void Play(int toParam)
    {
        if(_to == toParam)
            return;

        _to = toParam;
        StartCoroutine(Change());
    }

    IEnumerator Change()
    {
        _scoreTxt.fontStyle = FontStyle.Bold;
        _scoreTxt.fontSize += 4;
        
        while (_from <= _to)
        {
            _scoreTxt.text = $"{_prefix}{_from}";
            
            var diff = _to - _from;
            if (diff >= 1000) _step = 1000;
            else if (diff >= 100) _step = 100;
            else if (diff >= 10) _step = 10;
            else _step = 1;
            yield return _wait007Second;
            _from += _step;
        }

        if (_to == 0)
        {
            _scoreTxt.text = $"{_prefix}0";
            _from = _to = 0;
        }
        
        yield return _wait015Second;
        _win.Play();
        
        yield return _wait007Second;
        
        _scoreTxt.fontSize -= 4;
        _scoreTxt.fontStyle = FontStyle.Bold;
    }
}
