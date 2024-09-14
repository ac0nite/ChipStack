using Components;
using Pivots;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blocks
{
    [ExecuteInEditMode]
    public class BlockView : ViewBase
    {
        [SerializeField] private PivotComponent pivotComponent;

        public IPivotExtended PivotComponent => pivotComponent;
    }
}