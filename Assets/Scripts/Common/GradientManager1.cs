using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientManager1 : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 1f)] private float _step = 0.03f;

    private float _evalute = 0f;
    private Gradient _gradient = null;
    public GradientColorKey[] colorKeys;
    public GradientAlphaKey[] alphaKeys;
    private Color _firstColor;
    private Color _secondColor;
    void Awake()
    {
        _gradient = new Gradient();
        colorKeys = new GradientColorKey[2];
        alphaKeys = new GradientAlphaKey[2];

        _firstColor = RandomColor();
        _secondColor = RandomColor();

        GenerateGradient();
    }

    void Start()
    {
    }

    public Gradient getGradient()
    {
        return _gradient;
    }

    public Gradient Lerp(Gradient fromC, Gradient toC, float t)
    {
        Gradient _grLerp = new Gradient();

        Color fromColor = Color.Lerp(fromC.colorKeys[0].color, toC.colorKeys[0].color, t);
        Color toColor = Color.Lerp(fromC.colorKeys[1].color, toC.colorKeys[1].color, t);

        GradientColorKey[] cKeys = new GradientColorKey[2];
        GradientAlphaKey[] aKeys = new GradientAlphaKey[2];

        cKeys[0].color = fromColor;
        cKeys[0].time = 0f;
        aKeys[0].alpha = 0f;
        aKeys[0].time = 0f;

        cKeys[1].color = toColor;
        cKeys[1].time = 1f;
        aKeys[1].alpha = 0f;
        aKeys[1].time = 1f;

        _grLerp.SetKeys(cKeys, aKeys);

        return _grLerp;
    }

    public Color RandomColor()
    {
        return new Color(Gen(), Gen(), Gen());
    }

    public void GenerateGradient()
    {
        _evalute = 0f;
        //float steps = 2 - 1f;
        //for (int i = 0; i < 2; i++)
        //{
        //    float step = i / steps;
        //    colorKeys[i].color = RandomColor();
        //    colorKeys[i].time = step;
        //    alphaKeys[i].alpha = 0.0F;
        //    alphaKeys[i].time = step;
        //}

        colorKeys[0].color = _firstColor;
        colorKeys[0].time = 0f;
        alphaKeys[0].alpha = 0.0f;
        alphaKeys[0].time = 0.0f;

        colorKeys[1].color = _secondColor;
        colorKeys[1].time = 1f;
        alphaKeys[0].alpha = 0.0f;
        alphaKeys[0].time = 1f;

        _gradient.SetKeys(colorKeys, alphaKeys);
    }

    private float Gen()
    {
        return UnityEngine.Random.Range(0f, 1f);
    }

    public Color GetColor()
    {
        _evalute += _step;
        if (_evalute >= 1)
        {
            _evalute = 0f;
            _firstColor = _secondColor;
            _secondColor = RandomColor();
            GenerateGradient();
        }
        return _gradient.Evaluate(_evalute);
    }

    public Color GetFromColor()
    {
        return _gradient.Evaluate(0f);
    }

    public Color GetToColor()
    {
        return _gradient.Evaluate(1f);
    }
}
