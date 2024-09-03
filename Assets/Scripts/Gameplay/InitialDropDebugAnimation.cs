using System;
using Animations;
using Components;
using DG.Tweening;
using Pivots;
using Remainders;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{

    public class AnimationComponent
    {
        public IComponent Component { get; set; }
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }

        private void UpdateMove(float value)
        {
            Component.Position = Vector3.Lerp(From, To, value);
        }
        
        private void UpdateSize(float value)
        {
            Component.Size = Vector3.Lerp(From, To, value);
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
            [HideIf(nameof(UseEase))]
            public AnimationCurve Curve;
        }
    }
    public class InitialDropDebugAnimation : GameplayAnimationBase
    {
        private readonly InitialDropAnimationDebug _settings;
        private readonly AnimationComponent _moveDownAnimation;
        private IAnimationComponent _component;

        public InitialDropDebugAnimation()
        {
            _settings = _animationSettings.InitialDropDebug;
            _moveDownAnimation = new AnimationComponent();
            _sequence.AppendSequence(_moveDownAnimation.MoveTween(_settings.Move));
        }
        public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
        {
            _component = components[0];
            _moveDownAnimation.Component = _component;
            _component.ChangePivot(PivotTransform.PivotWidth.center, PivotTransform.PivotHeight.bottom);
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
            _component.Animation.Play(_settings.FlyDown);
            _sequence.Play(() =>
            {
                _component.Animation.Play(_settings.FlyTouchDown, () => _component.Animation.Play(_settings.Landing, callback));
            });
        }
    }
    
    public class DropAnimationDebug : GameplayAnimationBase
    {
        private readonly Animations.DropAnimationDebug _settings;
        private readonly AnimationComponent _moveDownAnimation;
        private Vector3 _stratching;
        private IAnimationComponent[] _components;

        public DropAnimationDebug()
        {
            _settings = _animationSettings.DropDebug;
            _moveDownAnimation = new AnimationComponent();
            _sequence.AppendSequence(_moveDownAnimation.MoveTween(_settings.Move));
        }
        public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
        {
            _components = components;
            _moveDownAnimation.Component = components[0];
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            _moveDownAnimation.From = targets[0];
            _moveDownAnimation.To = targets[1];
            _stratching = targets[2];
            return this;
        }

        public override void Play(Action callback = null)
        {
            _sequence.Play(() =>
            {
                var count = _components.Length;
                for (int i = 0; i < count; i++)
                {
                    var animation = i switch
                    {
                        0 => _settings.DropHard,
                        1 => _settings.DropMiddle,
                        _ => _settings.DropLight
                    };
                    _components[i].ChangePivot(PivotTransform.PivotWidth.center, PivotTransform.PivotHeight.bottom);
                    _components[i].Animation.Play(animation, i == count - 1 ? Stratching : null);
                }
            });

            void Stratching()
            {
                var pivot = _stratching.ToPivotTransform();
                _components[0].ChangePivot(pivot.width, pivot.height);
                _components[0].Animation.Play(StretchingSettings(), callback);
            }
            
            AnimationBase.Settings StretchingSettings()
            {
                return new AnimationBase.Settings
                {
                    Delay = _settings.Stretching.Delay,
                    Speed = _settings.Stretching.Speed,
                    Id = _stratching.ToStretchingAnimationId()
                };
            }
        }
    }
}