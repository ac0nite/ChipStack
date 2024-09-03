using UnityEngine;

namespace Pivots
{
    public interface IPivotScaling
    {
        bool IsChanged(IPivotExtendedTransform root);
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
        protected readonly IPivotExtendedTransform _child;

        public PivotScaling(IPivotExtendedTransform root, IPivotExtendedTransform child)
        {
            _child = child;
            
            var position = child.Pivot.localPosition;
            
            _from = new Vector3(
                IsLeft(root) ? - position.x - 1 : 1 - position.x,
                position.y,
                IsBottom(root) ? - position.z - 1 : 1 - position.z);

            _to = new Vector3(-position.x, position.y, -position.z);
            
            _t = new Vector3(
                Mathf.InverseLerp(_from.x, _to.x, position.x),
                Mathf.InverseLerp(_from.y, _to.y, position.y),
                Mathf.InverseLerp(_from.z, _to.z, position.z));
            
            _size = Vector3.Scale(root.Pivot.localScale, root.Model.localScale);
            
            _p = new Vector3(
                _size.x * _t.x,
                _size.y * _t.y,
                _size.z * _t.z);
            
            _scaleFactor = new Vector3(
                1 / (1f -  _t.x),
                1 / (1f -  _t.y),
                1 / (1f -  _t.z));
            
            //Debug.Log(this);
        }

        public virtual bool IsChanged(IPivotExtendedTransform root)
        {
            var size = Vector3.Scale(root.Pivot.localScale, root.Model.localScale);
            
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
            
            _child.Pivot.localScale = Vector3.Scale(_scaleFactor, Vector3.one - _t);
        }

        private bool IsBottom(IPivotTransform pivotTransform) => pivotTransform.PWidth is PivotTransform.PivotWidth.bottom_left or PivotTransform.PivotWidth.bottom_right;
        private bool IsLeft(IPivotTransform pivotTransform) => pivotTransform.PWidth is PivotTransform.PivotWidth.top_left or PivotTransform.PivotWidth.bottom_left;
        public override string ToString()
        {
            return $"from:{_from} _to:{_to} _t:{_t} _p:{_p} _scaleFactor:{_scaleFactor} _size:{_size}";
        }
    }
}