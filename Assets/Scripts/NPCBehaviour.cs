using UnityEngine;

// NPCの制御
public class NPCBehaviour : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed;
    [Header("目標座標に対する許容誤差")]
    public float _stoppingDistance = 0.1f;
    [Header("滞在時間の最小値")]
    public float _minStayTime = 3f;
    [Header("滞在時間の最大値")]
    public float _maxStayTime = 5f;
    [Header("キャラクターの部屋")]
    public int _baseRoom = 0;

    // キャラクターのステート
    public enum CharacterState
    {
        STAY_ROOM,
        LEAVE_ROOM,
        GO_TO_ROOM,
        ENTRY_ROOM
    }
    private CharacterState _currentState;

    // 目標ルーム選定クラス
    private RoomSelecter _roomSelecter;
    // 部屋滞在時間を保持
    private float _remainStayTime;
    // 滞在中の部屋を保持
    private RoomDetails _currentRoom;
    // ターゲット座標を保持
    private Transform _targetPos;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (_currentState != CharacterState.STAY_ROOM)
            MoveToTarget();
        else
            UpdateStayRoomState();
    }

    void InitializeNPC()
    {
        _roomSelecter = GameObject.FindWithTag("RoomSelecter").GetComponent<RoomSelecter>();
        _currentState = CharacterState.STAY_ROOM;
        _currentRoom = _roomSelecter.OutRemainRoom(_baseRoom);
    }

    void UpdateStayRoomState()
    {
        if (_currentRoom == null) return;

        _remainStayTime -= Time.deltaTime;

        if (_remainStayTime <= 0)
            NextState();
    }

    void NextState()
    {
        switch (_currentState)
        {
            case CharacterState.STAY_ROOM:
                _currentState = CharacterState.LEAVE_ROOM;
                _targetPos = _currentRoom.RoomOutPoints;
                break;
            case CharacterState.LEAVE_ROOM:
                _currentState = CharacterState.GO_TO_ROOM;
                _currentRoom = _roomSelecter.SelectTargetRoom(_baseRoom, _currentRoom.RoomNum);
                if (_currentRoom == null)
                    _currentState = CharacterState.STAY_ROOM;
                else
                    _targetPos = _currentRoom.RoomOutPoints;
                break;
            case CharacterState.GO_TO_ROOM:
                _currentState = CharacterState.ENTRY_ROOM;
                _targetPos = _currentRoom.RoomInPoints;
                break;
            case CharacterState.ENTRY_ROOM:
                _currentState = CharacterState.STAY_ROOM;
                _remainStayTime = Random.Range(_minStayTime, _maxStayTime);
                break;
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (_targetPos.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _targetPos.position) <= _stoppingDistance)
            NextState();
    }
}
