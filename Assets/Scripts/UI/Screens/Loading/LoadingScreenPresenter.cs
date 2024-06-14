using Core.UI.MVP;

namespace UI.Screens.Loading
{
    public class LoadingScreenPresenter : ScreenPresenterBase<LoadingScreen>
    {
        public LoadingScreenPresenter(LoadingScreen view) : base(view)
        {
        }

        protected override void Dispose()
        { }
    }
}