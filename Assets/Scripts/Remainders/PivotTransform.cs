using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Remainders
{
    [Serializable]
    public class PivotTransform
    {
        public enum PivotWidth { center, top_left, top_right, bottom_left, bottom_right }
        public enum PivotHeight { center, top, bottom }
        
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _model;
        [Space]
        [SerializeField] private PivotWidth _pivotWidth;
        [SerializeField] private PivotHeight _pivotHeight;

        [OdinSerialize]
        public Vector3 Position
        {
            get => _root.position;
            set => _root.position = value;
        }
        
        [OdinSerialize]
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
            var y = _pivotHeight switch
            {
                PivotHeight.top => _model.localScale.y * -0.5f,
                PivotHeight.bottom => _model.localScale.y * 0.5f,
                PivotHeight.center => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var localPosition = _pivotWidth switch
            {
                PivotWidth.bottom_left => new Vector3(_model.localScale.x * 0.5f, y, _model.localScale.z * 0.5f),
                PivotWidth.bottom_right => new Vector3(_model.localScale.x * -0.5f, y, _model.localScale.z * 0.5f),
                PivotWidth.top_left => new Vector3(_model.localScale.x * 0.5f, y, _model.localScale.z * -0.5f),
                PivotWidth.top_right => new Vector3(_model.localScale.x * -0.5f, y, _model.localScale.z * -0.5f),
                PivotWidth.center => new Vector3(0, y, 0),
                _ => throw new ArgumentOutOfRangeException()
            };

            _model.localPosition = localPosition;
            _pivot.localPosition = -localPosition;
        }
    }
}