using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

// ÉJÉÅÉâêßå‰
public class CamController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private CinemachineVirtualCameraBase _overallView;
    [SerializeField]
    private CinemachineVirtualCameraBase _playerFollow;
    [SerializeField]
    private CinemachineVirtualCameraBase _playerInRoom;

    [SerializeField]
    private RoomBunker _roomBunker;
    private bool _isPlayerFollow;
    private CinemachineVirtualCameraBase _currentFollow;
    private VisibilityHandler _visibilityHandler;

    // Start is called before the first frame update
    void Start()
    {
        _overallView.Priority = 2;
        _playerFollow.Priority = 0;
        _playerInRoom.Priority = 0;
        _isPlayerFollow = false;
        _currentFollow = _playerFollow;
        _visibilityHandler = VisibilityHandler.Instance;

        _playerMovement.ActionInRoom += ChangeFollowCam;
    }

    public void ChangeCamView()
    {
        _isPlayerFollow = !_isPlayerFollow;
        _overallView.Priority = _isPlayerFollow ? 0 : 1;
        _currentFollow.Priority = _isPlayerFollow ? 1 : 0;
    }

    private void ChangeFollowCam(bool inRoom)
    {
        _currentFollow.Priority = 0;
        if (!inRoom)
        {
            _playerFollow.Priority = 1;
            _currentFollow = _playerFollow;
        }
        else
        {
            _playerInRoom.Follow = _roomBunker.RoomDetails[_playerMovement.CurentRoomNum].transform;
            _playerInRoom.Priority = 1;
            _currentFollow = _playerInRoom;
        }
        _visibilityHandler.ChangeTargetRoom(!inRoom, _playerMovement.CurentRoomNum);
    }

}
