using UnityEngine;

public class StayRoomState : IRoomAIState
{
    private Vector3 _targetPos;
    private Vector3 _errorVector;
    private GameObject _npc;
    private bool _isEntry;
    private bool _wallHit;
    private float _changeTime;
    private int _remainState;
    private float _roomMoveSpeed;
    private float _roomRotSpeed;
    private float _distance;
    private Vector3 _remainDirection;
    private float _rayDistance;
    private const float MAX_ANGLE = 180.0f;
    private const int MAX_ATTEMP = 100;
    private float _stateMinTime;
    private float _stateMaxTime;
    private float _remainStateTime;
    private bool _isWalk;
    private bool _isStateFin;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => _isStateFin;

    public StayRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance, float ray, float minTime, float maxTime, Vector3 errorVector)
    {
        _npc = npc;
        _roomMoveSpeed = moveSpeed;
        _roomRotSpeed = rotSpeed;
        _distance = distance;
        _rayDistance = ray;
        _stateMinTime = minTime;
        _stateMaxTime = maxTime;
        _errorVector = errorVector;
    }

    // ステートに入った時の処理
    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isEntry = false;
        if (_targetPos == _errorVector) _isEntry = true;
        _isStateFin = false;
        _remainStateTime = Random.Range(_stateMinTime, _stateMaxTime);
        SetParam();
    }

    // ステートの更新

    public void UpdateState()
    {
        // 入場
        if (!_isEntry)
        {
            _isWalk = true;
            Vector3 direction = (_targetPos - _npc.transform.position).normalized;
            _npc.transform.position += direction * _roomMoveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _roomRotSpeed * Time.deltaTime);

            if (Vector3.Distance(_npc.transform.position, _targetPos) <= _distance) _isEntry = true;
            return;
        }

        // 自由歩行
        _changeTime -= Time.deltaTime;
        if (_changeTime <= 0) SetParam();

        switch (_remainState)
        {
            case 0: // 停止
                _isWalk = false;
                break;
            case 1: // 歩行
                if (_remainDirection == Vector3.zero)
                {
                    _remainState = 0;
                    break;
                }
                // 前方に障害物があれば回避
                AvoidObstacle();
                // 前方に歩行
                MoveForward();
                _isWalk = true;

                break;
        }

        // ステートカウントダウン
        _remainStateTime -= Time.deltaTime;
        if (_remainStateTime <= 0) _isStateFin = true;
    }

    private void SetParam()
    {
        _remainState = Random.Range(0, 2);
        _changeTime = Random.Range(1, 3);
        if(_remainState == 1) SetRandomDirection();
    }

    private void SetRandomDirection()
    {
        int attempts = 0;
        while (attempts < MAX_ATTEMP)
        {
            float randomAngle = Random.Range(-MAX_ANGLE, MAX_ANGLE);
            Vector3 randomDirection = Quaternion.AngleAxis(randomAngle, _npc.transform.up) * _npc.transform.forward;

            RaycastHit hit;
            if (!Physics.Raycast(_npc.transform.position, randomDirection, out hit, _rayDistance * 2))
            {
                // 確率で停止
                int selectRnd = Random.Range(0, 3);
                if (selectRnd == 0)
                {
                    _remainState = 0;
                }
                else
                {
                    _remainDirection = randomDirection.normalized;
                }
                return;
            }

            attempts++;
        }

        _remainDirection = Vector3.zero;
    }

    private void AvoidObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out hit, _rayDistance))
        {
            if (hit.collider.CompareTag("Wall") && !_wallHit)
            {
                SetRandomDirection();
                _wallHit = true;
            }
            else
            {
                _wallHit = false;
            }
        }
    }

    private void MoveForward()
    {
        _npc.transform.position += _remainDirection * _roomMoveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(-_remainDirection);
        _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _roomRotSpeed * Time.deltaTime);
    }
}
