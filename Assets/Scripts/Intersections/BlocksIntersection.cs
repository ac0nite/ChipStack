using System;
using Components;
using UnityEngine;

namespace Intersections
{
    public class BlocksIntersection
    {
        private IComponent _bottom;
        private IComponent _top;
        private Rect? _intersection;
        private (Rect oneRemainder, Rect otherRemainder) _remainders;
    
        private readonly Intersection IntersectionZero = Intersection.Zero;
        private readonly Settings _settings;

        public BlocksIntersection(Settings settings)
        {
            _settings = settings;
            _bottom = null;
            _top = null;
            _intersection = null;
        }

        public void Add(IComponent block)
        {
            if(_bottom == null)
                _bottom = block;
            else if (_top == null)
                _top = block;
            else
            {
                _bottom = _top;
                _top = block;
            }
        }

        public void Clear()
        {
            _bottom = null;
            _top = null;
            _intersection = null;
        }

        public bool HasIntersect
        {
            get
            {
                if (_bottom == null || _top == null)
                    return false;
        
                var bottomRect = GetRect(_bottom);
                var topRect = GetRect(_top);

                if ((_intersection = GetIntersectionWithClamp(bottomRect, topRect, _settings.MinSize)) != null)
                {
                    _remainders = GetTopRemainderIntersection(_intersection.Value);
                }
            
                return _intersection != null;    
            }
        }

        public Intersection AreaOfIntersection => ConvertRectToIntersectionTransform(_intersection, _top);
    
        public (Intersection One, Intersection Two) AreaOfRemaindersIntersection => 
            (ConvertRectToIntersectionTransform(_remainders.oneRemainder, _top), 
                ConvertRectToIntersectionTransform(_remainders.otherRemainder, _top));
        private Rect GetRect(IComponent block)
        {
            var pos = block.Position;
            var scale = block.Size;
            return new Rect(pos.x - scale.x / 2, pos.z - scale.z / 2, scale.x, scale.z);
        }

        private Intersection ConvertRectToIntersectionTransform(Rect? intersection, IComponent original)
        {
            return intersection == null ? IntersectionZero : new Intersection(intersection.Value, original);
        }

        private Rect? GetIntersection(Rect a, Rect b)
        {
            var x1 = Mathf.Max(a.xMin, b.xMin);
            var y1 = Mathf.Max(a.yMin, b.yMin);
            var x2 = Mathf.Min(a.xMax, b.xMax);
            var y2 = Mathf.Min(a.yMax, b.yMax);

            if (x1 < x2 && y1 < y2)
            {
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            }
            else
            {
                return null;
            }
        }
        
        private Rect? GetIntersectionWithClamp(Rect a, Rect b, float min)
        {
            var x1 = Mathf.Max(a.xMin, b.xMin);
            var y1 = Mathf.Max(a.yMin, b.yMin);
            var x2 = Mathf.Min(a.xMax, b.xMax);
            var y2 = Mathf.Min(a.yMax, b.yMax);

            if (x1 < x2 && y1 < y2)
            {
                var width = x2 - x1;
                var height = y2 - y1;
                if (b.width - width < min)
                {
                    width = b.width;
                    x1 = a.xMin;
                }
                
                if (b.height - height < min)
                {
                    height = b.height;
                    y1 = a.yMin;
                }
                return new Rect(x1, y1, width, height);
            }
            else
            {
                return null;
            }
        }

        private (Rect, Rect) GetTopRemainderIntersection(Rect intersection)
        {
            var topScale = _top.Size;

            Vector2 remOnePos, remTwoPos;
            var width = topScale.x - intersection.width;
            var height = topScale.z - intersection.height;
        
            if (_top.Position.x < _bottom.Position.x)
            {
                remOnePos.x = intersection.xMin - width;
                remOnePos.y = intersection.yMin;
            }
            else
            {
                remOnePos.x = intersection.xMax;
                remOnePos.y = intersection.yMin;
            }

            if (_top.Position.z > _bottom.Position.z)
            {
                remTwoPos.x = intersection.xMin;
                remTwoPos.y = intersection.yMax;
            }
            else
            {
                remTwoPos.x = intersection.xMin;
                remTwoPos.y = intersection.yMin - height;
            
                remOnePos.y = intersection.yMin - height;
            }

            return (new Rect(remOnePos.x, remOnePos.y, width, topScale.z), 
                new Rect(remTwoPos.x, remTwoPos.y, intersection.width, height));
        }
        
        [Serializable]
        public struct Settings
        {
            [Min(0)] public float MinSize;
        }
    }
}