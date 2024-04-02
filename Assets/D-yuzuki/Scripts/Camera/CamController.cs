using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CamState
{
    PLAYER_IN_ROOM,
    OVERAL_VIEW,
    PLAYER_FOLLOW
}

// カメラ制御
public class CamController : MonoBehaviour
{
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

    private event Action<CamState> _actionChangeCam;

    [SerializeField]
    private PlayerInRoomHandler _playerInRoomHandler;
    private RoomBunker _roomBunker;
    private VisibilityHandler _visibilityHandler;
    [SerializeField]
    private PostProcessHandler _postProcessHandler;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        CheckCameraStatePair();
#endif
        ChangeCamView(CamState.OVERAL_VIEW);
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _visibilityHandler = VisibilityHandler.Instance;

        _playerInRoomHandler.ActionInRoom += ChangeRoomCam;
        if (_postProcessHandler != null) _actionChangeCam += _postProcessHandler.ChangeVolume;
    }

    private void ChangeCamView(CamState newState)
    {
        foreach (CameraStatePair pair in _vCams)
        {
            pair.camera.Priority = pair.state == newState ? HIGH_PRIORITY : LOW_PRIORITY;
            if (pair.state == CamState.PLAYER_IN_ROOM && newState == CamState.PLAYER_IN_ROOM)
                pair.camera.Follow = _roomBunker.RoomDetails[_playerInRoomHandler.CurentRoomNum].transform;
        }
        _actionChangeCam?.Invoke(newState);
    }

    private void ChangeRoomCam(bool inRoom)
    {
        _playerInRoom = inRoom;
        ChangeCamView(inRoom ? CamState.PLAYER_IN_ROOM : _currentDefaultCam);
        _visibilityHandler.ChangeTargetRoom(!inRoom, _playerInRoomHandler.CurentRoomNum);
    }

    public void DefaultCamChanger()
    {
        if (_playerInRoom) return;

        _currentDefaultCam = (_currentDefaultCam == CamState.OVERAL_VIEW) ? CamState.PLAYER_FOLLOW : CamState.OVERAL_VIEW;
        ChangeCamView(_currentDefaultCam);
    }

    // 設定ミスをチェック
    private void CheckCameraStatePair()
    {
        HashSet<CamState> uniqueStates = new HashSet<CamState>();
        foreach (CameraStatePair pair in _vCams)
            if (!uniqueStates.Add(pair.state))
                Debug.LogError("Duplicate CamState detected: " + pair.state.ToString());
    }
}
