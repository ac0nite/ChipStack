using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

    public class ChangeParam
    {
        //по сути, когда присваиваем _value занчение тем самым инициализируем минимальное значение скорости
        private float _value = 0f;

        private float _counter = 0f;

        // максимальное значение 
        private float _amplitude = 1f;

        // задаём коэффиуиент для вичисления cos(time * speed), по сути задаётся частота дискретности
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