using System;
using System.Collections.Generic;
using Core.UI.Behaviour;
using Core.UI.MVP;

namespace Core.UI
{
    public class ScreenManagerBase : IScreenManager
    {
        protected IScreenViewKeeper viewKeeper;
        protected Dictionary<Type, IScreenPresenter> presenters = new();
        protected PresenterFactory factory = new();

        public virtual void Initialise(IScreenViewKeeper keeper)
        {
            viewKeeper = keeper;
        }
        public TPresenter Register<TPresenter, TView>(params object[] model)
            where TPresenter : ScreenPresenterBase<TView>
            where TView : ViewBase
        {
            var type = TypeScreenExists<TView>();
            var view = viewKeeper.GetView<TView>();
            var presenter = factory.Create<TPresenter, TView>(view, model);
            presenters.Add(type, presenter);
            return presenter;
        }

        public IScreenBehaviour Behaviour { get; set; }

        public void ShowScreen<Tview>() where Tview : ViewBase
        {
            var screenType = TypeScreenValidate<Tview>();
            var presenter = presenters[screenType];
            Behaviour.Show(presenter);
        }

        public void HideScreen<Tview>() where Tview : ViewBase
        {
            var screenType = TypeScreenValidate<Tview>();
            var presenter = presenters[screenType];
            Behaviour.Hide(presenter);
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