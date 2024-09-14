using Components;
using MEC;
using Pivots;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Remainders
{
    public class RemainderView : ViewBase
    {
        [SerializeField] private PivotComponent rootPivotComponent;
        [SerializeField] private PivotComponent onePivotComponent;
        [SerializeField] private PivotComponent twoPivotComponent;
        public IPivotExtended Root => rootPivotComponent;
        public IPivotExtended One => onePivotComponent;
        public IPivotExtended Two => twoPivotComponent;

        public IPivotsAdjuster PivotsAdjuster
        {
            get
            {
                return _pivotsAdjuster ??= new PivotsAdjuster(rootPivotComponent, onePivotComponent, twoPivotComponent);
            }
        }
        
        private IPivotsAdjuster _pivotsAdjuster;

        public void SetActive(bool oneIsValid, bool twoIsValid)
        {
            onePivotComponent.gameObject.SetActive(oneIsValid);
            twoPivotComponent.gameObject.SetActive(twoIsValid);
        }

        public override void Reset()
        {
            twoPivotComponent.Reset();
            onePivotComponent.Reset();
            rootPivotComponent.Reset();
            
            base.Reset();
        }

#if UNITY_EDITOR
        [Button]
        private void EnablePivotsAdjuster()
        {
            PivotsAdjuster.Enable(Segment.EditorUpdate);
        }
        
        [Button]
        private void DisablePivotsAdjuster()
        {
            PivotsAdjuster.Disable();
        }
#endif
    }
}