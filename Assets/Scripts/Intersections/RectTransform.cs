using UnityEngine;

namespace Intersections
{
    public struct RectTransform
    {
        private Vector3 _position;
        private Vector3 _size;

        public RectTransform(Rect rect, float height, float depth)
        {
            _position = new Vector3(rect.x + rect.width * 0.5f, height, rect.y + rect.height * 0.5f);
            _size = new Vector3(rect.width, depth, rect.height);
        }

        public void ApplyTo(Transform applyToTransform)
        {
            applyToTransform.position = _position;
            applyToTransform.localScale = _size;
        }
        public bool IsValid => _size is { x: > 0, z: > 0 };
        
        public int Area => (int)(_size.x * _size.z);
    
        public static RectTransform Zero => new RectTransform()
        {
            _position = Vector3.zero, 
            _size = Vector3.zero
        };

        //public override string ToString() => $"Position:{_position} Scale:{_scale}";
        //public override string ToString() => $"Scale:{_scale.x} {_scale.z}";
        public override string ToString() => $"{_position}";
    }
    
    public static class RectExtension
    {
        public static RectTransform ToRectTransform(this Rect rect, float width, float depth)
        {
            return new RectTransform(rect, width, depth);
        }
    }

    public static class RectPairExtension
    {
        public static (RectTransform one, RectTransform two) ToRectTransform(this (Rect one, Rect two) pair, float width, float depth)
        {
            return (pair.one.ToRectTransform(width, depth), pair.two.ToRectTransform(width, depth));
        }
    }
}