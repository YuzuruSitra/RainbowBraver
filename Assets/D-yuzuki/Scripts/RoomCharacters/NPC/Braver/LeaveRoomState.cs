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

    // �X�e�[�g�ɓ��������̏���
    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isStateFin = false;
        _isWalk = true;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        direction.y = 0f;
        _npc.transform.position += direction * _moveSpeed * Time.deltaTime;

        // �^�[�Q�b�g�̕���������
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }

        MonitorStateExit();
    }

    // �X�e�[�g�̏I�����Ď�
    public void MonitorStateExit()
    {
        Vector3 tmp1 = _npc.transform.position;
        tmp1.y = 0;
        Vector3 tmp2 = _targetPos;
        tmp2.y = 0;
        if (Vector3.Distance(tmp1, tmp2) <= _distance) _isStateFin = true;
    }

}