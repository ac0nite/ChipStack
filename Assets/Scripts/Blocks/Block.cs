using Components;
using Core.Pool;
using Intersections;
using UnityEngine;
using RectTransform = Intersections.RectTransform;

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
            get => View.transform.position;
            set
            {
                // Debug.Log($"Change: {value}");
                View.transform.position = value;
            }
        }

        public Vector3 Size
        {
            get => View.transform.localScale;
            set => View.transform.localScale = value;
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

        public void ChangeTransform(RectTransform rectTransform)
        {
            rectTransform.ApplyTo(View.transform);
            View.Component.EnablePhysics();
        }

        public void ChangeTransform(Vector3 position, Vector3 scale)
        {
            Position = position;
            Size = scale;
        }
    }
}