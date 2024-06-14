namespace Core.UI.MVP
{
    public abstract class ScreenPresenterBase<TView> : IScreenPresenter where TView : IView 
    {
        protected readonly TView _view;

        protected ScreenPresenterBase(TView view)
        {
            _view = view;
            _view.OnDisposeEvent += ViewDisposeHandler;
        }

        public virtual void Show()
        {
            if(!_view.IsVisible) 
                _view.Show();
        }

        public virtual void Hide()
        {
            if(_view.IsVisible) 
                _view.Hide();
        }
        
        private void ViewDisposeHandler()
        {
            Dispose();            
        }

        protected abstract void Dispose();
    }
}