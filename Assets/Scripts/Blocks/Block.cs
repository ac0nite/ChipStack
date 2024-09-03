using Animations;
using Components;
using Core.Pool;
using Intersections;
using Pivots;
using Remainders;
using UnityEngine;

namespace Blocks
{
    public class Block : IPresenter<BlockView>, IAnimationComponent
    {
        public Block(BlockView view)
        {
            View = view;
            Animation = new AnimationBlock(view.Animator);
        }
        public BlockView View { get; private set; }
    
        public void SendToPool(Poolable<BlockView> pool)
        {
            pool.Release(View);
        }
    
        public Vector3 Position
        {
            get => View.PivotTransform.Position;
            set => View.PivotTransform.Position = value;
        }

        public Vector3 Size
        {
            get => View.PivotTransform.Size;
            set => View.PivotTransform.Size = value;
        }

        public AnimationBlock Animation { get; }

        public void ChangePivot(PivotTransform.PivotWidth pivotWidth, PivotTransform.PivotHeight pivotHeight)
        {
            View.PivotTransform.SetPivot(pivotWidth, pivotHeight);
        }

        public void Enable()
        {
            View.Enable();
        }
        public void ClearAndDisable()
        {
            View.Reset();
            // View.Component.DisablePhysics();
            // View.Component.SetTransformDefault();
            // View.Component.DisableActive();
        }

        public void ChangeTransform(Intersection intersection)
        {
            intersection.ApplyTo(View.PivotTransform);
            //View.Component.EnablePhysics();
        }
    }
}