using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    private enum AvoidPatterns
    {
        Wait,
        Move
    }
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    private GameObject _npc;
    private float _moveSpeed;
    private float _rotSpeed;
    private float _distance;
    private Vector3 _targetPos;
    private bool _isWalk;
    private bool _isStateFin;
    private float _rayDistance;

    private GameObject _currentHitObj;
    private float _avoidRotFactor;
    private Vector3 _avoidDirection;
    private float _avoidDistance;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => _isStateFin;

    public GoToRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance, float ray, float avoidRot, float avoidDistance)
    {
        _npc = npc;
        _moveSpeed = moveSpeed;
        _rotSpeed = rotSpeed;
        _distance = distance;
        _rayDistance = ray;
        _avoidRotFactor = avoidRot;
        _avoidDistance = avoidDistance;
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
        if (IsAvoid() && _currentAvoid == AvoidPatterns.Move) AvoidMoving();
        else if (IsAvoid() && _currentAvoid == AvoidPatterns.Wait) AvoidWaiting();
        else DefaultMoving();
        // if (Vector3.Distance(_npc.transform.position, _targetPos) <= _distance) _isStateFin = true;
    }

    private void DefaultMoving()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        _npc.transform.position += direction * _moveSpeed * Time.deltaTime;

        // ターゲットの方向を向く
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    private void AvoidMoving()
    {
        _npc.transform.position += _avoidDirection * _moveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(-_avoidDirection);
        _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
    }

    private void AvoidWaiting()
    {
        // ターゲットの方向を向く
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    private bool IsAvoid()
    {
        Vector3[] directions = {
        Quaternion.AngleAxis(-_avoidRotFactor, Vector3.up) * -_npc.transform.forward,
        -_npc.transform.forward,
        Quaternion.AngleAxis(_avoidRotFactor, Vector3.up) * -_npc.transform.forward, };

        foreach (Vector3 direction in directions)
        {
            Debug.DrawRay(_npc.transform.position, direction * _rayDistance, Color.red);
            if (Physics.Raycast(_npc.transform.position, direction, out RaycastHit hit, _rayDistance) &&
                hit.collider.CompareTag("RoomNPC") && _currentHitObj == null)
            {
                _currentHitObj = hit.collider.gameObject;
                _currentAvoid = AvoidChoose(_currentHitObj);
                if (_currentAvoid == AvoidPatterns.Move) SetAvoidDirection();
                else return true;
            }
        }


        if (_currentHitObj == null) return false;

        if (IsTooFarFromObstacle())
        {
            _currentHitObj = null;
            return false;
        }
        
        return true;
    }

    private void SetAvoidDirection()
    {
        _avoidDirection = Quaternion.AngleAxis(_avoidRotFactor, Vector3.up) * -_npc.transform.forward;
        _avoidDirection.Normalize();
    }

    private bool IsTooFarFromObstacle()
    {
        float distanceX = Mathf.Abs(_currentHitObj.transform.position.x - _npc.transform.position.x);
        return distanceX > _avoidDistance;
    }

    private AvoidPatterns AvoidChoose(GameObject otherObj)
    {
        NPCController otherNPCController = _currentHitObj.GetComponent<NPCController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        if (otherDistance < thisDistance)
            return AvoidPatterns.Wait;
        else
            return AvoidPatterns.Move;
    }

}