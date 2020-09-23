using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speedLerp = 5f;
    [SerializeField] private GameObject _firstTarget = null;
    private Vector3 _default = Vector3.zero;
    private Vector3 _target = Vector3.zero;
    private Vector3 _offset = Vector3.zero;

    void Start()
    {
        _default = transform.position;
        _target = _firstTarget.transform.position;
        _offset = transform.position - _firstTarget.transform.position;

        GameController.EventBlock += OnNextTarget;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target + _offset, _speedLerp * Time.deltaTime);
    }

    void OnNextTarget(Vector3 target)
    {
        _target = target;
    }

    void OnDestroy()
    {
        GameController.EventBlock -= OnNextTarget;
    }
}
