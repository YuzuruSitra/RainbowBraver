using UnityEngine;

public class InnNPCMover
{
    private Vector3 _targetPos;
    private const float GRAVITY = 20.0f;
    private float _speed;
    private GameObject _character;
    public GameObject Character => _character;
    private CharacterController _controller;
    private float _distance;
    public bool IsAchieved => IsMoveAchieved();

    public InnNPCMover(GameObject character, float speed, float distance)
    {
        _character = character;
        _controller = _character.GetComponent<CharacterController>();
        _speed = speed;
        _distance = distance;
    }

    public void SetTarGetPos(Vector3 targetPos)
    {
        _targetPos = targetPos;
    }
    
    public void Moving()
    {
        Vector3 direction = (_targetPos - _character.transform.position).normalized;
        // “ü—Í
        if (_controller.isGrounded) direction.y = 0;

        direction.y -= GRAVITY * Time.deltaTime;
        _controller.Move(direction * _speed * Time.deltaTime);
    }

    private bool IsMoveAchieved()
    {
        Vector3 tmp1 = _character.transform.position;
        tmp1.y = 0;
        Vector3 tmp2 = _targetPos;
        tmp2.y = 0;
        return (Vector3.Distance(tmp1, tmp2) <= _distance);
    }

}
