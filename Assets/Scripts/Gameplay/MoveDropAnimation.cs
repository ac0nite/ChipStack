using System;
using Animations;
using Blocks;
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
        public override GameplayAnimationBase SetBlocks(params Block[] blocks)
        {
            _moving.Move.AnimComponent.SetComponent(blocks[0]);
            _animation = blocks[0].View.Animation;
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