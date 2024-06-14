namespace Core.UI.MVP
{
    public interface IScreenViewKeeper
    {
        TView GetView<TView>() where TView : ViewBase;
    }
}