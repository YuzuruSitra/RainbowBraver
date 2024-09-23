using UnityEngine;

public class MaidGoToState : GoToRoomState
{
     // レイの距離
    private float _rayDistance = 1.0f;
    // 回避距離
    private const float AVOID_DISTANCE = 0.75f;

    public MaidGoToState(InnNPCMover mover) : base(mover)
    {
        _currentAvoid = AvoidPatterns.Move;
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
}

