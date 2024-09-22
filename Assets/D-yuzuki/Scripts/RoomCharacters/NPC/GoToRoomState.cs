using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // 回避パターン
    protected enum AvoidPatterns
    {
        Wait,
        Move
    }
    private InnNPCMover _innNpcMover;
    protected GameObject _npc;

    // 現在の回避パターン
    protected AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // ターゲットの位置
    protected Vector3 _targetPos;
    // 歩行フラグ
    private bool _isWalk;
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
    protected virtual bool IsAvoidingObstacle()
    {
        return false;
    }

}
