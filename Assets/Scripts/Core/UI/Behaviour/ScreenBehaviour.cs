using Core.UI.MVP;

namespace Core.UI.Behaviour
{
    public class ScreenBehaviour : ScreenBehaviourBase
    {
        public override void Show(IScreenPresenter presenter)
        {
            TryToHideTopScreenAndRemove();
            base.Show(presenter);
        }
    }
}