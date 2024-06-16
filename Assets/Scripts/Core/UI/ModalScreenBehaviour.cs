using Core.UI.Behaviour;
using UI.Screens.Shading;

namespace Core.UI
{
    public class ModalScreenBehaviour : ScreenBehaviourBase
    {
        private readonly ShadingScreenPresenter _shadingPresenter;

        public ModalScreenBehaviour(ShadingScreenPresenter shading)
        {
            _shadingPresenter = shading;
            _shadingPresenter.OnShadingTapPressedEvent += CloseTopScreenHandler;
        }

        private void CloseTopScreenHandler()
        {
            TryToHideTopScreenAndRemove();
            UpdateSortingOrder(_beginSortingOrder);
        }

        public override int UpdateSortingOrder(int beginOrder)
        {
            var order = base.UpdateSortingOrder(beginOrder);
            if (order > beginOrder)
            {
                _presenters[^1].UpdateOrder(order);
                _shadingPresenter.Show();
                _shadingPresenter.UpdateOrder(order - 1);
            }
            else
                _shadingPresenter.Hide();
            
            return ++order;
        }
    }
}