using System;
using System.Collections.Generic;
using UnityEngine;

namespace SavingData
{
    public class SavingKeys
    {
        public static readonly string SoundMutedKey = nameof(SoundMutedKey);
        public static readonly string StageKey = nameof(StageKey);
        public static readonly string BestScoreKey = nameof(BestScoreKey);
        public static readonly string TotalScoreKey = nameof(TotalScoreKey);
        public static readonly string BestLevelKey = nameof(BestLevelKey);
        public static readonly string GradientIndexKey = nameof(GradientIndexKey);
    }
    
    public class Saving
    {
        public static int GetIntValue(string key)
        {
            return PlayerPrefs.GetInt(key, 0);
        }
        
        public static void SetIntValue(string key, int value)
        {
            PlayerPrefs.SetInt(key,value);
        }

        public static bool GetBoolValue(string key)
        {
            return GetIntValue(key) != 0;
        }
        
        public static bool SetBoolValue(string key, bool value)
        {
            SetIntValue(key, value ? 1 : 0);
            return value;
        }
        
        public static float GetFloatValue(string key)
        {
            return PlayerPrefs.GetFloat(key, 0);
        }
        
        public static void SetFloatValue(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        
        public static T GetValue<T>(string key) where T : struct
        {
            if(!_typeValueForGetValueFuncs.TryGetValue(typeof(T), out var func))
                throw new ArgumentException("No GetValueFunc for type " + typeof(T).Name);
            
            return (T)func(key);
        }
        
        public static void SetValue<T>(string key, T value) where T : struct
        {
            if(!_typeValueForSetValueFuncs.TryGetValue(typeof(T), out var func))
                throw new ArgumentException("No SetValueFunc for type " + typeof(T).Name);
            
            func(key, value);
        }
        
        private static readonly Dictionary<Type, Func<string, object>> _typeValueForGetValueFuncs =
            new ()
            {
                {typeof(int), key => GetIntValue(key)},
                {typeof(float), key => GetFloatValue(key)},
                {typeof(bool), key => GetBoolValue(key)}
            };
        
        private static readonly Dictionary<Type, Action<string, object>> _typeValueForSetValueFuncs =
            new ()
            {
                {typeof(int), (key, value) => SetIntValue(key, (int) value)},
                {typeof(float), (key, value) => SetFloatValue(key, (float) value)},
                {typeof(bool), (key, value) => SetBoolValue(key, (bool) value)}
            };
    }
}