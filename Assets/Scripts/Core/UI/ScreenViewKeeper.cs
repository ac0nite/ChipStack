using System;
using System.Collections.Generic;
using Core.UI.MVP;
using UnityEngine;

namespace Core.UI
{
    [Serializable]
    public class ScreenViewKeeper : IScreenViewKeeper
    {
        [SerializeField] private List<ViewBase> _screenViews;

        public TView GetView<TView>() where TView : ViewBase
        {
            var screenType = typeof(TView);
            var screenView = _screenViews.Find(screen => screen.GetType() == screenType);
            if (screenView == null)
                throw new Exception($"Screen {screenType} not created");
            return screenView as TView;
        }
    }
}