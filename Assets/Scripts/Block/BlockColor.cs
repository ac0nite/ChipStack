using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BlockColor : MonoBehaviour
{
    private Renderer _render = null;
    private Material _material = null;
    public Color Color { get; set; }
    public Color FogColor { get;  private set; }

    private void Awake()
    {
        _render = GetComponent<Renderer>();
        FogColor = Camera.main.backgroundColor;
        GameManager.Instance.FogColor.EventUpdateFogColor += OnUpdateFogColor;
        //Camera.main.GetComponent<UpdateFogColor>().EventUpdateFogColor += OnUpdateFogColor;
    }


    private void OnDestroy()
    {
        if (GameManager.TryInstance != null)
            GameManager.Instance.FogColor.EventUpdateFogColor -= OnUpdateFogColor;
    }

    void Start()
    {
        NextColor();
    }

    private void Update()
    {
        _material = _render.material;
        var color = Color.Lerp(_material.GetColor("_FogColor"), FogColor, Time.deltaTime * GameManager.Instance.FogColor.SpeedLerp);
        _material.SetColor("_FogColor", color);
        _render.material = _material;
    }

    public void NextColor()
    {
        SetColor(GameManager.Instance.Gradient.GetColor());
    }

    public void SetColor(Color color)
    {
        Color = color;
        _material = _render.material;
        _material.SetColor("_Color", Color);
        _render.material = _material;
    }

    void OnUpdateFogColor(Color color)
    {
        FogColor = color;
    }
}
