using Core.Utils.Extended;
using UnityEngine;

namespace Pivots
{
    public class PivotCroppedScaling : PivotScaling
    {
        private readonly Vector3 _defaultPosition;
        private readonly Transform _rootPivot;
        private readonly CroppedAxisDelegate _croppedAxis;

        public delegate void CroppedAxisDelegate();

        public PivotCroppedScaling(IPivotExtendedTransform root, IPivotExtendedTransform child, CroppedAxisDelegate cropped) 
            : base(root, child)
        {
            CroppedAxisXDelegate ??= UpdateCroppedAxisXPositionAndScale;
            CroppedAxisZDelegate ??= UpdateCroppedAxisZPositionAndScale;
            
            _defaultPosition = child.Pivot.localPosition;
            _rootPivot = root.Pivot;

            _croppedAxis = cropped;
        }

        public static CroppedAxisDelegate CroppedAxisXDelegate { get; private set; }
        public static CroppedAxisDelegate CroppedAxisZDelegate { get; private set; }

        public override void UpdatePositionAndScale()
        {
            base.UpdatePositionAndScale();
            _croppedAxis?.Invoke();
        }

        private void UpdateCroppedAxisXPositionAndScale()
        {
            _child.Pivot.localPosition = _child.Pivot.localPosition.SetX(_defaultPosition.x);
            _child.Pivot.localScale = _child.Pivot.localScale.SetX(1f/_rootPivot.localScale.x);
        }
        
        private void UpdateCroppedAxisZPositionAndScale()
        {
            _child.Pivot.localPosition = _child.Pivot.localPosition.SetZ(_defaultPosition.z);
            _child.Pivot.localScale = _child.Pivot.localScale.SetZ(1f/_rootPivot.localScale.z);
        }
    }
}