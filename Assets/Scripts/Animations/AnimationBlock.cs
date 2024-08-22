using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Animations
{
    public class AnimationBlock : AnimationBase
    {
        public AnimationBlock(Animator animator) : base(animator)
        {
        }
    }

    public static class AnimatorTiming
    {
        private static readonly List<int> _animatorsId = new();
        private static readonly Dictionary<int, float> _timings = new ();

        public static void UpdateTiming(Animator animator)
        {
            if(_animatorsId.Contains(Animator.StringToHash(animator.name))) return;
            _animatorsId.Add(Animator.StringToHash(animator.name));
            var clips = animator.runtimeAnimatorController.animationClips;
            clips.ForEach(clip =>
            {
                _timings[Animator.StringToHash(clip.name)] = clip.length;
                Debug.Log($"{clip.name}:{Animator.StringToHash(clip.name)} - {clip.length}");
            });
        }
        
        public static float GetTiming(int id) => _timings[id];
    }

    public class AnimationBase
    {
        protected const int LayerIndex = 0;
        protected readonly Animator _animator;

        public AnimationBase(Animator animator)
        {
            _animator = animator;
            AnimatorTiming.UpdateTiming(animator);
        }
        
        public void Play(Settings settings, Action callback = null)
        {
            _animator.DelayAsync(settings.Delay).ContinueWith(animator =>
            {
                animator.SetTrigger(settings.Id);
                animator.SetFloat(AnimationsConstants.Speed, settings.Speed);
                _ = animator.OnCompletedAsync(AnimatorTiming.GetTiming(settings.Id) / settings.Speed, callback);
            });
        }

        #region Settings

        [Serializable]
        public class Settings
        {
            [ValueDropdown(nameof(AnimationsDropdownList))]
            public int Id;
            [MinValue(0)]
            public float Delay;
            [MinValue(0.01f)]
            public float Speed;

#if ODIN_INSPECTOR
            private static readonly IEnumerable AnimationsDropdownList = new ValueDropdownList<int>()
            {
                { nameof(AnimationsConstants.FlyDown), AnimationsConstants.FlyDown },
                { nameof(AnimationsConstants.FlyTouchDown), AnimationsConstants.FlyTouchDown },
                { nameof(AnimationsConstants.LandingHard), AnimationsConstants.LandingHard },
            };
#endif
        }

        #endregion
    }
    
    public static class AnimationsConstants
    {
        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int FlyDown = Animator.StringToHash(nameof(FlyDown));
        public static readonly int FlyTouchDown = Animator.StringToHash(nameof(FlyTouchDown));
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int LandingHard = Animator.StringToHash(nameof(LandingHard));
    }
    
    public static class AnimatorExtension
    {
        public static async UniTask<Animator> DelayAsync(this Animator animator, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            return animator;
        }
        
        public static async UniTask OnCompletedAsync(this Animator animator, float delay,  Action callback, int layerIndex = 0)
        {
            //var length = await animator.ClipLengthAsync(layerIndex);
            //var length = AnimatorTiming.GetTiming(id);
            await UniTask.WaitForSeconds(delay);
            Debug.Log($"Delay: {delay}");
            callback?.Invoke();
        }
        
        public static async UniTask<float> ClipLengthAsync(this Animator animator, int layerIndex = 0)
        {
            await UniTask.Yield();
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
    }
}