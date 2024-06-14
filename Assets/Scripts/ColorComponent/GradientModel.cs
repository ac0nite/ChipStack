using System;
using SavingData;
using UnityEngine;

namespace ColorComponent
{
    public interface IGradientModel
    {
        Color Color { get; }
        Color FogColor { get; }
        void UpdateColor();
        event Action<Color, Color> NextColorEvent;
    }

    public class GradientModel : GradientComponent, IGradientModel
    {
        public GradientModel(Settings settings) : base(settings, SavingKeys.GradientIndexKey)
        { }
        public event Action<Color, Color> NextColorEvent;
        public Color Color => _currentGradientColor;
        public Color FogColor => _nextGradientColor;
        public void UpdateColor()
        {
            UpdateNextColor();
            NextColorEvent?.Invoke(_currentGradientColor, _nextGradientColor);
        }
    }
}