using Core.UI.MVP;

namespace UI.Popups
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class ModifyScreenPopupPresenter : PopupPresenterBase<ModifyScreenPopup>
    {
        public ModifyScreenPopupPresenter(ModifyScreenPopup view) : base(view)
        {
            _view.OnCloseButtonPressedEvent += Close;
        }

        protected override void Dispose()
        {
            _view.OnCloseButtonPressedEvent -= Close;
        }
    }
}