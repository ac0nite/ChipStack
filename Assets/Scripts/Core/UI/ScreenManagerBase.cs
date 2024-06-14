using System;
using System.Collections.Generic;
using Core.UI.MVP;

namespace Core.UI
{
    public class ScreenManagerBase : IScreenManager
    {
        protected IScreenViewKeeper viewKeeper;
        protected Dictionary<Type, IScreenPresenter> presenters = new();
        protected PresenterFactory factory = new();
        protected Type currentScreenType = null;

        public virtual void Initialise(IScreenViewKeeper keeper)
        {
            viewKeeper = keeper;
        }
        public void Register<TPresenter, TView>(params object[] model)
            where TPresenter : ScreenPresenterBase<TView>
            where TView : ViewBase
        {
            var type = TypeScreenExists<TView>();
            var view = viewKeeper.GetView<TView>();
            var presenter = factory.Create<TPresenter, TView>(view, model);
            presenters.Add(type, presenter);
        }
        public void ShowScreen<Tview>() where Tview : ViewBase
        {
            CloseCurrentScreen();
            currentScreenType = TypeScreenValidate<Tview>();
            presenters[currentScreenType].Show();
        }

        public void HideScreen<Tview>() where Tview : ViewBase
        {
            var type = TypeScreenValidate<Tview>();
            presenters[type].Hide();
        }
        public void CloseCurrentScreen()
        {
            if(currentScreenType == null) return;
            presenters[currentScreenType].Hide();
            currentScreenType = null;
        }
        protected Type TypeScreenExists<TView>() where TView : ViewBase
        {
            var type = typeof(TView);
            if (presenters.ContainsKey(type))
                throw new Exception($"Screen {type} already created");
            return type;
        }
        protected Type TypeScreenValidate<TView>() where TView : ViewBase
        {
            var type = typeof(TView);
            if (!presenters.ContainsKey(type))
                throw new Exception($"Screen {type} not created");
            return type;
        }
    }
}