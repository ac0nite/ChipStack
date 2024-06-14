using System;
using Components;
using UnityEngine;

namespace Blocks.Movements
{
    public class CenterMovement : IMovable
    { 
        private readonly IComponent _movable;
        private Settings _settings;
        private Vector3 _center;
        private Vector3 _size;
        private float _minX;
        private float _maxX;
        private float _minZ;
        private float _maxZ;
        private Vector3 _target;

        public CenterMovement(IComponent component)
        {
            _movable = component;
        }
        public void UpdateSettings(Settings settings, IComponent previewComponent)
        {
            _settings = settings;
            
            _size = previewComponent.Size;
            
            _center = previewComponent.Position;
            _center.y += _size.y + _settings.ShiftFromUp;

            _minX = _center.x - _size.x * 0.5f - _settings.ShiftFromEdge;
            _maxX = _center.x + _size.x * 0.5f + _settings.ShiftFromEdge;
            _minZ = _center.z - _size.z * 0.5f - _settings.ShiftFromEdge;
            _maxZ = _center.z + _size.z * 0.5f + _settings.ShiftFromEdge;

            _movable.Position = _center;
            _target = RandomTargetPoint;
        }
        public void UpdatePosition(float deltaTime)
        {
            if(IsCompleted) _target = RandomTargetPoint;
            Position = _movable.Position + Direction.normalized * (deltaTime * _settings.Speed);
        }
        public Vector3 Position { get; private set; }
        public bool IsCompleted => Direction.sqrMagnitude < 0.01f;
        public void ApplyPosition()
        {
            _movable.Position = Position;
        }
        private Vector3 Direction => _target - _movable.Position;
        private float RandomX => UnityEngine.Random.Range(_minX, _maxX);
        private float RandomZ => UnityEngine.Random.Range(_minZ, _maxZ);
        private Vector3 RandomTargetPoint => new Vector3(RandomX, _center.y, RandomZ);

        [Serializable]
        public struct Settings
        {
            [Range(0,5)] public float Speed;
            [Range(0,5)] public float ShiftFromEdge;
            [Range(0,1)] public float ShiftFromUp;
        }
    }
}