﻿using System;
using Components;
using UnityEngine;

namespace Intersections
{
    public class BlocksIntersection
    {
        private IComponent _bottom;
        private IComponent _top;
        
        private Rect _general;
        private (Rect one, Rect two) _remainders;
    
        private readonly RectTransform _rectTransformZero = RectTransform.Zero;
        private readonly Settings _settings;

        public BlocksIntersection(Settings settings)
        {
            _settings = settings;
            _bottom = null;
            _top = null;
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
        }

        public bool HasIntersect
        {
            get
            {
                if (_bottom == null || _top == null)
                    return false;
        
                var bottomRect = GetRect(_bottom);
                var topRect = GetRect(_top);

                return HasIntersectionWithClamp(bottomRect, topRect, _settings.MinSize);    
            }
        }

        public Rect GeneralRect => _general;
        public (Rect one, Rect two) RemaindersRect => CalculateTopRemaindersRect(_general);
        
        private Rect GetRect(IComponent block)
        {
            var pos = block.Position;
            var scale = block.Size;
            return new Rect(pos.x - scale.x / 2, pos.z - scale.z / 2, scale.x, scale.z);
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

        public Vector2 Offset { get; private set; }

        private bool HasIntersectionWithClamp(Rect a, Rect b, float min)
        {
            var x1 = Mathf.Max(a.xMin, b.xMin);
            var y1 = Mathf.Max(a.yMin, b.yMin);
            var x2 = Mathf.Min(a.xMax, b.xMax);
            var y2 = Mathf.Min(a.yMax, b.yMax);
            Offset = Vector3.zero;

            if (x1 < x2 && y1 < y2)
            {
                var x1d = x1;
                var y1d = y1;
                
                var width = x2 - x1;
                var height = y2 - y1;
                if (b.width - width < min)
                {
                    Offset = new Vector2(x1 - a.xMin, 0);
                    width = b.width;
                    x1 = a.xMin;
                }
                
                if (b.height - height < min)
                {
                    Offset = new Vector2(Offset.x, y1 - a.yMin);
                    height = b.height;
                    y1 = a.yMin;
                }
                
                // Debug.Log($"{x1d + Offset.x} {y1d + Offset.y} == {x1} {y1}");
                
                _general = new Rect(x1, y1, width, height);

                return true;
            }
            else
            {
                _general = Rect.zero;
                return false;
            }
        }

        private (Rect, Rect) CalculateTopRemaindersRect(Rect intersection)
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