namespace Core.UI.MVP
{
    public interface IScreenPresenter
    {
        bool IsModal { get; }
        void UpdateOrder(int order = 0);
        void Show();
        void Hide();
    }
}