using System;

namespace Core.UI.MVP
{
    public interface IView
    {
        void Show(bool withAnimation = true);
        void Hide(bool withAnimation = true);
        void ChangeOrder(int order);
        bool IsVisible { get; }
        event Action OnDisposeEvent;
    }
}