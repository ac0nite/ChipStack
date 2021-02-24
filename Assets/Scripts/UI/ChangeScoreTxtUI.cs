using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ChangeScoreTxtUI : MonoBehaviour
{
    [SerializeField] private Text _scoreTxt = null;
    [SerializeField] private String _prefix = null;
    private int _from = 0;
    private int _to = 0;
    private int _step = 1;
    private AudioSource _win = null;

    private void Awake()
    {
        _win = GetComponent<AudioSource>();
        _scoreTxt.text = _prefix + _from.ToString();
    }

    public void ChangeParam(int toParam)
    {
        if(_to == toParam)
            return;

        _to = toParam;
        StartCoroutine(Change());
    }

    IEnumerator Change()
    {
        while (_from <= _to)
        {
            _scoreTxt.text = _prefix + _from.ToString();
            
            var diff = _to - _from;
            if (diff >= 1000) _step = 1000;
            else if (diff >= 100) _step = 100;
            else if (diff >= 10) _step = 10;
            //else if (diff >= 20) _step = 2;
            else _step = 1;
            //yield return new WaitForSeconds(_delay);
            yield return null;
            _from += _step;
        }

        if (_to == 0)
        {
            _scoreTxt.text = _prefix + "0";
            _from = _to = 0;
        }

        _scoreTxt.fontStyle = FontStyle.Bold;
        _scoreTxt.fontSize += 3;
        yield return null;
        _win.Play();
        
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        
        _scoreTxt.fontSize -= 3;
        _scoreTxt.fontStyle = FontStyle.Normal;
    }
}
