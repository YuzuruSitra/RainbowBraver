using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private Button _changeViewButton;
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private RoomSelecter _roomSelecter;
    [SerializeField]
    private CinemachineVirtualCameraBase _overallView;
    [SerializeField]
    private CinemachineVirtualCameraBase _playerFollow;
    [SerializeField]
    private CinemachineVirtualCameraBase _playerInRoom;


    private bool _isPlayerFollow;
    private CinemachineVirtualCameraBase _currentFollow;

    // Start is called before the first frame update
    void Start()
    {
        _overallView.Priority = 2;
        _playerFollow.Priority = 0;
        _playerInRoom.Priority = 0;
        _isPlayerFollow = false;
        _currentFollow = _playerFollow;

        _playerMovement.ActionInRoom += ChangeFollowCam;
        _changeViewButton.onClick.AddListener(ChangeCamView);
    }

    private void ChangeCamView()
    {
        if (_isPlayerFollow)
        {
            _overallView.Priority = 1;
            _currentFollow.Priority = 0;
        }
        else
        {
            _overallView.Priority = 0;
            _currentFollow.Priority = 1;
        }
        _isPlayerFollow = !_isPlayerFollow;
    }

    private void ChangeFollowCam(bool inRoom)
    {
        _currentFollow.Priority = 0;
        if (!inRoom)
        {
            _playerFollow.Priority = 1;
            _currentFollow = _playerFollow;
            ChangeVisibilityWall(true);
        }
        else
        {
            _playerInRoom.Follow = _roomSelecter.RoomDetails[_playerMovement.CurentRoomNum].transform;
            _playerInRoom.Priority = 1;
            _currentFollow = _playerInRoom;
            ChangeVisibilityWall(false);
        }
    }

    // ï«ÇÃâ¬éãê´ïœçX
    private void ChangeVisibilityWall(bool state)
    {
        _roomSelecter.RoomDetails[_playerMovement.CurentRoomNum].FrontWall.enabled = state;
        _roomSelecter.RoomDetails[_playerMovement.CurentRoomNum].FrontDoor.enabled = state;
    }
}
