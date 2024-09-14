using System;
using Core.Utils.Extended;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pivots
{
    public class PivotComponent : MonoBehaviour, IPivotExtended
    {
        public enum WidthAlignment { center, top_left, top_right, bottom_left, bottom_right }
        public enum HeightAlignment { center, top, bottom }
        
        [SerializeField] private Transform _rootTransform;
        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private Transform _modelTransform;
        [Space]
        [SerializeField] public WidthAlignment _widthAlignment;
        [SerializeField] public HeightAlignment _heightAlignment;
        
        public Vector3 Position
        {
            get => _rootTransform.position;
            set => _rootTransform.position = value;
        }
        
        public Vector3 Size
        {
            get => _modelTransform.localScale;
            set
            {
                _modelTransform.localScale = value;
                UpdatePivotPositionBasedOnAlignment();
            }
        }
        
        public void SetPivotAlignment(WidthAlignment widthAlignment, HeightAlignment heightAlignment)
        {
            _widthAlignment = widthAlignment;
            _heightAlignment = heightAlignment;
            UpdatePivotPositionBasedOnAlignment();
        }

        public GameObject GameObject => _rootTransform.gameObject;
        
        [Button]
        private void UpdatePivotPositionBasedOnAlignment()
        {
            _modelTransform.localPosition = CalculatePivotLocalPosition();
            _pivotTransform.localPosition = -_modelTransform.localPosition;
        }
        
        [Button]
        public void FineTunePivotAlignment()
        {
            var isPosMatches = PivotPositionMatches();
            var isScaleNormal = PivotScaleNormal();
            var updatedPivotLocalPosition = CalculatePivotLocalPosition();
            var sign = _widthAlignment is (WidthAlignment.bottom_right or WidthAlignment.top_right) ? 1 : -1;
            var updatedRootScale = _rootTransform.localScale.ScaleProportionallyTo(_pivotTransform.localScale);

            if (isPosMatches is { X: true, Y: true, Z: true } && 
                isScaleNormal is { X: false } or { Y: false } or { Z: false })
            {
                var oldScale = _rootTransform.localScale;
                
                _rootTransform.localScale = new Vector3(
                    !isScaleNormal.X ? updatedRootScale.x : _rootTransform.localScale.x,
                    !isScaleNormal.Y ? updatedRootScale.y : _rootTransform.localScale.y,
                    !isScaleNormal.Z ? updatedRootScale.z : _rootTransform.localScale.z);

                _rootTransform.localPosition = new Vector3(
                    !isScaleNormal.X ? _rootTransform.localPosition.x - Shift(_modelTransform.localScale.x, _pivotTransform.localScale.x, oldScale.x) : _rootTransform.localPosition.x,
                    !isScaleNormal.Y ? _rootTransform.localPosition.y - Shift(_modelTransform.localScale.y, _pivotTransform.localScale.y, oldScale.y) : _rootTransform.localPosition.y,
                    !isScaleNormal.Z ? _rootTransform.localPosition.z - Shift(_modelTransform.localScale.z, _pivotTransform.localScale.z, oldScale.z) : _rootTransform.localPosition.z);
                
                _pivotTransform.localScale = Vector3.one;
            }
            else if (isPosMatches is { X: false } or { Y: false } or { Z: false } && 
                     isScaleNormal is { X: false } or { Y: false } or { Z: false })
            {
                var oldScale = _rootTransform.localScale;
                
                _rootTransform.localScale = new Vector3(
                    !isScaleNormal.X ? updatedRootScale.x : _rootTransform.localScale.x,
                    !isScaleNormal.Y ? updatedRootScale.y : _rootTransform.localScale.y,
                    !isScaleNormal.Z ? updatedRootScale.z : _rootTransform.localScale.z);
                
                _rootTransform.localPosition = new Vector3(
                    !isScaleNormal.X ? _rootTransform.localPosition.x + (_pivotTransform.localPosition.x + updatedPivotLocalPosition.x) * oldScale.x - Shift(_modelTransform.localScale.x,_pivotTransform.localScale.x, oldScale.x) : _rootTransform.localPosition.x,
                    !isScaleNormal.Y ? _rootTransform.localPosition.y + (_pivotTransform.localPosition.y + updatedPivotLocalPosition.y) * oldScale.y - Shift(_modelTransform.localScale.y, _pivotTransform.localScale.y, oldScale.y) : _rootTransform.localPosition.y,
                    !isScaleNormal.Z ? _rootTransform.localPosition.z + (_pivotTransform.localPosition.z + updatedPivotLocalPosition.z) * oldScale.z - Shift(_modelTransform.localScale.z, _pivotTransform.localScale.z, oldScale.z) : _rootTransform.localPosition.z);
                    
                _pivotTransform.localScale = Vector3.one;
            }
            
            _modelTransform.localPosition = updatedPivotLocalPosition;
            _pivotTransform.localPosition = -updatedPivotLocalPosition;
            
            return;

            float Shift(float model, float pivot, float rootScale) => ((model * pivot) - model) * 0.5f * sign * rootScale;
        }

        [Button]
        public void Reset()
        {
            UpdatePivotPositionBasedOnAlignment();
            
            _pivotTransform.localScale = Vector3.one;
            _rootTransform.localScale = Vector3.one;
            _rootTransform.localPosition = Vector3.zero;
        }

        public bool IsActive => _rootTransform.gameObject.activeSelf;

        private (bool X, bool Y, bool Z) PivotPositionMatches() =>
                (Mathf.Approximately(_pivotTransform.localPosition.x + _modelTransform.localPosition.x, 0), 
                Mathf.Approximately(_pivotTransform.localPosition.y + _modelTransform.localPosition.y, 0),
                Mathf.Approximately(_pivotTransform.localPosition.z + _modelTransform.localPosition.z, 0));

        private (bool X, bool Y, bool Z) PivotScaleNormal() =>
            (Mathf.Approximately(_pivotTransform.localScale.x, 1f),
            Mathf.Approximately(_pivotTransform.localScale.y, 1f),
            Mathf.Approximately(_pivotTransform.localScale.z, 1f));
        
        private Vector3 CalculatePivotLocalPosition() => _widthAlignment switch
        {
            WidthAlignment.bottom_left => new Vector3(_modelTransform.localScale.x * 0.5f, PivotAxisY, _modelTransform.localScale.z * 0.5f),
            WidthAlignment.bottom_right => new Vector3(_modelTransform.localScale.x * -0.5f, PivotAxisY, _modelTransform.localScale.z * 0.5f),
            WidthAlignment.top_left => new Vector3(_modelTransform.localScale.x * 0.5f, PivotAxisY, _modelTransform.localScale.z * -0.5f),
            WidthAlignment.top_right => new Vector3(_modelTransform.localScale.x * -0.5f, PivotAxisY, _modelTransform.localScale.z * -0.5f),
            WidthAlignment.center => new Vector3(0, PivotAxisY, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private float PivotAxisY => _heightAlignment switch
        {
            HeightAlignment.top => _modelTransform.localScale.y * -0.5f,
            HeightAlignment.bottom => _modelTransform.localScale.y * 0.5f,
            HeightAlignment.center => 0,
            _ => throw new ArgumentOutOfRangeException()
        };

        #region Extended

        public Transform Root => _rootTransform;
        public Transform Pivot => _pivotTransform;
        public Transform Model => _modelTransform;
        public WidthAlignment PivotWidthAlignment => _widthAlignment;
        public HeightAlignment PivotHeightAlignment => _heightAlignment;

        #endregion
    }
}