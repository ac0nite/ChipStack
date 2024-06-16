using System;
using UI.Screens.Loading;
using UI.Screens.Shading;

namespace UI.Screens
{
    [Serializable]
    public class ScreensSettings
    {
        public ShadingScreen.Settings Shading;
        public LoadingScreen.Settings Loading;
    }
}