using System;
using Animations;
using Components;
using UnityEngine;

namespace Gameplay
{
    public class MoveDropAnimation : GameplayAnimationBase
    {
        private readonly Animations.DropAnimation _settings;
        private readonly TweenAnimation _moving;
        private AnimationBlock _animation;

        public MoveDropAnimation()
        {
            _settings = _animationSettings.Drop;
            _moving = TweenAnimation.CreateSimpleMove(_settings.Move);
        }
        public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
        {
            _moving.Move.AnimComponent.SetComponent(components[0]);
            _animation = components[0].Animation;
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            _moving.Move.AnimComponent.UpdateParams(targets[0]);
            return this;
        }

        public override void Play(Action callback = null)
        {
            _animation.Play(_settings.DropMoveAnimation);
            _moving.Play(callback);
        }
    }
}