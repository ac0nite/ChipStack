using System;
using UnityEngine;

namespace Core.Utils
{
    public class CameraHelper
    {
        public static void UpdateView(Camera camera, Settings settings)
        {
            camera.orthographicSize = settings.Ratio * ((float)Screen.height / Screen.width) + 0.143f + settings.Offset;
        }
    
        [Serializable]
        public struct Settings
        {
            /// <summary>
            /// default 5.143
            /// </summary>
            public float Ratio;
            public float Offset;
        }
    }   
}