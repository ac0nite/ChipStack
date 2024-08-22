using Components;
using Core.Pool;
using Intersections;
using Remainders;
using UnityEngine;

namespace Blocks
{
    public class Block : IPresenter<BlockView>, IComponent
    {
        public BlockView View { get; private set; }
    
        public void SendToPool(Poolable<BlockView> pool)
        {
            pool.Release(View);
        }

        public Block(BlockView view)
        {
            View = view;
        }
    
        public Vector3 Position
        {
            get => View.PivotTransform.Position;
            set
            {
                // Debug.Log($"Change: {value}");
                View.PivotTransform.Position = value;
            }
        }

        public Vector3 Size
        {
            get => View.PivotTransform.Size;
            set => View.PivotTransform.Size = value;
        }

        public void ChangePivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight)
        {
            View.PivotTransform.SetPivot(pivotWidth, pivotHeight);
        }

        public void Enable()
        {
            View.Component.EnableActive();
        }
        public void ClearAndDisable()
        {
            View.Component.DisablePhysics();
            View.Component.SetTransformDefault();
            View.Component.DisableActive();
        }

        public void ChangeTransform(Intersection intersection)
        {
            intersection.ApplyTo(View.PivotTransform);
            //View.Component.EnablePhysics();
        }

        public void ChangeTransform(Vector3 position, Vector3 scale)
        {
            Position = position;
            Size = scale;
        }
    }
}