using Components;
using Pivots;
using UnityEngine;

namespace Remainders
{
    public class RemainderView : ViewBase
    {
        [SerializeField] private PivotTransform _rootPivotTransform;
        [SerializeField] private PivotTransform _onePivotTransform;
        [SerializeField] private PivotTransform _twoPivotTransform;
        [SerializeField] private PivotsScaling _pivotsScaling;
        public IPivotTransform Root => _rootPivotTransform;
        public IPivotTransform One => _onePivotTransform;
        public IPivotTransform Two => _twoPivotTransform;
        public IPivotsScaling PivotsScaling => _pivotsScaling;

        protected override void Awake()
        {
            base.Awake();
            _pivotsScaling.SetParams(_rootPivotTransform, _onePivotTransform, _twoPivotTransform);
        }

        public void SetActive(bool oneIsValid, bool twoIsValid)
        {
            _onePivotTransform.gameObject.SetActive(oneIsValid);
            _twoPivotTransform.gameObject.SetActive(twoIsValid);
        }
    }
}