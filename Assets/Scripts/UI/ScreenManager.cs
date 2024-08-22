using Core.UI;
using Core.UI.Behaviour;
using Core.UI.MVP;
using Cysharp.Threading.Tasks;
using UI.Popups;
using UI.Screens.Debug;
using UI.Screens.Gameplay;
using UI.Screens.Loading;
using UI.Screens.PreGameplay;
using UI.Screens.Result;
using UI.Screens.Shading;

namespace UI
{
    public class ScreenManager : ScreenManagerBase
    {
        private ScreenManager() { }
        private static readonly ScreenManager _instance = new();

        public static void Create(IScreenViewKeeper keeper, GameplayContext context)
        {
            _instance.Initialise(keeper);
            
            _instance.Register<LoadingScreenPresenter, LoadingScreen>();
            _instance.Register<PreGameplayScreenPresenter, PreGameplayScreen>(context.StatesMachineModel);
            _instance.Register<GameplayScreenPresenter, GameplayScreen>(context.Currency.ScoreModelGetter);
            _instance.Register<ResultScreenPresenter, ResultScreen>(context.Currency.ScoreModelGetter, context.StatesMachineModel);
            _instance.Register<DebugScreenPresenter, DebugScreen>(context.BlockFacade);
            
            _instance.Register<ModifyScreenPopupPresenter, ModifyScreenPopup>();
            
            var shading = _instance.Register<ShadingScreenPresenter, ShadingScreen>();
            
            _instance.Behaviour = new ScreensBehaviour(1, new ScreenBehaviour(), new ModalScreenBehaviour(shading));
        }
        
        public static void Show<TView>() where TView : ViewBase
        {
            _instance.ShowScreen<TView>();            
        }
        
        public static void Hide<TView>() where TView : ViewBase
        {
            _instance.HideScreen<TView>();
        }

        #region LOADING

        public class Loading
        {
            private Loading() { }
            public delegate UniTask ExecuteDelegateAsync();
            public delegate void CompletedDelegate();
            private static ExecuteDelegateAsync ExecuteAsync { get; set; } = null;
            private static CompletedDelegate Completed { get; set; } = null;

            public async void Show()
            {   
                ScreenManager.Show<LoadingScreen>();
                await UniTask.DelayFrame(1);
                await ExecuteAsync();
                Completed?.Invoke();
                Hide();
            }
            
            public static Loading Execute(ExecuteDelegateAsync execute)
            {
                ExecuteAsync = execute;
                return new Loading();
            }

            public Loading OnCompleted(CompletedDelegate completed)
            {
                Completed = completed;
                return this;
            }
        
            public static void Hide()
            {
                ExecuteAsync = null;
                Completed = null;
                ScreenManager.Hide<LoadingScreen>();
            }
        }

        #endregion
    }
}