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

        public bool IsModal => _view.IsModal;

        public void UpdateOrder(int order = 0)
        {
            _view.ChangeOrder(order);
        }

        public virtual void Show()
        {
            if (!_view.IsVisible)
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
            _view.OnDisposeEvent -= ViewDisposeHandler;
        }

        protected abstract void Dispose();
    }
}