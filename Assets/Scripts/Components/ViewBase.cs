using System;
using Animations;
using Remainders;
using UnityEngine;

namespace Components
{
    public class ViewBase : MonoBehaviour
    {
        public CustomComponent Component { get; private set; }
        public AnimationBlock Animation { get; private set; }

        private void OnValidate()
        {
            Awake();
        }

        protected virtual void Awake()
        {
            Component = new CustomComponent(transform);
            Animation = new AnimationBlock(Component.Animator);
        }
    }
}