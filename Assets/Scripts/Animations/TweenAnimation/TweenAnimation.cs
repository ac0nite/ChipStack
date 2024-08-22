using System;
using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class TweenAnimation : TweenAnimationBase
    {
        public static TweenAnimation CreateSimpleMove(TweenComponent.Settings settings)
        {
            var tween =  CreateOnlyMove();
            tween.AppendSequence(tween.Move.CreateChangeTween(settings));
            return tween;
        }

        public static TweenAnimation CreateInitialDropDebug(AnimationSettings settings)
        {
            var tween =  CreateMoveAndSize();
            //tween.AppendSequence(tween.Move.CreateChangeTween(settings.InitialDropDebug.Move));
            // tween.JoinSequence(tween.Move.CreateChangeTween(settings.InitialDropDebug.Move), tween.Size.CreateChangeLoopTween(settings.InitialDropDebug.Fly));
            // tween.AppendSequence(tween.Size.CreateChangeLoopTween(settings.InitialDropDebug.Stratching));
            // tween.StepOnCompleted(() => Debug.Log($"Step completed!"));
            return tween;
        }

        private void StepOnCompleted(Action stepCallback)
        {
            sequence.OnStepComplete(() => stepCallback?.Invoke());
        }

        private static TweenAnimation CreateOnlyMove()
        {
            return new TweenAnimation
            {
                Move = TweenComponent.CreateMoveTween()
            };
        }
        private static TweenAnimation CreateOnlySize()
        {
            return new TweenAnimation
            {
                Size = TweenComponent.CreateSizeTween()
            };
        }
        private static TweenAnimation CreateMoveAndSize()
        {
            return new TweenAnimation
            {
                Move = TweenComponent.CreateMoveTween(),
                Size = TweenComponent.CreateSizeTween()
            };
        }
    }
}