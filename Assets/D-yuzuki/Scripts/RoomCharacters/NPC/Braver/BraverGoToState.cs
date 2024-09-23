using UnityEngine;

public class BraverGoToState : GoToRoomState
{
     // レイの距離
    private float _rayDistance = 1.5f;
    // 回避距離
    private const float AVOID_DISTANCE = 1.5f;
    // 回避ステート判定閾値
    private const float AVOID_THRESHOLD = 0.0f;

    public BraverGoToState(InnNPCMover mover) : base(mover)
    {

    }

    // 現在の衝突オブジェクト
    private GameObject _currentHitObj;

    protected override bool IsAvoidingObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance) &&
            hit.collider.CompareTag("RoomBraver") && _currentHitObj == null)
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
        if (distanceX > AVOID_DISTANCE)
        {
            _currentHitObj = null;
            return false;
        }
        return true;
    }

    // 回避をキャンセルすべきかを判断
    private bool ShouldCancelAvoidance(GameObject otherObj)
    {
        var isCancel = false;
        var otherNPCController = _currentHitObj.GetComponent<BraverController>();
        var otherDistance = otherNPCController.GetDistanceToTarget();
        var thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        if (thisDistance < otherDistance)
        {
            _currentHitObj = null;
            isCancel = true;
        }
        
        return isCancel;
    }

    // 回避パターンを決定
    private void DecideAvoidancePattern(GameObject otherObj)
    {
        var otherNPCController = _currentHitObj.GetComponent<BraverController>();
        var otherDistance = otherNPCController.GetDistanceToTarget();
        var targetDistance = Vector3.Distance(_npc.transform.position, otherObj.transform.position);
        
        // 閾値に基づいてパターンを決定するロジックを調整
        _currentAvoid = (targetDistance - AVOID_THRESHOLD >= otherDistance) ? AvoidPatterns.Wait : AvoidPatterns.Move;
    }
}

