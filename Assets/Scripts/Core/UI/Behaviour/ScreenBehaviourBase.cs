using System;
using System.Collections.Generic;
using Core.UI.MVP;

namespace Core.UI.Behaviour
{
    public class ScreenBehaviourBase : IScreenBehaviour
    {
        protected List<IScreenPresenter> _presenters = new ();
        protected int _beginSortingOrder;
        public event Action OnChangeEvent;
        public virtual void Show(IScreenPresenter presenter)
        {
            presenter.Show();
            _presenters.Add(presenter);
            OnChangeEvent?.Invoke();
        }

        public virtual void Hide(IScreenPresenter presenter)
        {
            if(!IsValid(presenter)) return;
            
            presenter.Hide();
            _presenters.Remove(presenter);
            OnChangeEvent?.Invoke();
        }

        public virtual int UpdateSortingOrder(int beginOrder)
        {
            var order = _beginSortingOrder = beginOrder;
            for (var i = 0; i < _presenters.Count; i++)
                _presenters[i].UpdateOrder(order++);
            return order;
        }
        
        private bool IsValid(IScreenPresenter presenter) => _presenters.Contains(presenter);

        protected bool TryToHideTopScreenAndRemove()
        {
            if (_presenters.Count == 0)
                return false;
            
            _presenters[^1].Hide();
            _presenters.Remove(_presenters[^1]);
            return true;
        }
    }
}