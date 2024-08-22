using System;
using Animations;
using Blocks;
using Components;
using DG.Tweening;
using Remainders;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{

    public class AnimationComponent
    {
        public IComponent AnimComponent { get; set; }
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }

        private void UpdateMove(float value)
        {
            AnimComponent.Position = Vector3.Lerp(From, To, value);
        }
        
        private void UpdateSize(float value)
        {
            AnimComponent.Size = Vector3.Lerp(From, To, value);
        }

        public Tween MoveTween(Settings settings)
        {
            var tween = DOVirtual
                .Float(0, 1, settings.Duration, UpdateMove)
                .SetDelay(settings.Delay);

            if (settings.UseEase)
                tween.SetEase(settings.Ease);
            else
                tween.SetEase(settings.Curve);
            
            return tween;
        }

        [Serializable]
        public struct Settings
        {
            public float Delay;
            public float Duration;
            public bool UseEase;
            [ShowIf(nameof(UseEase))]
            public Ease Ease;
            [ShowIf(nameof(UseEase), false)]
            public AnimationCurve Curve;
        }
    }
    public class InitialDropDebugAnimation : GameplayAnimationBase
    {
        private readonly InitialDropAnimationDebug _settings;
        private readonly AnimationComponent _moveDownAnimation;
        private AnimationBlock _animation;


        public InitialDropDebugAnimation()
        {
            _settings = _animationSettings.InitialDropDebug;
            _moveDownAnimation = new AnimationComponent();
            _sequence.AppendSequence(_moveDownAnimation.MoveTween(_settings.Move));
        }
        public override GameplayAnimationBase SetBlocks(params Block[] blocks)
        {
            _moveDownAnimation.AnimComponent = blocks[0];
            _moveDownAnimation.AnimComponent.ChangePivot(PivotTransform.PivotWidth.center, PivotTransform.PivotHeight.bottom);
            _animation = blocks[0].View.Animation;
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            _moveDownAnimation.From = _settings.BeginPosition;
            _moveDownAnimation.To = targets[0];
            
            return this;
        }

        public override void Play(Action callback = null)
        {
            _animation.Play(_settings.FlyDown);
            _sequence.Play(() =>
            {
                _animation.Play(_settings.FlyTouchDown, () => _animation.Play(_settings.Landing, callback));
            });
        }
    }
}