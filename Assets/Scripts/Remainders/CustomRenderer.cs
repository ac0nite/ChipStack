using Components;
using UnityEngine;

namespace Remainders
{
    public class CustomRenderer : ColorRenderer
    {
        public CustomRenderer(Renderer renderer) : base(renderer)
        { }
        public void Enable() => _renderer.enabled = true;
        public void Disable() => _renderer.enabled = false;
    }
}