using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

    public class ChangeParam
    {
        private float _value = 0f;
        private float _counter = 0f;
        private float _amplitude = 1f;
        private float _speed = 1f;

        public float Amplitude
        {
            set
            {
                _amplitude = value / 2f;
            }
        }

        public float Value
        {
            set
            {
                _counter = 0f;
                _value = value;
            }
            get
            {
                var v = _value + _amplitude - _amplitude * Mathf.Cos(_counter);
                _counter += Time.deltaTime * _speed;
                return v;
            }
        }

        public float Speed
        {
            private get => _speed;
            set => _speed = value;
        }
    }