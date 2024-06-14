using System;
using DG.Tweening;

namespace Core.Utils
{
    public static class General
    {
        [Serializable]
        public struct MovementSettings
        {
            public int Duration;
            public Ease Ease;
        }
    }
}