using System;
using Animations;
using Blocks;
using UnityEngine;

namespace Gameplay
{
    public class DropAnimation : GameplayAnimationBase
    {
        private readonly Animations.DropAnimation _settings;
        private Block[] _blocks;

        public DropAnimation()
        {
            _settings = _animationSettings.Drop;
        }
        public override GameplayAnimationBase SetBlocks(params Block[] blocks)
        {
            _blocks = blocks;
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            return this;
        }

        public override void Play(Action callback = null)
        {
            var count = _blocks.Length;
            for (int i = 0; i < count; i++)
            {
                var animation = i switch
                {
                    0 => _settings.TopHitAnimation,
                    1 => _settings.MiddleHitAnimation,
                    _ => _settings.BottomHitAnimation
                };
                _blocks[i].View.Animation.Play(animation, i == count - 1 ? callback : null);
            }
        }
    }
}