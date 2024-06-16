using System;
using Core.UI.MVP;

namespace Core.UI.Behaviour
{
    public interface IScreenBehaviour
    {
        void Show(IScreenPresenter presenter);
        void Hide(IScreenPresenter presenter);
        int UpdateSortingOrder(int beginOrder);
        event Action OnChangeEvent;
    }
}