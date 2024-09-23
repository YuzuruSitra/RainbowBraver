using System.Collections.Generic;
using UnityEngine;

// ルームNPCの制御クラス
public class BraverController : MonoBehaviour
{
    public int BraverNum { get; private set; }

    public int BaseRoom { get; private set; }

    [Header("移動速度")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [Header("目標座標に対する許容誤差")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;

    public RoomAIState CurrentState { get; private set; }
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    // 目標ルーム選定クラス
    private BraverRoomSelecter _braverRoomSelecter;
    private RoomPosAllocation _roomPosAllocation;
    //private Animator _animator;
    // 滞在中の部屋番号を保持
    public int StayRoomNum { get; private set; }
    // 次の部屋番号を保持
    private int _nextRoomNum;
    // ターゲット座標を保持
    private Vector3 _targetPos;
    // 行動制限
    public bool IsFreedom { get; set; }
    public InnNPCMover InnNPCMover { get; private set; }
    
    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (!IsFreedom) return;
        _states[CurrentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);
        if (_states[CurrentState].IsStateFin) NextState(CurrentState);
    }

    void InitializeNPC()
    {
        InnNPCMover = new InnNPCMover(gameObject, _moveSpeed, _stoppingDistance);
        StayRoomNum = BaseRoom;
        _braverRoomSelecter = BraverRoomSelecter.Instance;
        _roomPosAllocation = RoomPosAllocation.Instance;
        //_animator = gameObject.GetComponent<Animator>();

        // 各状態のインスタンスを作成して登録
        _states.Add(RoomAIState.STAY_ROOM, new BraverStayState(InnNPCMover, _roomPosAllocation.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new BraverExitState(InnNPCMover));
        _states.Add(RoomAIState.LEAVE_ROOM, new BraverLeaveState(InnNPCMover));
        _states.Add(RoomAIState.GO_TO_ROOM, new BraverGoToState(InnNPCMover));
        // STAY_ROOMから開始
        CurrentState = RoomAIState.STAY_ROOM;
        _states[CurrentState].EnterState(_roomPosAllocation.ErrorVector, StayRoomNum);
        IsFreedom = true;
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
                _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(BaseRoom, StayRoomNum);
                newState = RoomAIState.EXIT_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.EXIT_POINT, transform.position.y);
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                // 部屋の移動
                StayRoomNum = _nextRoomNum;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.IN_POINT, transform.position.y);
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
        _states[CurrentState].ExitState();
        _states[newState].EnterState(_targetPos, StayRoomNum);
        CurrentState = newState;
    }

    // 外部からの参照
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _targetPos);
    }

    public void FinWarpHandler(int currentRoom)
    {
        _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(BaseRoom, currentRoom);
        NextState(RoomAIState.EXIT_ROOM);
    }

    public void SetNumber(int braverNum, int roomNum)
    {
        BraverNum = braverNum;
        BaseRoom = roomNum;
    }

}
