using System;
using DG.Tweening;

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

        private void StepOnCompleted(Action stepCallback)
        {
            sequence.OnStepComplete(() => stepCallback?.Invoke());
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
}