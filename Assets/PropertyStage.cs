using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PropertyStage
{
    [Header("Вращение по окружности")]
    [Range(0f, 10f)] public float SpeedCircle = 0f;
    [Range(0f, 10f)] public float AmplitudeCircle = 1f;
    [Range(0f, 10f)] public float SpeedAmplitudeCircle = 0.5f;

    [Header("Скорость движение центральной координаты")]
    [Range(0f, 2f)] public float SpeedCenter = 0.1f;

    [Header("Скорость движение центра")]
    [Range(0f, 10f)] public float AmplitudeRadius = 1f;
    [Range(0f, 10f)] public float SpeedAmplitudeRadius = 0.5f;

    [Header("Максимальное положение от края")]
    [Range(0f, 5f)] public float ScatterForEdge = 0f;
}
