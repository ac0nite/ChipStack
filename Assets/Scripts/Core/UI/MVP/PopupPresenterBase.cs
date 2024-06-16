using UI;

namespace Core.UI.MVP
{
    public abstract class PopupPresenterBase<TView> : ScreenPresenterBase<TView> 
        where TView : ViewBase
    {
        protected PopupPresenterBase(TView view) : base(view)
        { }

        protected void Close()
        {
            ScreenManager.Hide<TView>();
        }
    }
}