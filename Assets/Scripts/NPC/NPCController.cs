using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoomAIState
{
    bool IsStateFin { get; }
    bool IsWalk { get; }
    void EnterState(Vector3 targetPos);
    void UpdateState();
}

public enum RoomAIState
{
    STAY_ROOM,
    EXIT_ROOM,
    LEAVE_ROOM,
    GO_TO_ROOM
}

// ルームNPCの制御クラス
public class NPCController : MonoBehaviour
{
    [Header("キャラクターの部屋")]
    [SerializeField]
    private int _baseRoom = 0;
    [Header("移動速度")]
    [SerializeField] 
    private float _moveSpeed;
    [Header("回転速度")]
    [SerializeField] 
    private float _rotationSpeed;
    [Header("部屋内の速度低下係数")]
    [SerializeField]
    private float _roomFriction;
    [Header("目標座標に対する許容誤差")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;
    [Header("滞在時間の最小値")]
    [SerializeField] 
    private float _minStayTime;
    [Header("滞在時間の最大値")]
    [SerializeField] 
    private float _maxStayTime;
    [Header("壁の認知距離")]
    [SerializeField]
    private float _rayLength;

    private RoomAIState _currentState;
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    // 目標ルーム選定クラス
    private RoomSelecter _roomSelecter;
    private Animator _animator;
    // 滞在中の部屋を保持
    private RoomDetails _currentRoom;
    // ターゲット座標を保持
    private Vector3 _targetPos = Vector3.zero;
    private bool _isCurrentWalk;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        _states[_currentState].UpdateState();
        ChangeAnimWalk(_states[_currentState].IsWalk);
        if (_states[_currentState].IsStateFin) NextState();
    }

    void InitializeNPC()
    {
        _roomSelecter = GameObject.FindWithTag("RoomSelecter").GetComponent<RoomSelecter>();
        _animator = gameObject.GetComponent<Animator>();
        _currentRoom = _roomSelecter.OutRemainRoom(_baseRoom);

        // 各状態のインスタンスを作成して登録
        _states.Add(RoomAIState.STAY_ROOM, new StayRoomState(gameObject, _moveSpeed * _roomFriction, _rotationSpeed * _roomFriction, _stoppingDistance, _rayLength, _minStayTime, _maxStayTime));
        _states.Add(RoomAIState.EXIT_ROOM, new ExitRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance));
        _states.Add(RoomAIState.LEAVE_ROOM, new LeaveRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance));
        _states.Add(RoomAIState.GO_TO_ROOM, new GoToRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance));
        // STAY_ROOMから開始
        _currentState = RoomAIState.STAY_ROOM;
        _states[_currentState].EnterState(_targetPos);
    }

    // アニメーションの遷移
    void ChangeAnimWalk(bool isWalk)
    {
        if (_isCurrentWalk == isWalk) return;
        _animator.SetBool("IsWalk", isWalk);
        _isCurrentWalk = isWalk;
    }

    void NextState()
    {
        RoomAIState newState;
        switch (_currentState)
        {
            case RoomAIState.STAY_ROOM:
                newState = RoomAIState.EXIT_ROOM;
                if (_currentRoom == null) break;
                _targetPos.x = _currentRoom.RoomExitPoints.position.x;
                _targetPos.y = transform.position.y;
                _targetPos.z = _currentRoom.RoomExitPoints.position.z;
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                if (_currentRoom == null) break;
                _targetPos.x = _currentRoom.RoomOutPoints.position.x;
                _targetPos.y = transform.position.y;
                _targetPos.z = _currentRoom.RoomOutPoints.position.z;
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                _currentRoom = _roomSelecter.SelectTargetRoom(_baseRoom, _currentRoom.RoomNum);
                if (_currentRoom == null) break;
                _targetPos.x = _currentRoom.RoomOutPoints.position.x;
                _targetPos.y = transform.position.y;
                _targetPos.z = _currentRoom.RoomOutPoints.position.z;
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                if (_currentRoom == null) break;
                _targetPos.x = _currentRoom.RoomInPoints.position.x;
                _targetPos.y = transform.position.y;
                _targetPos.z = _currentRoom.RoomInPoints.position.z;
                break;
            default:
                newState = _currentState;
                break;
        }
        if (_currentRoom == null && _currentState == RoomAIState.STAY_ROOM)
        {
            _targetPos = Vector3.zero;
            newState = RoomAIState.STAY_ROOM;
        }
        _states[newState].EnterState(_targetPos);
        _currentState = newState;
    }

}
