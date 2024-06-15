using System;
using UnityEngine;

namespace Core.Helper
{
    public static class CustomMath
    {
        public class CosExtended
        {
            private float _speed;
            private float _amplitude;
            private float _cashedValue;
            private float _samplingRate;

            public void SetSettings(Settings settings)
            {
                _speed = settings.Speed;
                _amplitude = settings.Amplitude * 0.5f;
                _cashedValue = settings.Min + _amplitude;
                _samplingRate = 0f;
            }

            public void UpdateValue(float deltaTime)
            {
                Value = _cashedValue - _amplitude * Mathf.Cos(_samplingRate);
                _samplingRate += _speed * deltaTime;
            }

            public float Value { get; private set; } = 0f;

            [Serializable]
            public struct Settings
            {
                [Range(0f, 20f)] public float Amplitude;
                [Range(0f, 20f)] public float Min;
                [Range(0f, 20f)] public float Speed;
                
                public void UpdateAmplitude(float value) => Amplitude = value;
                public void UpdateMin(float value) => Min = value;
                public void UpdateSpeed(float value) => Speed = value;

                public Settings Clone() => new Settings()
                {
                    Amplitude = Amplitude,
                    Min = Min,
                    Speed = Speed
                };
            }
        }
    }
}