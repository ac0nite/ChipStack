using System;
using Animations;
using Components;
using DG.Tweening;
using Pivots;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{

    public class AnimationComponent
    {
        private bool _isLog;
        private object _messagePrefix;
        public IComponent Component { get; set; }
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }

        private void UpdateMove(float value)
        {
            // if (_isLog)
            //     Debug.Log($"{_messagePrefix} From: {From} To: {To} Progress: {value}");

            // if (!From.HasValue || value == 0)
            // {
            //     From = Component.Position;
            //     Debug.Log($"Internal Initialize Update: {_messagePrefix} From: {From} To: {To} Progress: {value}");
            // }
            // if(value == 0) return;
            
            Component.Position = Vector3.Lerp(From, To, value);
        }
        
        private void UpdateSize(float value)
        {
            // From ??= Component.Size;
            Component.Size = Vector3.Lerp(From, To, value);
        }

        public Tween MoveTween(Settings settings)
        {
            var tween = DOVirtual
                .Float(0, 1, settings.Duration, UpdateMove)
                .SetDelay(settings.Delay)
                //.OnStart(InternalStart)
                //.OnPlay(InternalPlay)
                //.OnRewind(InternalRewind)
                //.OnStepComplete(InternalCompleted)
                .SetAutoKill(false)
                .SetRecyclable(true);
            
            if (settings.UseEase)
                tween.SetEase(settings.Ease);
            else
                tween.SetEase(settings.Curve);
            
            return tween;
        }

        private void InternalRewind()
        {
            if (_isLog) Debug.Log($"Internal Rewind: {_messagePrefix} From: {From}");
        }

        private void InternalPlay()
        {
            if (_isLog) Debug.Log($"Internal Play: {_messagePrefix} From: {From}");
        }

        private void InternalStart()
        {
            if (_isLog) Debug.Log($"Internal Start: {_messagePrefix} From: {From}");
        }

        public void SetLog(string prefix)
        {
            _isLog = true;
            _messagePrefix = prefix;
        }

        private void InternalCompleted()
        {
            //From = null;
            if (_isLog) Debug.Log($"Internal Completed: {_messagePrefix} From: {From}");
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
            _component.ChangePivot(PivotComponent.WidthAlignment.center, PivotComponent.HeightAlignment.bottom);
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
                    _components[i].ChangePivot(PivotComponent.WidthAlignment.center, PivotComponent.HeightAlignment.bottom);
                    _components[i].Animation.Play(animation, i == count - 1 ? Stratching : null);
                }
            });

            void Stratching()
            {
                var pivot = _stratching.ToPivotAlignment();
                _components[0].ChangePivot(pivot.width, pivot.height);
                
                Debug.Log($"Stratching: {_stratching} Pivot: {pivot}");
                
                //callback?.Invoke();
                _components[0].Animation.PlayWithTransitionIdle(StretchingSettings(), callback);
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
    
    public class RemainderAnimationDebug : GameplayAnimationBase
    {
        private readonly AnimationComponent _moveAnimation;
        private readonly AnimationComponent _downAnimation;

        public RemainderAnimationDebug()
        {
            var settings = _animationSettings.RemainderDebug;
            _moveAnimation = new AnimationComponent();
            _downAnimation = new AnimationComponent();
            
            _moveAnimation.SetLog("Move");
            _downAnimation.SetLog("Down");
            
            _sequence.AppendSequence(_moveAnimation.MoveTween(settings.Move), _downAnimation.MoveTween(settings.Down));
        }
        public override GameplayAnimationBase SetComponents(params IAnimationComponent[] components)
        {
            _moveAnimation.Component = components[0];
            _downAnimation.Component = components[0];
            return this;
        }

        public override GameplayAnimationBase SetParams(params Vector3[] targets)
        {
            Debug.Log($"From: {_moveAnimation.Component.Position} To: {targets[0]} To: {targets[1]}");
            
            _moveAnimation.From = _moveAnimation.Component.Position;
            _moveAnimation.To = targets[0];
            _downAnimation.From = targets[0];
            _downAnimation.To = targets[1];
            return this;
        }

        public override void Play(Action callback = null)
        {
            Time.timeScale = 1f;
            _sequence.Play(() =>
            {
                Time.timeScale = 1f;
                callback?.Invoke();
            });
        }
    }
}