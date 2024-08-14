using System;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class TweenAnimation : TweenAnimationBase
    {
        public static TweenAnimation CreateInitialDrop(TweenAnimationSettings.InitialDropSettings settings)
        {
            var tween =  CreateMoveAndSize();
            tween.AppendSequence(tween.Move.CreateChangeTween(settings.Move));
            tween.JoinSequence(tween.Size.CreateChangeLoopTween(settings.Size));
            return tween;
        }
        
        private static TweenAnimation CreateOnlyMove()
        {
            return new TweenAnimation
            {
                Move = TweenComponent.UseMove()
            };
        }
        private static TweenAnimation CreateOnlySize()
        {
            return new TweenAnimation
            {
                Size = TweenComponent.UseSize()
            };
        }
        private static TweenAnimation CreateMoveAndSize()
        {
            return new TweenAnimation
            {
                Move = TweenComponent.UseMove(),
                Size = TweenComponent.UseSize()
            };
        }
    }

    [Serializable]
    public class TweenAnimationSettings
    {
        public InitialDropSettings InitialDrop;
        
        [Serializable]
        public class InitialDropSettings
        {
            [Header("MOVE")]
            public Vector3 BeginPosition;
            public TweenComponent.Settings Move;
            [Header("SIZE")]
            public Vector3 ScaleSize;
            public TweenComponent.SettingsLoop Size;
        }
    }
        
    public class TweenAnimationBase
    {
        protected Sequence sequence;
        public TweenComponent Move { get; protected set; } = null;
        public TweenComponent Size { get; protected set; } = null;
            

        protected void AppendSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Append(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        
        protected void JoinSequence(params Tween[] tweens)
        {
            sequence ??= DOTween.Sequence();
            
            foreach (var tween in tweens)
                sequence.Join(tween);
            
            sequence
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }
        
        public void Play(Action callback)
        {
            sequence
                ?.OnComplete(() =>
                {
                    Debug.Log($"Tween completed!");
                    callback?.Invoke();
                })
                .Restart(false);
        }

        public void SetComponent(IComponent component)
        {
            Move?.AnimComponent.SetComponent(component);
            Size?.AnimComponent.SetComponent(component);
        }
    }
    public class TweenComponent
    {
        public static TweenComponent UseMove() => new TweenComponent(new AnimationComponent(PropertyWrapper.Move));
        public static TweenComponent UseSize() => new TweenComponent(new AnimationComponent(PropertyWrapper.Size));

        public TweenComponent(AnimationComponent component)
        {
            AnimComponent = component;
        }

        public AnimationComponent AnimComponent { get; }

        // public static TweenAnimation CreateMoveDownAnimation(Settings settings)
        // {
        //     var tween = new TweenAnimation();
        //     tween.AppendSequence(tween.CreateChangeTween(settings));
        //     return tween;
        // }
        //
        // public static TweenAnimation CreateMoveRemainderAnimation(Settings move, Settings elastic)
        // {
        //     var tween = new TweenAnimation();
        //     tween.AppendSequence(tween.CreateChangeTween(move).OnComplete(tween.MoveComponent.NextParam));
        //     tween.JoinSequence(tween.CreateChangeLoopTween(elastic));
        //     tween.AppendSequence(tween.CreateChangeTween(move));
        //     return tween;
        // }

        public Tweener CreateChangeTween(Settings settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenChangeUpdate)
            .SetDelay(settings.Delay)
            .SetEase(settings.Ease)
            .OnComplete(() => Debug.Log("Move completed!"));
        
        public Tweener CreateChangeLoopTween(SettingsLoop settings) => DOVirtual
            .Float(0f, 1f, settings.Duration, TweenChangeUpdate)
            .SetDelay(settings.Delay)
            .SetEase(settings.Ease)
            .SetLoops(settings.Loops, settings.LoopType)
            .OnComplete(() => Debug.Log("Size completed!"));

        private void TweenChangeUpdate(float value)
        {
            AnimComponent.Value = Vector3.Lerp(AnimComponent.From, AnimComponent.To, value);
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

            public AnimationComponent(PropertyWrapper propertyComponent)
            {
                _property = propertyComponent;
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
        }
        public class PropertyWrapper
        {
            public static PropertyWrapper Move = new PropertyWrapper((c) => c.Position, (c, v) => c.Position = v);
            public static PropertyWrapper Size = new PropertyWrapper((c) => c.Size, (c, v) => c.Size = v);
            
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
            public Ease Ease;
        }
        
        [Serializable]
        public struct SettingsLoop
        {
            public float Delay;
            public float Duration;
            public Ease Ease;
            public int Loops;
            public LoopType LoopType;
        }
        
        public static Vector3 ScaledVector(Vector3 vector, Vector3 scale) => new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
    }
}