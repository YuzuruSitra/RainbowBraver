using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // 回避パターン
    private enum AvoidPatterns
    {
        Wait,
        Move
    }
    private InnNPCMover _innNpcMover;
    private GameObject _npc;

    // 現在の回避パターン
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // ターゲットの位置
    private Vector3 _targetPos;
    // 歩行フラグ
    private bool _isWalk;
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
    private bool _launchState;
    // ステート終了フラグのgetter
    public bool IsStateFin => _innNpcMover.IsAchieved && _launchState;
    private bool _currentDirection = true;

    public GoToRoomState(InnNPCMover mover)
    {
        _innNpcMover = mover;
        _npc = _innNpcMover.Character;
    }

    public void EnterState(Vector3 pos, int targetRoom)
    {
        _targetPos = pos;
        _innNpcMover.SetTarGetPos(_targetPos);
        _launchState = true;
    }

    public void UpdateState()
    {
        if (IsAvoidingObstacle())
            if (_currentAvoid == AvoidPatterns.Wait) 
                _isWalk = false;
            else 
                AvoidMoving();
        else
            DefaultMoving();
    }

    public void ExitState()
    {
        _launchState = false;
    }

    // デフォルトの移動
    private void DefaultMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos);
        _innNpcMover.Moving();
    }

    // 回避時の移動
    private void AvoidMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos, reverseDirection: true);
        _innNpcMover.Moving();
    }

    // ターゲットに向かって移動
    private void MoveTowardsTarget(Vector3 target, bool reverseDirection = false)
    {
        if (_currentDirection == reverseDirection) return;
        _currentDirection = reverseDirection;
        _innNpcMover.SetTarGetPos((reverseDirection ? -target : target));
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
}
