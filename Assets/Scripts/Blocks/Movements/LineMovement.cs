using System;
using Components;
using UnityEngine;
using Random = System.Random;

namespace Blocks.Movements
{
    public class LineMovement : IMovable
    {
        private Settings _settings;
        private readonly Random _random;
        private IComponent _movable;
        private IMovable _target;
        private float _completedDistance;

        public LineMovement()
        {
            _random = new Random();
        }

        public void UpdateSettings(Settings settings)
        {
            _settings = settings;
            _completedDistance = _settings.CompletedDistance * 2f;
        }
        public void AssignMoving(IComponent component, IMovable target)
        {
            _movable = component;
            _target = target;
            _movable.Position = BeginRandomPosition;
            IsCompleted = false;
        }
    
        public void UpdatePosition(float deltaTime)
        {
            if(IsCompleted) return;
            Position = _movable.Position + Direction.normalized * (deltaTime * _settings.Speed);
            IsCompleted = Direction.sqrMagnitude < _completedDistance;
        }

        public Vector3 Position { get; private set; }
        public bool IsCompleted { get; private set; }
        public void ApplyPosition()
        {
            _movable.Position = Position;
        }

        private Vector3 Direction => _target.Position - Position;
        private Vector3 BeginRandomPosition => _random.Next(2) == 0 
            ? new Vector3(_settings.StartPosition, _target.Position.y, _movable.Position.z) 
            : new Vector3(_movable.Position.x, _target.Position.y, _settings.StartPosition);
    
        [Serializable]
        public struct Settings
        {
            [Range(0, 20)] public float Speed;
            public float StartPosition;
            [Range(0, 1)] public float CompletedDistance;
        }
    }
}