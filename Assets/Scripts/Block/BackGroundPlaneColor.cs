using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundPlaneColor : MonoBehaviour
{
    private Color _current;
    private Color _target;
    private Renderer _renderer = null;
    private Material _material = null;
    [SerializeField] private float _speedChangeColor = 5f;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        NextColor();
    }

    void Update()
    {
        _current = _material.GetColor("_Color");
        var color = Color.Lerp(_current, _target, Time.deltaTime * _speedChangeColor);
        _material.SetColor("_Color", color);
        _renderer.material = _material;
    }

    public void NextColor()
    {
        _target = GameManager.Instance.Gradient.GetToColor();
    }
}
