using System.Collections.Generic;
using UnityEngine;

public interface IRoomAIState
{
    bool IsStateFin { get; }
    bool IsWalk { get; }
    void EnterState(Vector3 targetPos, int targetRoom);
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
public class BraverController : MonoBehaviour
{
    [Header("キャラクターの部屋")]
    [SerializeField]
    private int _baseRoom = 0;
    public int BaseRoom => _baseRoom;
    [Header("移動速度")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [Header("目標座標に対する許容誤差")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;
    public float StoppingDistance => _stoppingDistance;
    [Header("滞在時間の最小値")]
    [SerializeField] 
    private float _minStayTime;
    [Header("滞在時間の最大値")]
    [SerializeField] 
    private float _maxStayTime;
    [Header("部屋内の障害物認知距離")]
    [SerializeField]
    private float _stayRoomRayLength;
    [Header("部屋移動中の障害物認知距離")]
    [SerializeField]
    private float _goToRoomRayLength;
    [Header("部屋移動中の回避終了距離")]
    [SerializeField]
    private float _goToAvoidDistance;

    private RoomAIState _currentState;
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    // 目標ルーム選定クラス
    private BraverRoomSelecter _braverRoomSelecter;
    private RoomPosAllocation _roomPosAllocation;
    //private Animator _animator;
    // 滞在中の部屋番号を保持
    private int _targetRoomNum;
    // 次の部屋番号を保持
    private int _nextRoomNum;
    // ターゲット座標を保持
    private Vector3 _targetPos;
    private bool _isCurrentWalk;
    // 行動制限
    public bool IsFreedom = true;
    // 移動用クラス
    private InnNPCMover _innNPCMover;
    public InnNPCMover InnNPCMover => _innNPCMover;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (!IsFreedom) return;
        _states[_currentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);

        if (_states[_currentState].IsStateFin) NextState(_currentState);
    }

    void InitializeNPC()
    {
        _innNPCMover = new InnNPCMover(gameObject, _moveSpeed, _stoppingDistance);
        _targetRoomNum = _baseRoom;
        _braverRoomSelecter = BraverRoomSelecter.Instance;
        _roomPosAllocation = RoomPosAllocation.Instance;
        //_animator = gameObject.GetComponent<Animator>();

        // 各状態のインスタンスを作成して登録
        _states.Add(RoomAIState.STAY_ROOM, new StayRoomState(_innNPCMover, _roomPosAllocation.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new ExitRoomState(_innNPCMover));
        _states.Add(RoomAIState.LEAVE_ROOM, new LeaveRoomState(_innNPCMover));
        _states.Add(RoomAIState.GO_TO_ROOM, new GoToRoomState(_innNPCMover));
        // STAY_ROOMから開始
        _currentState = RoomAIState.STAY_ROOM;
        _states[_currentState].EnterState(_roomPosAllocation.ErrorVector, _targetRoomNum);
    }

    // アニメーションの遷移
    void ChangeAnimWalk(bool isWalk)
    {
        /*
        if (_isCurrentWalk == isWalk) return;
        _animator.SetBool("IsWalk", isWalk);
        _isCurrentWalk = isWalk;
        */
    }

    void NextState(RoomAIState state)
    {
        RoomAIState newState;
        switch (state)
        {
            case RoomAIState.STAY_ROOM:
                _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(_baseRoom, _targetRoomNum);
                newState = RoomAIState.EXIT_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.EXIT_POINT, transform.position.y);
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                // 部屋の移動
                _targetRoomNum = _nextRoomNum;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.IN_POINT, transform.position.y);
                break;
            default:
                newState = state;
                break;
        }
        if (_nextRoomNum == RoomBunker.ERROR_ROOM_NUM && state == RoomAIState.STAY_ROOM)
        {
            _targetPos = _roomPosAllocation.ErrorVector;
            newState = RoomAIState.STAY_ROOM;
        }
        
        _states[newState].EnterState(_targetPos, _targetRoomNum);
        _currentState = newState;
    }

    // 外部からの参照
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _targetPos);
    }

    public void FinWarpHandler(int currentRoom)
    {
        _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(_baseRoom, currentRoom);
        NextState(RoomAIState.EXIT_ROOM);
    }

}
