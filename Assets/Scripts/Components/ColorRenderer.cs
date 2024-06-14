using UnityEngine;

namespace Components
{
    public class ColorRenderer
    {
        protected readonly Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;
        private Color _color;
        private Color _fogColor;

        public ColorRenderer(Renderer renderer)
        {
            _renderer = renderer;
            _propertyBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateColor();
            }
        }
    
        public Color FogColor
        {
            get => _fogColor;
            set
            {
                _fogColor = value;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            _propertyBlock.SetColor(ColorConstants.ColorName, _color);
            _propertyBlock.SetColor(ColorConstants.FogColorName, _fogColor);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}