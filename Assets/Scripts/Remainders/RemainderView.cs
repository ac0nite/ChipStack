using Components;
using UnityEngine;

namespace Remainders
{
    public class RemainderView : ViewBase
    {
        [SerializeField] private PivotTransform _oneRemainder;
        [SerializeField] private PivotTransform _twoRemainder;
        public PivotTransform OneRemainder => _oneRemainder;
        public PivotTransform TwoRemainder => _twoRemainder;
    }
}