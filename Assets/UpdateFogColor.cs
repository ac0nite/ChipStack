using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFogColor : MonoBehaviour
{
    public Action<Color> EventUpdateFogColor;
    private Color _fogColor;
    [SerializeField] [Range(0.01f, 1f)] public float SpeedLerp = 0.5f;

    public Color FogColor
    {
        set
        {
            _fogColor = value;
            EventUpdateFogColor?.Invoke(_fogColor);
        }
        get { return _fogColor; }
    }

    void Update()
    {
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, _fogColor, SpeedLerp * Time.deltaTime);
    }
}
