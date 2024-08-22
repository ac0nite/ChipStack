using System;
using System.Collections.Generic;
using Components;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Animations
{
    public class TweenComponent
    {
        public static TweenComponent CreateMoveTween() => new TweenComponent(new AnimationComponent(PropertyWrapper.CreateMove()));
        public static TweenComponent CreateSizeTween() => new TweenComponent(new AnimationComponent(PropertyWrapper.CreateSize()));

        private TweenComponent(AnimationComponent component)
        {
            AnimComponent = component;
        }

        public AnimationComponent AnimComponent { get; }

        public Tweener CreateChangeTween(Settings settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenChangeUpdate)
            .SetDelay(settings.Delay)
            .SetEase(settings.Ease)
            .OnComplete(AnimComponent.NextParams);
        
        public Tweener CreateChangeLoopTween(SettingsLoop settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenChangeUpdate)
            .SetDelay(settings.Delay)
            .SetEase(settings.Ease)
            .SetLoops(settings.Loops, settings.LoopType)
            .OnComplete(AnimComponent.NextParams);

        private void TweenChangeUpdate(float value)
        {
            AnimComponent.Value = Vector3.Lerp(AnimComponent.From, AnimComponent.To, value);
        }
        
        public interface IAnimationTarget
        {
            public Vector3 Value(Vector3 original);
        }
        
        public class AnimationTargetBase : IAnimationTarget
        {
            protected readonly Vector3 _value;

            protected AnimationTargetBase(Vector3 value)
            {
                _value = value;
            }
            public virtual Vector3 Value(Vector3 original) => _value;
        }

        public class AnimationTarget : AnimationTargetBase
        {
            public AnimationTarget(Vector3 value) : base(value)
            {
            }
        }
        
        public class AnimationScaledTarget : AnimationTargetBase
        {
            public AnimationScaledTarget(Vector3 value) : base(value)
            {
            }

            public override Vector3 Value(Vector3 original)
            {
                return new Vector3(original.x * _value.x, original.y * _value.y, original.z * _value.z);
            }
        }

        public class AnimationComponent
        {
            private readonly PropertyWrapper _property;
            private IComponent _component;
            
            public Vector3 Value
            {
                get => _property.Getter(_component);
                set => _property.Setter(_component, value);
            }

            public Vector3 From { get; private set; }
            public Vector3 To { get; private set; }

            private Queue<IAnimationTarget> _to;

            public AnimationComponent(PropertyWrapper propertyComponent)
            {
                _property = propertyComponent;
                _to = new Queue<IAnimationTarget>(5);
            }

            public void SetComponent(IComponent component)
            {
                _component = component;
            }
            
            public void UpdateParams(Vector3 from, Vector3 to)
            {
                From = from;
                To = to;
            }
            
            public void UpdateParamsDebug(Vector3 from, params IAnimationTarget[] to)
            {
                to.ForEach(_to.Enqueue);
                
                From = from;
                To = _to.Dequeue().Value(From);
                
                Debug.Log($"From: {From} To: {To} LenTo:{to.Length}");
            }
            
            public void UpdateParamsDebug(params IAnimationTarget[] to)
            {
                to.ForEach(_to.Enqueue);
                
                From = Value;
                To = _to.Dequeue().Value(From);
                
                Debug.Log($"From: {From} To: {To} LenTo:{to.Length}");
            }
            
            public void UpdateParams(Vector3 to)
            {
                From = Value;
                To = to;
            }
            
            public void UpdateScaledParams(Vector3 toScaled)
            {
                From = Value;
                To = ScaledVector(From, toScaled);
            }

            public void NextParams()
            {
                Debug.Log($"Next params");
                To = _to.Dequeue().Value(From);
            }
        }
        public class PropertyWrapper
        {
            public static PropertyWrapper CreateMove() => new PropertyWrapper((c) => c.Position, (c, v) => c.Position = v);
            public static PropertyWrapper CreateSize() => new PropertyWrapper((c) => c.Size, (c, v) => c.Size = v);
            
            private Func<IComponent, Vector3> _getter;
            private Action<IComponent, Vector3> _setter;

            public PropertyWrapper(Func<IComponent, Vector3> getter, Action<IComponent, Vector3> setter)
            {
                _getter = getter;
                _setter = setter;
            }

            public Vector3 Getter(IComponent component) => _getter(component);
            public void Setter(IComponent component, Vector3 value) => _setter(component, value);
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
        
        [Serializable]
        public struct SettingsLoop
        {
            public float Delay;
            public float Duration;
            public Ease Ease;
            public int Loops;
            public LoopType LoopType;
            public AnimationCurve Curve;
        }
        
        public static Vector3 ScaledVector(Vector3 vector, Vector3 scale) => new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
    }
    
    public static class Vector3Extensions
    {
        public static TweenComponent.AnimationTarget AsAnimationTarget(this Vector3 value) => new TweenComponent.AnimationTarget(value);
        public static TweenComponent.AnimationScaledTarget AsAnimationScaledTarget(this Vector3 value) => new TweenComponent.AnimationScaledTarget(value);
    }
}