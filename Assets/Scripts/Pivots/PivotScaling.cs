using Core.Utils.Extended;
using UnityEngine;

namespace Pivots
{
    public interface IPivotScaling
    {
        bool IsChanged(IPivotExtended root);
        void UpdatePositionAndScale();
    }
    
    public class PivotScaling : IPivotScaling
    {
        private readonly Vector3 _from;
        private readonly Vector3 _to;
        private readonly Vector3 _p;
        private readonly Vector3 _scaleFactor;
        private Vector3 _t;
        private Vector3 _size;
        protected readonly IPivotExtended _child;

        public PivotScaling(IPivotExtended root, IPivotExtended child)
        {
            _child = child;
            
            var position = child.Pivot.localPosition;
            
            _to = new Vector3(-position.x, position.y, -position.z);
            
            var ratio = Vector3.one.Divide(child.Root.localScale);
            
            _from = new Vector3(
                IsLeft(root) ? _to.x - ratio.x : ratio.x - _to.x,
                position.y,
                IsBottom(root) ? _to.z - ratio.z : ratio.z - _to.z);
            
            _t = new Vector3(
                Mathf.InverseLerp(_from.x, _to.x, position.x),
                Mathf.InverseLerp(_from.y, _to.y, position.y),
                Mathf.InverseLerp(_from.z, _to.z, position.z));
            
            _size = CalcSize(root);
            
            _p = new Vector3(
                _size.x * _t.x,
                _size.y * _t.y,
                _size.z * _t.z);
            
            _scaleFactor = new Vector3(
                1 / (1f -  _t.x),
                1 / (1f -  _t.y),
                1 / (1f -  _t.z));
            
            // Debug.Log($"Initial: {this}");
        }

        private static Vector3 CalcSize(IPivotExtended root)
        {
            return Vector3.Scale(root.Pivot.localScale, root.Model.localScale);
        }

        public virtual bool IsChanged(IPivotExtended root)
        {
            if (!_child.IsActive) 
                return false;

            var size = CalcSize(root);
            
            if (Mathf.Approximately(size.x, _size.x) &&
                Mathf.Approximately(size.y, _size.y) &&
                Mathf.Approximately(size.z, _size.z))
                return false;
            
            _size = size;
            _t = new Vector3(_p.x / _size.x, _p.y / _size.y, _p.z / _size.z);
            
            return true;
        }

        public virtual void UpdatePositionAndScale()
        {
            _child.Pivot.localPosition = new Vector3(
                Mathf.Lerp(_from.x, _to.x, _t.x),
                Mathf.Lerp(_from.y, _to.y, _t.y),
                Mathf.Lerp(_from.z, _to.z, _t.z));
            
            _child.Pivot.localScale = new Vector3(
                (1 - _t.x) * _scaleFactor.x, 
                (1 - _t.y) * _scaleFactor.y, 
                (1 - _t.z) * _scaleFactor.z);
        }

        private static bool IsBottom(IPivot pivot) => pivot.PivotWidthAlignment is PivotComponent.WidthAlignment.bottom_left or PivotComponent.WidthAlignment.bottom_right;
        private static bool IsLeft(IPivot pivot) => pivot.PivotWidthAlignment is PivotComponent.WidthAlignment.top_left or PivotComponent.WidthAlignment.bottom_left;
        public override string ToString()
        {
            return $"from:{_from} _to:{_to} _t:{_t} _p:{_p} _sf:{_scaleFactor} _s:{_size} => p:{_child.Pivot.localPosition} s:{_child.Pivot.localScale}";
        }
    }
}