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
            get => View.PivotComponent.Position;
            set => View.PivotComponent.Position = value;
        }

        public Vector3 Size
        {
            get => View.PivotComponent.Size;
            set => View.PivotComponent.Size = value;
        }

        public bool IsActive => View.PivotComponent.IsActive;

        public AnimationBlock Animation { get; }

        public void ChangePivot(PivotComponent.WidthAlignment widthAlignment, PivotComponent.HeightAlignment heightAlignment)
        {
            View.PivotComponent.SetPivotAlignment(widthAlignment, heightAlignment);
        }

        public void Enable()
        {
            View.Enable();
        }
        public void ClearAndDisable()
        {
            View.Reset();
        }

        public void ChangeTransform(Intersection intersection)
        {
            intersection.ApplyTo(View.PivotComponent);
        }
    }
}