using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animations
{
    public class AnimationBlock : AnimationBase
    {
        public AnimationBlock(Animator animator) : base(animator)
        {
        }
    }

    public class AnimationBase
    {
        protected const int LayerIndex = 0;
        protected readonly Animator _animator;

        public AnimationBase(Animator animator)
        {
            _animator = animator;
        }
        
        public void Play(Settings settings, Action callback = null)
        {
            _animator.DelayAsync(settings.Delay).ContinueWith(animator =>
            {
                animator.SetTrigger(settings.Id);
                animator.SetFloat(AnimationsConstants.Speed, settings.Speed);
                _ = animator.OnCompletedAsync(callback);
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
                { nameof(AnimationsConstants.FlyLanding), AnimationsConstants.FlyLanding },
                { nameof(AnimationsConstants.Landing), AnimationsConstants.Landing },
            };
#endif
        }

        #endregion
    }
    
    public static class AnimationsConstants
    {
        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int FlyLanding = Animator.StringToHash(nameof(FlyLanding));
        public static readonly int Landing = Animator.StringToHash(nameof(Landing));
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
    }
    
    public static class AnimatorExtension
    {
        public static async UniTask<Animator> DelayAsync(this Animator animator, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            return animator;
        }
        
        public static async UniTask OnCompletedAsync(this Animator animator, Action callback, int layerIndex = 0)
        {
            var length = await animator.ClipLengthAsync(layerIndex);
            await UniTask.WaitForSeconds(length);
            callback?.Invoke();
        }
        
        public static async UniTask<float> ClipLengthAsync(this Animator animator, int layerIndex = 0)
        {
            await UniTask.Yield();
            return animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }
    }
}