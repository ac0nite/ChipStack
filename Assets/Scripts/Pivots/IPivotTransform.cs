using Components;
using UnityEngine;

namespace Pivots
{
    public interface IPivotTransform : IComponent
    {
        PivotTransform.PivotWidth PWidth { get; }
        PivotTransform.PivotHeight PHeight { get; }
        void SetPivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight);
        void AdjustPivot();
    }
    
    public interface IPivotExtendedTransform : IPivotTransform
    {
        Transform Root { get; }
        Transform Pivot { get; }
        Transform Model { get; }
    }
}