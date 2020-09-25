using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColor : MonoBehaviour
{
    public Color Color { get; private set; }
    void Start()
    {
        NextColor();
    }

    public void NextColor()
    {
        Color = GameManager.Instance.Gradient.GetColor();
        var renderer = GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetColor("_Color", Color);
        renderer.material = material;
    }
}
