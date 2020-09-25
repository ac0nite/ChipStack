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

    [SerializeField] [Range(0f, 0.8f)] private float percent_part = 1f;

    // [SerializeField] private Vector3 _center = Vector3.zero;
    [SerializeField] private float _speedCenter = 2f;
    private float _kSpeedCenter = 1f;

    private Vector3 _targetCenter = Vector3.zero;
    private Vector3 _targetScale = Vector3.zero;

    [SerializeField] private float _amplitudeRadius = 2f;
    [SerializeField] private float _speedAmplitudeRadius = 2f;

    [SerializeField] private float _maxRadius = 1f;

    [SerializeField] private AudioSource _dropAudio = null;

    [SerializeField] private string _walkAudioName;
    [SerializeField] private string _cutAudioName;

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

        _radius.Value = (GameManager.Instance.Center.transform.position - transform.position).magnitude;
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
        Debug.Log($"BlockMovement Awake()");
    }

    private void Start()
    {
        InputManager.Instance.EventTap += Stop;
        _dropAudio.clip = Resources.Load<AudioClip>(("Music/" + _walkAudioName));
        _dropAudio.Play();
    }

    public void Init(Transform block)
    {
       // Debug.Log($"NEW CENTER: {block.transform.position}");
       var size = block.transform.localScale.x >= block.transform.localScale.z
           ? block.transform.localScale.x
           : block.transform.localScale.z;

       _targetScale = block.localScale;
        _targetCenter = new Vector3(block.transform.position.x, 0f, block.transform.position.z);
        GameManager.Instance.Center.transform.position = _targetCenter;

        // _radius.Value = (GameManager.Instance.Center.transform.position - transform.position).magnitude;
        _radius.Value = size / 2;
        _radius.Amplitude = size / 5;
        _maxRadius = size / 3;
        _nextTarget = RandomNextTarget(_radius.Value);


        //_kSpeedCenter = (block.transform.localScale.x * block.transform.localScale.z) / 25f;
        _kSpeedCenter = Mathf.Clamp((block.transform.localScale.x * block.transform.localScale.z) / 25f, 0.1f, 1f);
        Debug.Log($"_kSpeedCenter: {_kSpeedCenter}");


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

        if (!run && (transform.position - GameManager.Instance.Center.transform.position).magnitude >= _maxRadius)
        {
            transform.Translate((GameManager.Instance.Center.transform.position - transform.position).normalized * (Time.deltaTime * 5));
            _radius.Value = (GameManager.Instance.Center.transform.position - transform.position).magnitude;
        }
        else run = true;

        if (run && (_nextTarget - GameManager.Instance.Center.transform.position).sqrMagnitude < 0.01f)
       {
           _nextTarget = RandomNextTarget(_radius.Value);
       }
       else 
       {
            //_center.Translate((_nextTarget - _center.transform.position).normalized * (Time.deltaTime * _speedCenter));

            //_center += (_nextTarget - _center).normalized * (Time.deltaTime * _speedCenter);
            // GameManager.Instance.Center.transform.position = GameManager.Instance.Center.transform.position;
            var look = (_nextTarget - GameManager.Instance.Center.transform.position).normalized;
            GameManager.Instance.Center.transform.Translate(look * (Time.deltaTime * _speedCenter * _kSpeedCenter));
            //GameManager.Instance.Center.transform.position += (_nextTarget - GameManager.Instance.Center.transform.position).normalized * (Time.deltaTime * _speedCenter);

            //_center.Translate((_nextTarget - _center.transform.position).normalized * (Time.deltaTime * _speedCenter));
        }
    }

    private void OnDestroy()
    {
    }

    private Vector3 RandomNextTarget(float radius)
    {
        //var c = UnityEngine.Random.insideUnitCircle;

        //// Debug.Log($"c: {c}");
        //c *= radius;

        //var t = new Vector3(c.x, 0f, c.y);

        //Debug.Log($"RandomNextTarget: {t} radius: {radius}");
        //return t;


        //var x = UnityEngine.Random.Range((_targetCenter.x - _targetScale.x / 2) * percent_part, (_targetCenter.x + _targetScale.x / 2) * percent_part);
        //var z = UnityEngine.Random.Range((_targetCenter.z + _targetScale.z / 2) * percent_part, (_targetCenter.z - _targetScale.z / 2) * percent_part);
        var x = UnityEngine.Random.Range((_targetCenter.x - _targetScale.x / 2 - percent_part), (_targetCenter.x + _targetScale.x / 2 + percent_part));
        var z = UnityEngine.Random.Range((_targetCenter.z + _targetScale.z / 2 + percent_part), (_targetCenter.z - _targetScale.z / 2 - percent_part));
        return new Vector3(x, 0f, z);

        //return new Vector3(UnityEngine.Random.Range())
    }


    private Vector3 Anticlockwise()
    {
        //_current_speed.Value = SpeedСircle;
        
        var cache_speed = _current_speed.Value;
        //Debug.Log($"cache_speed: {cache_speed}");
        
        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(GameManager.Instance.Center.transform.position.x + Mathf.Cos(_timerCount) * cache_radius, transform.position.y, GameManager.Instance.Center.transform.position.z + Mathf.Sin(_timerCount) * cache_radius);
    }

    private Vector3 Clockwise()
    {
        //_current_speed.Value = SpeedСircle;
        
        var cache_speed = _current_speed.Value;
        //Debug.Log($"cache_speed: {cache_speed}");
        
        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(GameManager.Instance.Center.transform.position.x + Mathf.Sin(_timerCount) * cache_radius, transform.position.y, GameManager.Instance.Center.transform.position.z + Mathf.Cos(_timerCount) * cache_radius);
    }

    public void Stop()
    {
        // Debug.Log($"public void Stop()", transform.gameObject);
        _go = false;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(ExitRound(this.gameObject));
        GetComponent<Block>().Collision.RunLighting();

        if (InputManager.TryInstance != null)
            InputManager.Instance.EventTap -= Stop;

        //InputManager.Instance.EventTap -= Stop;

        _dropAudio.Stop();

        _dropAudio.PlayOneShot(Resources.Load<AudioClip>(("Music/" + _cutAudioName)));
    }

    IEnumerator ExitRound(GameObject obj)
    {
        yield return new WaitForSeconds(_timeExitRound);
        EventExit?.Invoke();
        GetComponent<Block>().Collision.StopLighting();
        //Destroy(obj);
    }
}


