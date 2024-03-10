using UnityEngine;

public class LeaveRoomState: IRoomAIState
{
    private GameObject _npc;
    private float _moveSpeed;
    private float _rotSpeed;
    private float _distance;
    private Vector3 _targetPos;
    private bool _isWalk;
    private bool _isStateFin;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => _isStateFin;

    public LeaveRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance)
    {
        _npc = npc;
        _moveSpeed = moveSpeed;
        _rotSpeed = rotSpeed;
        _distance = distance;
    }

    // ステートに入った時の処理
    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isStateFin = false;
        _isWalk = true;
    }

    // ステートの更新
    public void UpdateState()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        direction.y = 0f;
        _npc.transform.position += direction * _moveSpeed * Time.deltaTime;

        // ターゲットの方向を向く
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(_npc.transform.position, _targetPos) <= _distance) _isStateFin = true;
    }

}
