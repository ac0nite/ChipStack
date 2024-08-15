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
}