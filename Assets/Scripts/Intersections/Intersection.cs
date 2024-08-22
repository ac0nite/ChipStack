using Remainders;
using UnityEngine;

namespace Intersections
{
    public struct Intersection
    {
        private Vector3 _position;
        private Vector3 _size;

        public Intersection(Rect rect, float height, float depth)
        {
            _position = new Vector3(rect.x + rect.width * 0.5f, height, rect.y + rect.height * 0.5f);
            _size = new Vector3(rect.width, depth, rect.height);
        }

        public void ApplyTo(PivotTransform applyToTransform)
        {
            applyToTransform.Position = _position;
            applyToTransform.Size = _size;
        }
        public bool IsValid => _size is { x: > 0, z: > 0 };
        
        public int Area => (int)(_size.x * _size.z);
    
        public static Intersection Zero => new Intersection()
        {
            _position = Vector3.zero, 
            _size = Vector3.zero
        };

        public override string ToString() => $"Position:{_position} Scale:{_size}";
        //public override string ToString() => $"Scale:{_scale.x} {_scale.z}";
        // public override string ToString() => $"{_position}";
    }
    
    public static class RectExtension
    {
        public static Intersection ToIntersection(this Rect rect, float width, float depth)
        {
            return new Intersection(rect, width, depth);
        }
    }

    public static class RectPairExtension
    {
        public static (Intersection one, Intersection two) ToIntersection(this (Rect one, Rect two) pair, float width, float depth)
        {
            return (pair.one.ToIntersection(width, depth), pair.two.ToIntersection(width, depth));
        }
    }
}