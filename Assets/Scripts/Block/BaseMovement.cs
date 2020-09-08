using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    private Vector3 _target = Vector3.zero;
    [SerializeField] private float _speed = 10f;

    public Vector3 Target
    {
        set { _target = value; }
    }

    private void Awake()
    {
        _target = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target, _speed * Time.deltaTime);
    }
}
