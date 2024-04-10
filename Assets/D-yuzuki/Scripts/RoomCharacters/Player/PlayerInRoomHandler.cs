using System;
using UnityEngine;

public class PlayerInRoomHandler : MonoBehaviour
{
    private bool _inRoom = false;
    public event Action<bool> ActionInRoom;
    private int _currentRoomNum;
    public int CurentRoomNum => _currentRoomNum;
    private Vector3 _defaultSize;
    private Vector3 _inRoomSize;

    [Header("ïîâÆÇ≈ÇÃÉTÉCÉYåWêî")]
    [SerializeField]
    private float _scaleFactor;
    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        _defaultSize = _player.transform.lossyScale;
        _inRoomSize = _defaultSize * _scaleFactor;
    }

    void Update()
    {
        transform.position = _player.transform.position;
    }

    public void ChangeInRoom(bool state)
    {
        if (_inRoom == state) return;
        _inRoom = state;
        ActionInRoom?.Invoke(_inRoom);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            _currentRoomNum = other.gameObject.GetComponent<RoomDetails>().RoomNum;
            ChangeInRoom(true);
            _player.transform.localScale = _inRoomSize;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            ChangeInRoom(false);
            _player.transform.localScale = _defaultSize;
        }
    }
}
