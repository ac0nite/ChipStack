using Animations;
using Pivots;
using UnityEngine;

namespace Components
{
    public interface IComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        bool IsActive { get; }
    }

    public interface IAnimationComponent : IComponent
    {
        AnimationBlock Animation { get; }
        void ChangePivot(PivotComponent.WidthAlignment widthAlignment, PivotComponent.HeightAlignment heightAlignment);
    }

    public interface IViewComponent
    {
        public Transform Root { get; }
        public Animator Animator { get; }

        public void Enable();
        public void Disable();
        public void Reset();
    }
    public class BaseComponent : MonoBehaviour, IComponent
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 Size
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public bool IsActive => gameObject.activeSelf;
    }   
}