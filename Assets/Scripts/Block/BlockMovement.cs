using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class BlockMovement : MonoBehaviour
{
    public Action EventExit;
    [SerializeField] private float _timeExitRound = 2f;
    [SerializeField] public float SpeedСircle = 5f;
    [SerializeField] private float _amplitudeСircle = 1f;
    [SerializeField] private float _speedAmplitudeСircle = 1f;

    [SerializeField] private Vector3 _center = Vector3.zero;
    [SerializeField] private float _speedCenter = 2f;
    
    [SerializeField] private float _amplitudeRadius = 2f;
    [SerializeField] private float _speedAmplitudeRadius = 2f;

    [SerializeField] private float _maxRadius = 6f;
    private bool run = false;
    private bool _go = false;

    private float _timerCount = 0f;
    
    private ChangeParam _radius = new ChangeParam();
    private ChangeParam _current_speed = new ChangeParam();
    private Vector3 _nextTarget = Vector3.zero;
    private CLOCK_DIRECTION _direction = CLOCK_DIRECTION.Undefined;

    enum CLOCK_DIRECTION
    {
        Undefined = 0,
        Clock_Wise = 1,
        Anti_Clock_Wise = 2
    };

    private void Awake()
    {
        //_center.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        //var radius = (_center.position - transform.position).magnitude;
       // _center.y = 0.5f;
        
        _nextTarget = RandomNextTarget(_maxRadius);

        _radius.Value = (_center - transform.position).magnitude;
        _radius.Amplitude = _amplitudeRadius;
        _radius.Speed = _speedAmplitudeRadius;
        
        _current_speed.Value = SpeedСircle;
        _current_speed.Amplitude = _amplitudeСircle;
        _current_speed.Speed = _speedAmplitudeСircle;

        _direction = (CLOCK_DIRECTION)UnityEngine.Random.Range(1,3);
        //Debug.Log($"{_direction}");
        if (_direction == CLOCK_DIRECTION.Clock_Wise)
        {
            transform.position = new Vector3(-13f, 0f, 0f);
            _timerCount = 4.75f;
        }
        else
        {
            transform.position = new Vector3(0f, 0f, -13f);
            _timerCount = 4.8f;
        }

        //0 1.6 3.1 4.5   
        //_timerCount = 4.5f;

        _go = true;

        InputManager.Instance.EventTap += Stop;
    }

    public void Init(Transform block)
    {
        Debug.Log($"NEW CENTER: {block.transform.position}");
        
        _center = new Vector3(block.transform.position.x, 0f, block.transform.position.z);
        _radius.Value = (_center - transform.position).magnitude;
        _maxRadius = block.transform.localScale.x / 2f;


        //_maxRadius = Mathf.Abs(transform.localScale.x);
        //_center = center;
        //Debug.DrawLine(center.normalized, Vector3.up, Color.blue, 3f);
        //_maxRadius = transform.localScale.x;
    }

    private void Update()
    {
        if(!_go)
            return;
        
        if (run)
        {
            if (_direction == CLOCK_DIRECTION.Clock_Wise)
                transform.position = Clockwise();
            else
                transform.position = Anticlockwise();
        }

        if (!run && (transform.position - _center).magnitude >= _maxRadius)
        {
            transform.Translate((_center - transform.position).normalized * (Time.deltaTime * 10f));
            _radius.Value = (_center - transform.position).magnitude;
        }
        else run = true;

        if ((_nextTarget - _center).sqrMagnitude < 0.1f)
       {
           _nextTarget = RandomNextTarget(_radius.Value);
       }
       else 
       {
            //_center.Translate((_nextTarget - _center.transform.position).normalized * (Time.deltaTime * _speedCenter));

            //_center += (_nextTarget - _center).normalized * (Time.deltaTime * _speedCenter);
            _center = _center;

            //_center.Translate((_nextTarget - _center.transform.position).normalized * (Time.deltaTime * _speedCenter));
       }
    }

    private void OnDestroy()
    {
        if(InputManager.TryInstance != null)
            InputManager.Instance.EventTap -= Stop;
    }

    private Vector3 RandomNextTarget(float radius)
    {
        var c = UnityEngine.Random.insideUnitCircle * radius;
        // var t = new Vector3(c.x, transform.position.y, c.y);
        var t = new Vector3(c.x, 0f, c.y);
       // Debug.Log($"RandomNextTarget: {t}");
        return t;
    }


    private Vector3 Anticlockwise()
    {
        //_current_speed.Value = SpeedСircle;
        
        var cache_speed = _current_speed.Value;
        //Debug.Log($"cache_speed: {cache_speed}");
        
        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(_center.x + Mathf.Cos(_timerCount) * cache_radius, transform.position.y, _center.z + Mathf.Sin(_timerCount) * cache_radius);
    }

    private Vector3 Clockwise()
    {
        //_current_speed.Value = SpeedСircle;
        
        var cache_speed = _current_speed.Value;
        //Debug.Log($"cache_speed: {cache_speed}");
        
        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(_center.x + Mathf.Sin(_timerCount) * cache_radius, transform.position.y, _center.z + Mathf.Cos(_timerCount) * cache_radius);
    }

    public void Stop()
    {
        _go = false;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(ExitRound());
    }

    IEnumerator ExitRound()
    {
        yield return new WaitForSeconds(_timeExitRound);
        EventExit?.Invoke();
    }
}


