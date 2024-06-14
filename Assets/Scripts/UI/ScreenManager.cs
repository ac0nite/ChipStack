using System;
using Core.UI;
using Core.UI.MVP;
using Cysharp.Threading.Tasks;
using UI.Screens.Gameplay;
using UI.Screens.Loading;
using UI.Screens.PreGameplay;
using UI.Screens.Result;

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
        }
        
        public static void Show<Tview>() where Tview : ViewBase
        {
            _instance.ShowScreen<Tview>();            
        }
        
        public static void Hide<Tview>() where Tview : ViewBase
        {
            _instance.HideScreen<Tview>();
        }
        
        public static void CloseOpeningScreen()
        {
            _instance.CloseCurrentScreen();
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