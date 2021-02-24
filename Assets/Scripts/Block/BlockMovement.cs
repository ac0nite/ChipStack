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

    [Header("Вращение по окружности")]
    [SerializeField] private float _speedСircle = 5f;
    [SerializeField] private float _amplitudeСircle = 1f;
    [SerializeField] private float _speedAmplitudeСircle = 1f;

    [Header("Максимальное положение от края")]
    [SerializeField] [Range(0f, 1f)] private float _scatterFromEdge = 1f;

    [Header("Скорость движение центральной координаты")]
    [SerializeField] private float _speedCenter = 2f;
    private float _kSpeedCenter = 1f;

    private Vector3 _targetCenter = Vector3.zero;
    private Vector3 _targetScale = Vector3.zero;

    [Header("Движение по радиусу")]
    [SerializeField] private float _amplitudeRadius = 2f;
    [SerializeField] private float _speedAmplitudeRadius = 2f;

    [Header("Начальные параметры")]
    [SerializeField] private float _maxRadius = 1f;

    [SerializeField] private AudioSource _dropAudio = null;

    [SerializeField] private string _walkAudioName = null;
    [SerializeField] private string _cutAudioName = null;
    [SerializeField] private HelperLineRenderer _helperLineRenderer = null;

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
        //инициализация в зависимости от уровней
        InitStage(GameManager.Instance.Property[GameManager.Instance.ScoreManager.Stage-1]);

        _nextTarget = RandomNextTarget(_maxRadius);

        _radius.Value = (GameManager.Instance.Center.transform.position - transform.position).magnitude;
        _radius.Amplitude = _amplitudeRadius;
        _radius.Speed = _speedAmplitudeRadius;
        
        _current_speed.Value = _speedСircle;
        _current_speed.Amplitude = _amplitudeСircle;
        _current_speed.Speed = _speedAmplitudeСircle;

        _direction = (CLOCK_DIRECTION)UnityEngine.Random.Range(1,3);
       
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

        _go = true;

        InputManager.Instance.EventTap += Stop;
    }

    private void Start()
    {
        _dropAudio.clip = Resources.Load<AudioClip>(("Music/" + _walkAudioName));
        _dropAudio.Play();
    }

    private void InitStage(PropertyStage property)
    {
        _speedСircle = property.SpeedCircle;
        _amplitudeСircle = property.AmplitudeCircle;
        _speedAmplitudeСircle = property.SpeedAmplitudeCircle;

        _scatterFromEdge = property.ScatterForEdge;

        _speedCenter = property.SpeedCenter;

        _amplitudeRadius = property.AmplitudeRadius;
        _speedAmplitudeRadius = property.SpeedAmplitudeRadius;
    }

    public void Init(Transform block)
    {
        var size = block.transform.localScale.x >= block.transform.localScale.z
           ? block.transform.localScale.x
           : block.transform.localScale.z;

       _targetScale = block.localScale;
        _targetCenter = new Vector3(block.transform.position.x, 0f, block.transform.position.z);
        GameManager.Instance.Center.transform.position = _targetCenter;

        _radius.Value = size / 2;
        _radius.Amplitude = size / 5;
        _maxRadius = size / 3;
        _nextTarget = RandomNextTarget(_radius.Value);

        _kSpeedCenter = Mathf.Clamp((block.transform.localScale.x * block.transform.localScale.z) / 25f, 0.1f, 1f);
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
            //это когда блок движется в сторону центра прямо
            transform.Translate((GameManager.Instance.Center.transform.position - transform.position).normalized * (Time.deltaTime * 5));
            _radius.Value = (GameManager.Instance.Center.transform.position - transform.position).magnitude;
        }
        else 
            run = true;

        if (run && (_nextTarget - GameManager.Instance.Center.transform.position).sqrMagnitude < 0.01f)
       {
           _nextTarget = RandomNextTarget(_radius.Value);
       }
       else 
       {
           //движение центральной координаты
           var look = (_nextTarget - GameManager.Instance.Center.transform.position).normalized;
           GameManager.Instance.Center.transform.Translate(look * (Time.deltaTime * _speedCenter * _kSpeedCenter));
       }
    }

    private Vector3 RandomNextTarget(float radius)
    {
        var x = UnityEngine.Random.Range((_targetCenter.x - _targetScale.x / 2 - _scatterFromEdge), (_targetCenter.x + _targetScale.x / 2 + _scatterFromEdge));
        var z = UnityEngine.Random.Range((_targetCenter.z + _targetScale.z / 2 + _scatterFromEdge), (_targetCenter.z - _targetScale.z / 2 - _scatterFromEdge));
        return new Vector3(x, 0f, z);
    }

    private Vector3 Anticlockwise()
    {
        var cache_speed = _current_speed.Value;

        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(GameManager.Instance.Center.transform.position.x + Mathf.Cos(_timerCount) * cache_radius, transform.position.y, GameManager.Instance.Center.transform.position.z + Mathf.Sin(_timerCount) * cache_radius);
    }

    private Vector3 Clockwise()
    {
        var cache_speed = _current_speed.Value;

        _timerCount += Time.deltaTime * cache_speed;
        var cache_radius = _radius.Value;

        return new Vector3(GameManager.Instance.Center.transform.position.x + Mathf.Sin(_timerCount) * cache_radius, transform.position.y, GameManager.Instance.Center.transform.position.z + Mathf.Cos(_timerCount) * cache_radius);
    }

    public void Stop()
    {
        _go = false;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(ExitRound(this.gameObject));
        GetComponent<Block>().Collision.RunLighting();

        if (InputManager.TryInstance != null)
            InputManager.Instance.EventTap -= Stop;

        _dropAudio.Stop();

        _dropAudio.PlayOneShot(Resources.Load<AudioClip>(("Music/" + _cutAudioName)));

        _helperLineRenderer.DisableHelper();
    }

    IEnumerator ExitRound(GameObject obj)
    {
        yield return new WaitForSeconds(_timeExitRound);
        EventExit?.Invoke();
        GetComponent<Block>().Collision.StopLighting();
    }

    private void OnDestroy()
    {
        if (InputManager.TryInstance != null)
            InputManager.Instance.EventTap -= Stop;
    }
}


