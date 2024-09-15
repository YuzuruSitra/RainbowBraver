using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isFlipped;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isFlipped = _spriteRenderer.flipX;
    }

    void Update()
    {
        HandleSpriteFlip();
    }

    private void HandleSpriteFlip()
    {
        if (_playerMovement.CurrentDirection.x > 0 && !_isFlipped)
        {
            _spriteRenderer.flipX = true;
            _isFlipped = true;
        }
        else if (_playerMovement.CurrentDirection.x < 0 && _isFlipped)
        {
            _spriteRenderer.flipX = false;
            _isFlipped = false;
        }
    }

    private void AnimHandler()
    {
        // _animator.SetBool("IsIdle", true); 
    }
}
