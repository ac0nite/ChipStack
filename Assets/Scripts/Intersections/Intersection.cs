using Components;
using UnityEngine;

namespace Intersections
{
    public struct Intersection
    {
        private Vector3 _position;
        private Vector3 _scale;

        public Intersection(Rect rect, IComponent original)
        {
            _position = new Vector3(rect.x + rect.width / 2, original.Position.y, rect.y + rect.height / 2);
            _scale = new Vector3(rect.width, original.Size.y, rect.height);
        }

        public void ApplyTo(Transform applyToTransform)
        {
            applyToTransform.position = _position;
            applyToTransform.localScale = _scale;
        }
        
        public int Area => (int)(_scale.x * _scale.z);
    
        public static Intersection Zero => new Intersection()
        {
            _position = Vector3.zero, 
            _scale = Vector3.zero
        };

        public override string ToString() => $"Position:{_position} Scale:{_scale}";
    }
}