using System;
using System.ComponentModel;
using Core.Helper;
using UnityEngine;
using IComponent = Components.IComponent;
using Random = System.Random;

namespace Blocks.Movements
{
    public class CircleMovement : IMovable
    {
        public enum CircleType { clockwise, anticlockwise, random }
    
        private readonly IComponent _centerPoint;
        private readonly CustomMath.CosExtended _radius = new ();
        private readonly CustomMath.CosExtended _speed = new();
        private CircleType _circleType;
        private readonly Random _random;
        private float _samplingRate;
        private IComponent _movable;

        public CircleMovement(IComponent centerPoint)
        {
            _centerPoint = centerPoint;
            _random = new Random();
        }

        public void UpdateSettings(Settings settings)
        {
            _circleType = settings.Type == CircleType.random ? (CircleType)_random.Next(0, 2) : settings.Type;
            _radius.SetSettings(settings.Radius);
            _speed.SetSettings(settings.Speed);
            _samplingRate = 0f;
        }

        public void AssignMoving(IComponent component)
        {
            _movable = component;
            
            UpdatePosition(0);
            ApplyPosition();
        }

        public void UpdatePosition(float deltaTime)
        {
            var center = _centerPoint.Position;
            _radius.UpdateValue(deltaTime);
            _speed.UpdateValue(deltaTime);
        
            _samplingRate += _speed.Value * deltaTime;

            Position =  _circleType switch
            {
                CircleType.clockwise => new Vector3(center.x + Mathf.Sin(_samplingRate) * _radius.Value, center.y, center.z + Mathf.Cos(_samplingRate) * _radius.Value),
                CircleType.anticlockwise => new Vector3(center.x + Mathf.Cos(_samplingRate) * _radius.Value, center.y, center.z + Mathf.Sin(_samplingRate) * _radius.Value),
                CircleType.random => throw new InvalidEnumArgumentException("Random type not supported"),
                _ => throw new ArgumentOutOfRangeException()
            };
        
            Debug.DrawLine(center, _movable.Position, Color.red);
        }

        public Vector3 Position { get; private set; }
        public bool IsCompleted { get; } = false;
        public void ApplyPosition()
        {
            _movable.Position = Position;
        }
    
        [Serializable]
        public struct Settings
        {
            public CircleMovement.CircleType Type;
            public CustomMath.CosExtended.Settings Radius;
            public CustomMath.CosExtended.Settings Speed;
        }
    }
}