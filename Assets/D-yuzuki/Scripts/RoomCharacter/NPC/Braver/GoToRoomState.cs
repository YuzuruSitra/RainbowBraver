using Unity.VisualScripting;
using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // 回避パターン
    private enum AvoidPatterns
    {
        Wait,
        Move
    }

    // 現在の回避パターン
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // 自身のルームナンバー
    private int _roomNum;
    // NPCオブジェクト
    private GameObject _npc;
    // 移動速度
    private float _moveSpeed;
    // 回転速度
    private float _rotSpeed;
    // 目標までの距離
    private float _distance;
    // ターゲットの位置
    private Vector3 _targetPos;
    // 歩行フラグ
    private bool _isWalk;
    // ステート終了フラグ
    private bool _isStateFin;
    // レイの距離
    private float _rayDistance;
    // 現在の衝突オブジェクト
    private GameObject _currentHitObj;
    // 回避距離
    private float _avoidDistance;
    // 回避ステート判定閾値
    private const float AVOID_THRESHOLD = 1.5f;
    // 歩行フラグのgetter
    public bool IsWalk => _isWalk;
    // ステート終了フラグのgetter
    public bool IsStateFin => _isStateFin;

    public GoToRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance, float ray, float avoidDistance, int room)
    {
        _npc = npc;
        _moveSpeed = moveSpeed;
        _rotSpeed = rotSpeed;
        _distance = distance;
        _rayDistance = ray;
        _avoidDistance = avoidDistance;
        _roomNum = room;
    }

    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isStateFin = false;
    }

    public void UpdateState()
    {
        if (IsAvoidingObstacle())
        {
            if (_currentAvoid == AvoidPatterns.Wait) AvoidWaiting();
            else AvoidMoving();
        }
        else
        {
            DefaultMoving();
            MonitorStateExit();
        }
    }

    // デフォルトの移動
    private void DefaultMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos);
    }

    // 回避時の移動
    private void AvoidMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos, reverseDirection: true);
    }

    // ターゲットに向かって移動
    private void MoveTowardsTarget(Vector3 target, bool reverseDirection = false)
    {
        Vector3 direction = (target - _npc.transform.position).normalized;
        direction.y = 0f;

        _npc.transform.position += (reverseDirection ? -direction : direction) * _moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(reverseDirection ? direction : -direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    // 待機時の移動
    private void AvoidWaiting()
    {
        _isWalk = false;
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    // 障害物を回避するかどうかを判定
    private bool IsAvoidingObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance) &&
            hit.collider.CompareTag("RoomNPC") && _currentHitObj == null)
        {
            _currentHitObj = hit.collider.gameObject;
            
            if (ShouldCancelAvoidance(_currentHitObj)) return false;
            DecideAvoidancePattern(_currentHitObj);
        }

        return CheckObstacleTooFar();
    }

    // 障害物が離れすぎていないかを確認
    private bool CheckObstacleTooFar()
    {
        if (_currentHitObj == null) return false;
        float distanceX = Vector3.Distance(_npc.transform.position, _currentHitObj.transform.position);
        if (distanceX > _avoidDistance)
        {
            _currentHitObj = null;
            return false;
        }

        return true;
    }

    // 回避をキャンセルすべきかを判断
    private bool ShouldCancelAvoidance(GameObject otherObj)
    {
        bool isCancel = false;
        BraverController otherNPCController = _currentHitObj.GetComponent<BraverController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        if (thisDistance < otherDistance)
        {
            _currentHitObj = null;
            isCancel = true;
        }
        else if (thisDistance == otherDistance)
        {
            if (otherNPCController.BaseRoom < otherNPCController.BaseRoom) isCancel = true;
        }
        
        return isCancel;
    }

    // 回避パターンを決定
    private void DecideAvoidancePattern(GameObject otherObj)
    {
        BraverController otherNPCController = _currentHitObj.GetComponent<BraverController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float targetDistance = Vector3.Distance(_npc.transform.position, otherObj.transform.position);
        _currentAvoid = targetDistance - AVOID_THRESHOLD >= otherDistance ? AvoidPatterns.Wait : AvoidPatterns.Move;
    }

    // ステートの終了を監視
    public void MonitorStateExit()
    {
        Vector3 tmp1 = _npc.transform.position;
        tmp1.y = 0;
        Vector3 tmp2 = _targetPos;
        tmp2.y = 0;
        if (Vector3.Distance(tmp1, tmp2) <= _distance) _isStateFin = true;
    }
}
