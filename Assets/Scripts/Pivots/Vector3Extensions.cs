using System;
using System.Collections.Generic;
using Animations;
using UnityEngine;

namespace Pivots
{
    public static class Vector3Extensions
    {
        public static (PivotComponent.WidthAlignment width, PivotComponent.HeightAlignment height) ToPivotAlignment(
            this Vector3 value)
        {
            if(pivotTransformMap.TryGetValue(value, out var result))
                return result;

            throw new ArgumentOutOfRangeException();
        }
        
        public static int ToStretchingAnimationId(
            this Vector3 value)
        {
            if(!pivotTransformMap.ContainsKey(value))
                throw new ArgumentOutOfRangeException("key not found", nameof(value), null);

            return value switch
            {
                { x: 0, y: 0, z: 0 } => AnimationsConstants.Idle,
                { x: not 0, y: 0, z: 0 } => AnimationsConstants.StretchingHorizontal,
                { x: 0, y: 0, z: not 0 } => AnimationsConstants.StretchingVertical,
                { x: not 0, y: 0, z: not 0 } => AnimationsConstants.StretchingTwoSide,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }
        
        private static readonly Dictionary<Vector3, (PivotComponent.WidthAlignment, PivotComponent.HeightAlignment)> pivotTransformMap = new Dictionary<Vector3, (PivotComponent.WidthAlignment, PivotComponent.HeightAlignment)>
        {
            { PivotTransformStretching.Center, (PivotComponent.WidthAlignment.center, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.Left, (PivotComponent.WidthAlignment.top_right, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.Right, (PivotComponent.WidthAlignment.top_left, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.Top, (PivotComponent.WidthAlignment.bottom_left, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.Bottom, (PivotComponent.WidthAlignment.top_left, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.TopLeft, (PivotComponent.WidthAlignment.bottom_right, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.TopRight, (PivotComponent.WidthAlignment.bottom_left, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.BottomLeft, (PivotComponent.WidthAlignment.top_right, PivotComponent.HeightAlignment.center) },
            { PivotTransformStretching.BottomRight, (PivotComponent.WidthAlignment.top_left, PivotComponent.HeightAlignment.center) },
        };
    }
    
    public static class PivotTransformStretching
    {
        public static readonly Vector3 Center = new Vector3(0, 0, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Top = new Vector3(0, 0, 1);
        public static readonly Vector3 Bottom = new Vector3(0, 0, -1);
        public static readonly Vector3 TopLeft = new Vector3(-1, 0, 1);
        public static readonly Vector3 TopRight = new Vector3(1, 0, 1);
        public static readonly Vector3 BottomLeft = new Vector3(-1, 0, -1);
        public static readonly Vector3 BottomRight = new Vector3(1, 0, -1);
    }
}