using Core.UI.MVP;

namespace Core.UI
{
    public interface IScreenManager
    {
        void Initialise(IScreenViewKeeper keeper);

        void Register<TPresenter, TView>(params object[] objects)
            where TPresenter : ScreenPresenterBase<TView>
            where TView : ViewBase;

        void ShowScreen<TView>() where TView : ViewBase;
        void HideScreen<TView>() where TView : ViewBase;
        void CloseCurrentScreen();
    }
}