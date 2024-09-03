using System;
using System.Collections.Generic;
using Animations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pivots
{
    public class PivotTransform : MonoBehaviour, IPivotExtendedTransform
    {
        public enum PivotWidth { center, top_left, top_right, bottom_left, bottom_right }
        public enum PivotHeight { center, top, bottom }
        
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _model;
        [Space]
        [SerializeField] public PivotWidth _pivotWidth;
        [SerializeField] public PivotHeight _pivotHeight;
        
        public Vector3 Position
        {
            get => _root.position;
            set => _root.position = value;
        }
        
        public Vector3 Size
        {
            get => _model.localScale;
            set
            {
                _model.localScale = value;
                UpdatePivotPosition();
            }
        }
        
        public void SetPivot(PivotWidth pivotWidth, PivotHeight pivotHeight)
        {
            _pivotWidth = pivotWidth;
            _pivotHeight = pivotHeight;
            UpdatePivotPosition();
        }

        public GameObject GameObject => _root.gameObject;
        
        [Button]

        private void UpdatePivotPosition()
        {
            _model.localPosition = PivotLocalPositions();
            _pivot.localPosition = -_model.localPosition;
        }
        
        [Button]

        public void AdjustPivot()
        {
            var isPosMatches = PivotPositionMatches();
            var isScaleNormal = PivotScaleNormal();
            var updatedPivotLocalPosition = PivotLocalPositions();
            var sign = _pivotWidth is (PivotWidth.bottom_right or PivotWidth.top_right) ? 1 : -1;

            if (isPosMatches is { X: true, Y: true, Z: true } && 
                isScaleNormal is { X: false } or { Y: false } or { Z: false })
            {
                _root.localScale = new Vector3(
                    !isScaleNormal.X ? _pivot.localScale.x : _root.localScale.x,
                    !isScaleNormal.Y ? _pivot.localScale.y : _root.localScale.y,
                    !isScaleNormal.Z ? _pivot.localScale.z : _root.localScale.z);

                _root.localPosition = new Vector3(
                    !isScaleNormal.X ? _root.localPosition.x - Shift(_model.localScale.x, _pivot.localScale.x) : _root.localPosition.x,
                    !isScaleNormal.Y ? _root.localPosition.y - Shift(_model.localScale.y, _pivot.localScale.y) : _root.localPosition.y,
                    !isScaleNormal.Z ? _root.localPosition.z - Shift(_model.localScale.z, _pivot.localScale.z) : _root.localPosition.z);
                
                _pivot.localScale = Vector3.one;
            }
            else if (isPosMatches is { X: false } or { Y: false } or { Z: false } && 
                     isScaleNormal is { X: false } or { Y: false } or { Z: false })
            {
                _root.localScale = new Vector3(
                    !isScaleNormal.X ? _pivot.localScale.x : _root.localScale.x,
                    !isScaleNormal.Y ? _pivot.localScale.y : _root.localScale.y,
                    !isScaleNormal.Z ? _pivot.localScale.z : _root.localScale.z);
                
                _root.localPosition = new Vector3(
                    !isScaleNormal.X ? _root.localPosition.x + _pivot.localPosition.x + updatedPivotLocalPosition.x - Shift(_model.localScale.x,_pivot.localScale.x) : _root.localPosition.x,
                    !isScaleNormal.Y ? _root.localPosition.y + _pivot.localPosition.y + updatedPivotLocalPosition.y - Shift(_model.localScale.y, _pivot.localScale.y) : _root.localPosition.y,
                    !isScaleNormal.Z ? _root.localPosition.z + _pivot.localPosition.z + updatedPivotLocalPosition.z - Shift(_model.localScale.z, _pivot.localScale.z) : _root.localPosition.z);
                _pivot.localScale = Vector3.one;
            }
            
            _model.localPosition = updatedPivotLocalPosition;
            _pivot.localPosition = -updatedPivotLocalPosition;
            
            float Shift(float scale, float ratio) => ((scale * ratio) - scale) * 0.5f * sign;
        }

        private (bool X, bool Y, bool Z) PivotPositionMatches() =>
                (Mathf.Approximately(_pivot.localPosition.x + _model.localPosition.x, 0), 
                Mathf.Approximately(_pivot.localPosition.y + _model.localPosition.y, 0),
                Mathf.Approximately(_pivot.localPosition.z + _model.localPosition.z, 0));

        private (bool X, bool Y, bool Z) PivotScaleNormal() =>
            (Mathf.Approximately(_pivot.localScale.x, 1f),
            Mathf.Approximately(_pivot.localScale.y, 1f),
            Mathf.Approximately(_pivot.localScale.z, 1f));
        
        private Vector3 PivotLocalPositions() => _pivotWidth switch
        {
            PivotWidth.bottom_left => new Vector3(_model.localScale.x * 0.5f, PivotAxisY, _model.localScale.z * 0.5f),
            PivotWidth.bottom_right => new Vector3(_model.localScale.x * -0.5f, PivotAxisY, _model.localScale.z * 0.5f),
            PivotWidth.top_left => new Vector3(_model.localScale.x * 0.5f, PivotAxisY, _model.localScale.z * -0.5f),
            PivotWidth.top_right => new Vector3(_model.localScale.x * -0.5f, PivotAxisY, _model.localScale.z * -0.5f),
            PivotWidth.center => new Vector3(0, PivotAxisY, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private float PivotAxisY => _pivotHeight switch
        {
            PivotHeight.top => _model.localScale.y * -0.5f,
            PivotHeight.bottom => _model.localScale.y * 0.5f,
            PivotHeight.center => 0,
            _ => throw new ArgumentOutOfRangeException()
        };

        #region Extended

        public Transform Root => _root;
        public Transform Pivot => _pivot;
        public Transform Model => _model;
        public PivotWidth PWidth => _pivotWidth;
        public PivotHeight PHeight => _pivotHeight;

        #endregion
    }

    public static class PivotTransformStretching
    {
        public static readonly Vector3 Center = new Vector3(0, 0, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Top = new Vector3(0, 0, 1);
        public static readonly Vector3 Bottom = new Vector3(0, 0, -1);
        public static readonly Vector3 TopLeft = new Vector3(-1, 0, 1);
        public static readonly Vector3 TopRight = new Vector3(1, 0, 1);
        public static readonly Vector3 BottomLeft = new Vector3(-1, 0, -1);
        public static readonly Vector3 BottomRight = new Vector3(1, 0, -1);
    }
    public static class Vector3Extensions
    {
        public static (PivotTransform.PivotWidth width, PivotTransform.PivotHeight height) ToPivotTransform(
            this Vector3 value)
        {
            if(pivotTransformMap.TryGetValue(value, out var result))
                return result;

            throw new ArgumentOutOfRangeException();
        }
        
        public static int ToStretchingAnimationId(
            this Vector3 value)
        {
            if(!pivotTransformMap.ContainsKey(value))
                throw new ArgumentOutOfRangeException("key not found");

            return value switch
            {
                { x: 0, y: 0, z: 0 } => AnimationsConstants.Idle,
                { x: not 0, y: 0, z: 0 } => AnimationsConstants.StretchingHorizontal,
                { x: 0, y: 0, z: not 0 } => AnimationsConstants.StretchingVertical,
                { x: not 0, y: 0, z: not 0 } => AnimationsConstants.StretchingTwoSide,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }
        
        private static readonly Dictionary<Vector3, (PivotTransform.PivotWidth, PivotTransform.PivotHeight)> pivotTransformMap = new Dictionary<Vector3, (PivotTransform.PivotWidth, PivotTransform.PivotHeight)>
        {
            { PivotTransformStretching.Center, (PivotTransform.PivotWidth.center, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.Left, (PivotTransform.PivotWidth.top_right, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.Right, (PivotTransform.PivotWidth.top_left, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.Top, (PivotTransform.PivotWidth.bottom_left, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.Bottom, (PivotTransform.PivotWidth.top_left, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.TopLeft, (PivotTransform.PivotWidth.bottom_right, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.TopRight, (PivotTransform.PivotWidth.bottom_left, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.BottomLeft, (PivotTransform.PivotWidth.top_right, PivotTransform.PivotHeight.center) },
            { PivotTransformStretching.BottomRight, (PivotTransform.PivotWidth.top_left, PivotTransform.PivotHeight.center) },
        };
    }
}