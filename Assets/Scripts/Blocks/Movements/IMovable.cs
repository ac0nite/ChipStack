using Vector3 = UnityEngine.Vector3;

namespace Blocks.Movements
{
    public interface IMovable
    {
        void UpdatePosition(float deltaTime);
        Vector3 Position { get; }
        bool IsCompleted { get; }
        void ApplyPosition();
    }
}

// public class BlockMovement : MonoBehaviour
// {
//     public Action EventExit;
//     [SerializeField] private float _timeExitRound = 2f;
//
//     [Header("Вращение по окружности")]
//     [SerializeField] private float _speedСircle = 5f;
//     [SerializeField] private float _amplitudeСircle = 1f;
//     [SerializeField] private float _speedAmplitudeСircle = 1f;
//
//     [Header("Максимальное положение от края")]
//     [SerializeField] [Range(0f, 1f)] private float _scatterFromEdge = 1f;
//
//     [Header("Скорость движение центральной координаты")]
//     [SerializeField] private float _speedCenter = 2f;
//     private float _kSpeedCenter = 1f;
//
//     private Vector3 _targetCenter = Vector3.zero;
//     private Vector3 _targetScale = Vector3.zero;
//
//     [Header("Движение по радиусу")]
//     [SerializeField] private float _amplitudeRadius = 2f;
//     [SerializeField] private float _speedAmplitudeRadius = 2f;
//
//     [Header("Начальные параметры")]
//     [SerializeField] private float _maxRadius = 1f;
//
//     [SerializeField] private string _walkAudioName = null;
//     [SerializeField] private string _cutAudioName = null;
//     [SerializeField] private HelperLineRenderer _helperLineRenderer = null;
//
//     private bool _isCircleMovement;
//     [SerializeField] private bool _isMovement = false;
//     [SerializeField] private float _speed = 5f;
//
//     private float _timerCount = 0f;
//     
//     private ChangeParam _radius = new ChangeParam();
//     private ChangeParam _current_speed = new ChangeParam();
//     private Vector3 _nextTarget = Vector3.zero;
//     private CLOCK_DIRECTION _direction = CLOCK_DIRECTION.Undefined;
//     private Rigidbody _rigidbody;
//     private Transform _centerTr;
//
//     enum CLOCK_DIRECTION
//     {
//         Undefined = 0,
//         Clock_Wise = 1,
//         Anti_Clock_Wise = 2
//     }
//
//     private void Awake()
//     {
//         _rigidbody = GetComponent<Rigidbody>();
//         //_centerTr = GameManager.Instance.Center.transform;
//     }
//
//     private void InitialiseMovement(Transform previewBlock)
//     {
//         _nextTarget = RandomNextTarget();
//
//         _radius.Value = (_centerTr.position - transform.position).magnitude;
//         _radius.Amplitude = _amplitudeRadius;
//         _radius.Speed = _speedAmplitudeRadius;
//         
//         _current_speed.Value = _speedСircle;
//         _current_speed.Amplitude = _amplitudeСircle;
//         _current_speed.Speed = _speedAmplitudeСircle;
//
//         _direction = (CLOCK_DIRECTION)UnityEngine.Random.Range(1,3);
//         
//         if (_direction == CLOCK_DIRECTION.Clock_Wise)
//         {
//             transform.position = new Vector3(-13f, 0f, 0f);
//             _timerCount = 4.75f;
//         }   
//         else
//         {
//             transform.position = new Vector3(0f, 0f, -13f);
//             _timerCount = 4.8f;
//         }
//         transform.localScale = previewBlock.localScale;
//
//         var _previewTr = previewBlock.transform;
//         var _previewLocalScale = _previewTr.localScale;
//         var size = _previewLocalScale.x >= _previewLocalScale.z
//             ? _previewLocalScale.x
//             : _previewLocalScale.z;
//
//         _targetScale = previewBlock.localScale;
//         _targetCenter = new Vector3(_previewTr.position.x, 0f, _previewTr.position.z);
//
//         _radius.Value = size / 2;
//         _radius.Amplitude = size / 5;
//         _maxRadius = size / 3;
//         _nextTarget = RandomNextTarget();
//
//         _kSpeedCenter = Mathf.Clamp((_previewTr.localScale.x * _previewTr.localScale.z) / 25f, 0.1f, 1f);
//         
//         _centerTr.position = _targetCenter;
//     }
//
//     private void InitialiseStage(MovementSettings settings)
//     {
//         _speedСircle = settings.SpeedCircle;
//         _amplitudeСircle = settings.AmplitudeCircle;
//         _speedAmplitudeСircle = settings.SpeedAmplitudeCircle;
//
//         _scatterFromEdge = settings.ScatterForEdge;
//
//         _speedCenter = settings.SpeedCenter;
//
//         _amplitudeRadius = settings.AmplitudeRadius;
//         _speedAmplitudeRadius = settings.SpeedAmplitudeRadius;
//     }
//
//     public void Initialise(Transform previewBlock)
//     {
//         InitialiseStage(GameManager.Instance.Property[GameManager.Instance.ScoreManager.Stage - 1]);
//         InitialiseMovement(previewBlock);
//
//         InputManager.Instance.EventTap += Stop;
//         AudioManager.Play(_walkAudioName);
//
//         _isMovement = true;
//         _isCircleMovement = false;
//     }
//
//     private void Update()
//     {
//         if(!_isMovement) return;
//
//         if (_isCircleMovement)
//         {
//             transform.position = _direction == CLOCK_DIRECTION.Clock_Wise ? Clockwise() : Anticlockwise();
//         }
//         else
//         {
//             var direction = _centerTr.position - transform.position;
//             var distance = direction.magnitude;
//             if (distance >= _maxRadius)
//             {
//                 transform.position += direction.normalized * (Time.deltaTime * _speed);
//                 _radius.Value = (_centerTr.position - transform.position).magnitude;
//             }
//             else 
//                 _isCircleMovement = true;
//         }
//
//         if (_isCircleMovement && (_nextTarget - _centerTr.position).sqrMagnitude < 0.01f)
//         {
//             _nextTarget = RandomNextTarget();
//         }
//         else 
//         {
//             //движение центральной координаты
//             var look = (_nextTarget - _centerTr.position).normalized;
//             _centerTr.Translate(look * (Time.deltaTime * _speedCenter * _kSpeedCenter));
//         }
//     }
//
//     private Vector3 RandomNextTarget()
//     {
//         var x = UnityEngine.Random.Range((_targetCenter.x - _targetScale.x / 2 - _scatterFromEdge), (_targetCenter.x + _targetScale.x / 2 + _scatterFromEdge));
//         var z = UnityEngine.Random.Range((_targetCenter.z + _targetScale.z / 2 + _scatterFromEdge), (_targetCenter.z - _targetScale.z / 2 - _scatterFromEdge));
//         return new Vector3(x, 0f, z);
//     }
//
//     private Vector3 Anticlockwise()
//     {
//         var cache_speed = _current_speed.Value;
//
//         _timerCount += Time.deltaTime * cache_speed;
//         var cache_radius = _radius.Value;
//
//         return new Vector3(_centerTr.position.x + Mathf.Cos(_timerCount) * cache_radius, transform.position.y, _centerTr.position.z + Mathf.Sin(_timerCount) * cache_radius);
//     }
//
//     private Vector3 Clockwise()
//     {
//         var cache_speed = _current_speed.Value;
//
//         _timerCount += Time.deltaTime * cache_speed;
//         var cache_radius = _radius.Value;
//
//         return new Vector3(_centerTr.position.x + Mathf.Sin(_timerCount) * cache_radius, transform.position.y, _centerTr.position.z + Mathf.Cos(_timerCount) * cache_radius);
//     }
//
//     public void Stop()
//     {
//         _isMovement = false;
//         // _rigidbody.useGravity = true;
//         // _rigidbody.isKinematic = false;
//         //
//         // StartCoroutine(ExitRound());
//         // GetComponent<Block>().Collision.RunLighting();
//         //
//         // if (InputManager.TryInstance != null)
//         //     InputManager.Instance.EventTap -= Stop;
//         //
//         // AudioManager.Stop();
//         // AudioManager.PlayOneShot(_cutAudioName);
//         //
//         // _helperLineRenderer.DisableHelper();
//     }
//
//     IEnumerator ExitRound()
//     {
//         yield return new WaitForSeconds(_timeExitRound);
//         EventExit?.Invoke();
//         // GetComponent<Block>().Collision.StopLighting();
//     }
//
//     // private void OnDestroy()
//     // {
//     //     if (InputManager.TryInstance != null)
//     //         InputManager.Instance.EventTap -= Stop;
//     // }
//
//     public void Clear()
//     {
//         _rigidbody.isKinematic = true;
//         _isMovement = false;
//         _isCircleMovement = false;
//     }
// }


