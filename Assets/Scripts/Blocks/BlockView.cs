using Components;
using Pivots;
using UnityEngine;

namespace Blocks
{
    [ExecuteInEditMode]
    public class BlockView : ViewBase
    {
        [SerializeField] private PivotTransform _pivotTransform;

        public IPivotTransform PivotTransform => _pivotTransform;
    }
}