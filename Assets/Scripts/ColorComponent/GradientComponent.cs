using System;
using SavingData;
using UnityEngine;

namespace ColorComponent
{
    public class GradientComponent
    {
        private Gradient[] _gradients;
        private float _step;
        private float _evaluate;
        private int _index;
        private readonly string _databaseKey;

        protected GradientComponent(Settings settings, string savingKey)
        {
            _step = settings.Step;
            _gradients = settings.GradientArray;
            _databaseKey = savingKey;
            _index = LoadIndex();
        }

        protected Color _currentGradientColor => _currentGradient.Evaluate(_evaluate);
        protected Color _nextGradientColor => _gradients[(_index + 1) % _gradients.Length].Evaluate(_evaluate);

        protected virtual void UpdateNextColor()
        {
            _evaluate += _step;
            if (_evaluate >= 1f)
            {
                _evaluate = 0f; 
                _index++;
                if (_index >= _gradients.Length) _index = 0;
                SaveIndex(_index);
            }
        }

        protected virtual int LoadIndex() => Saving.GetIntValue(_databaseKey);
        protected virtual void SaveIndex(int index) => Saving.SetIntValue(_databaseKey, index);
        private Gradient _currentGradient => _gradients[_index % _gradients.Length];
    
        [Serializable]
        public class Settings
        {
            [Range(0, 1)] public float Step = 0.03f;
            public Gradient[] GradientArray;
        }
    }
}