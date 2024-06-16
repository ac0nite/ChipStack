using System;
using Core.UI.MVP;

namespace UI.Screens.Shading
{
    public class ShadingScreenPresenter : ScreenPresenterBase<ShadingScreen>
    {
        public event Action OnShadingTapPressedEvent;
        public ShadingScreenPresenter(ShadingScreen view) : base(view)
        {
            _view.OnShadingTapPressedEvent += CloseModalScreenHandler;
        }

        protected override void Dispose()
        {
            _view.OnShadingTapPressedEvent -= CloseModalScreenHandler;
        }
        
        private void CloseModalScreenHandler()
        {
            OnShadingTapPressedEvent?.Invoke();
        }
    }
}