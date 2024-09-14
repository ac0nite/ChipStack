using UnityEngine;

namespace Core.Utils
{
    public class SettingsScriptableObject<T> : ScriptableObject
        where T : Object
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                _instance ??= Resources.Load<T>(typeof(T).Name);
                return _instance;
            }
        }
    }
}