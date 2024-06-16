using System;
using Core.UI.MVP;

namespace Core.UI.Behaviour
{
    public class ScreensBehaviour : IScreenBehaviour
    {
        private readonly IScreenBehaviour _screenBehaviour;
        private readonly IScreenBehaviour _modalScreenBehaviour;
        private readonly int _beginSortingOrder;
        public event Action OnChangeEvent;

        public ScreensBehaviour(int beginSortingOrder, IScreenBehaviour screenBehaviour, IScreenBehaviour modalScreenBehaviour)
        {
            _beginSortingOrder = beginSortingOrder;
            _screenBehaviour = screenBehaviour;
            _modalScreenBehaviour = modalScreenBehaviour;
            
            _screenBehaviour.OnChangeEvent += UpdateSortingOrder;
            _modalScreenBehaviour.OnChangeEvent += UpdateSortingOrder;
        }

        private void UpdateSortingOrder()
        {
            UpdateSortingOrder(_beginSortingOrder);
        }
        public void Show(IScreenPresenter presenter)
        {
            if(presenter.IsModal) 
                _modalScreenBehaviour.Show(presenter);
            else
                _screenBehaviour.Show(presenter);
        }

        public void Hide(IScreenPresenter presenter)
        {
            if(presenter.IsModal) 
                _modalScreenBehaviour.Hide(presenter);
            else
                _screenBehaviour.Hide(presenter);
        }

        public int UpdateSortingOrder(int beginOrder)
        {
            var order = _screenBehaviour.UpdateSortingOrder(beginOrder);
            return _modalScreenBehaviour.UpdateSortingOrder(order);;
        }
    }
}