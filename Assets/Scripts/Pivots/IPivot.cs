using Components;
using UnityEngine;

namespace Pivots
{
    public interface IPivot : IComponent
    {
        PivotComponent.WidthAlignment PivotWidthAlignment { get; }
        PivotComponent.HeightAlignment PivotHeightAlignment { get; }
        void SetPivotAlignment(PivotComponent.WidthAlignment widthAlignment, PivotComponent.HeightAlignment heightAlignment);
        void FineTunePivotAlignment();
        void Reset();
    }
    
    public interface IPivotExtended : IPivot
    {
        Transform Root { get; }
        Transform Pivot { get; }
        Transform Model { get; }
    }
}