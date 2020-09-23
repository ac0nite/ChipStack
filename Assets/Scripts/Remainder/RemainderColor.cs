using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainderColor : MonoBehaviour
{
    public Color Color
    {
        set
        {
            var renderers = GetComponentsInChildren<Renderer>();
            Material material = null;
            foreach (var renderer in renderers)
            {
                material = renderer.material;
                material.SetColor("_Color", value);
                renderer.material = material;
            }
            //Material material = renderer.material;
            //material.SetColor("_Color", value);
            //renderer.material = material;
        }
    }
}
