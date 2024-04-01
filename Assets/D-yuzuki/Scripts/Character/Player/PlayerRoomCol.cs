using System;
using UnityEngine;

public class PlayerRoomCol : MonoBehaviour
{
    private bool _inRoom = false;
    public event Action<bool> ActionInRoom;
    private int _currentRoomNum;
    public int CurentRoomNum => _currentRoomNum;

    // Start is called before the first frame update
    void Start()
    {
        
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
            transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            ChangeInRoom(false);
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
