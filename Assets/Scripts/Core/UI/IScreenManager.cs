using Core.UI.Behaviour;
using Core.UI.MVP;

namespace Core.UI
{
    public interface IScreenManager
    {
        void Initialise(IScreenViewKeeper keeper);

        TPresenter Register<TPresenter, TView>(params object[] objects)
            where TPresenter : ScreenPresenterBase<TView>
            where TView : ViewBase;

        IScreenBehaviour Behaviour { get; set; }
    }
}