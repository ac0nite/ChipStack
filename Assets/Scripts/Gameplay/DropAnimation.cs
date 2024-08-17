using System;
using Animations;
using Blocks;
using UnityEngine;

namespace Gameplay
{
    public class DropAnimation : GameplayAnimationBase
    {
        private readonly TweenAnimation _moving;
        private readonly Animations.DropAnimation _settings;
        private Block[] _blocks;

        public DropAnimation()
        {
            _settings = _animationSettings.Drop;
            _moving = TweenAnimation.CreateSimpleMove(_settings.Move);
        }
        public override GameplayAnimationBase SetBlocks(params Block[] blocks)
        {
            _moving.Move.AnimComponent.SetComponent(blocks[0]);
            _blocks = blocks;
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            _moving.Move.AnimComponent.UpdateParams(targets[0]);
            return this;
        }

        public override void Play(Action callback = null)
        {
            _moving.Play(() =>
            {
                _blocks[0].View.Animation.Play(_settings.TopHitAnimation);
                _blocks[1]?.View.Animation.Play(_settings.MiddleHitAnimation);
                _blocks[2]?.View.Animation.Play(_settings.BottomHitAnimation, callback);
            });
        }
    }
}