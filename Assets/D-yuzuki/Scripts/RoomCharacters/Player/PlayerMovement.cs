using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("歩行速度")]
    [SerializeField] private float _walkSpeed;
    [Header("走行速度")]
    [SerializeField] private float _runSpeed;
    [Header("重力係数")]
    [SerializeField] private float _gravity;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _currentDirection;
    public Vector3 CurrentDirection => _currentDirection;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        UpdateCurrentDirection();
    }

    private void HandleMovement()
    {
        // X軸の移動入力
        _moveDirection.x = Input.GetAxis("Horizontal");
        // Z軸の移動入力は、X軸がゼロのときのみ受け付ける
        _moveDirection.z = Input.GetAxis("Vertical");
        // 地面にいるならY軸方向の移動をリセット
        if (_controller.isGrounded) _moveDirection.y = 0;

        _moveDirection.y -= _gravity * Time.deltaTime;
        // 走行か歩行かの速度を決定
        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
        // プレイヤーを移動
        _controller.Move(_moveDirection * speed * Time.deltaTime);
    }

    private void UpdateCurrentDirection()
    {
        // 現在の移動方向を更新 (Y軸は無視)
        Vector3 normalizedDirection = _moveDirection;
        normalizedDirection.y = 0;
        
        if (normalizedDirection != Vector3.zero)
        {
            _currentDirection = normalizedDirection.normalized;
            var x = _currentDirection.x;
            if (x != 0 && x != 0)
            {
                _currentDirection.x = x > 0 ? 1 : -1;
                _currentDirection.z = 0;
            }
        }
    }
}
