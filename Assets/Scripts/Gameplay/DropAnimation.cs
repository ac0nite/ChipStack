using System;
using Animations;
using Blocks;
using Components;
using UnityEngine;

namespace Gameplay
{
    public class DropAnimation : GameplayAnimationBase
    {
        private readonly Animations.DropAnimation _settings;
        private IAnimationComponent[] _components;

        public DropAnimation()
        {
            _settings = _animationSettings.Drop;
        }
        public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
        {
            _components = components;
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            return this;
        }

        public override void Play(Action callback = null)
        {
            var count = _components.Length;
            for (int i = 0; i < count; i++)
            {
                var animation = i switch
                {
                    0 => _settings.TopHitAnimation,
                    1 => _settings.MiddleHitAnimation,
                    _ => _settings.BottomHitAnimation
                };
                _components[i].Animation.Play(animation, i == count - 1 ? callback : null);
            }
        }
    }
}