using Cinemachine;
using UnityEngine;

// ÉJÉÅÉâêßå‰
public class CamController : MonoBehaviour
{
    private enum CamState
    {
        PLAYER_IN_ROOM,
        OVERAL_VIEW,
        PLAYER_FOLLOW
    }
    private CamState _currentDefaultCam;
    private bool _playerInRoom;

    [System.Serializable]
    private struct CameraStatePair
    {
        public CamState state;
        public CinemachineVirtualCameraBase camera;
    }

    [SerializeField]
    private CameraStatePair[] _vCams;

    private const int LOW_PRIORITY = 0;
    private const int HIGH_PRIORITY = 1;

    [SerializeField]
    private PlayerRoomCol _playerRoomCol;
    [SerializeField]
    private RoomBunker _roomBunker;
    private VisibilityHandler _visibilityHandler;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCamView(CamState.OVERAL_VIEW);
        _visibilityHandler = VisibilityHandler.Instance;

        _playerRoomCol.ActionInRoom += ChangeRoomCam;
    }

    private void ChangeCamView(CamState newState)
    {
        foreach (CameraStatePair pair in _vCams)
        {
            pair.camera.Priority = pair.state == newState ? HIGH_PRIORITY : LOW_PRIORITY;
            if (pair.state == CamState.PLAYER_IN_ROOM && newState == CamState.PLAYER_IN_ROOM)
                pair.camera.Follow = _roomBunker.RoomDetails[_playerRoomCol.CurentRoomNum].transform;
        }
    }

    private void ChangeRoomCam(bool inRoom)
    {
        _playerInRoom = inRoom;
        ChangeCamView(inRoom ? CamState.PLAYER_IN_ROOM : _currentDefaultCam);
        _visibilityHandler.ChangeTargetRoom(!inRoom, _playerRoomCol.CurentRoomNum);
    }

    public void DefaultCamChanger()
    {
        if (_playerInRoom) return;

        _currentDefaultCam = (_currentDefaultCam == CamState.OVERAL_VIEW) ? CamState.PLAYER_FOLLOW : CamState.OVERAL_VIEW;
        ChangeCamView(_currentDefaultCam);
    }

}
