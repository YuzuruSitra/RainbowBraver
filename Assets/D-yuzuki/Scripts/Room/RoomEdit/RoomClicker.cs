using System;
using UnityEngine;

// 部屋選択クラス
public class RoomClicker
{
    private RoomDetails _retentionRoom;
    public RoomDetails RetentionRoom => _retentionRoom;
    public event Action<RoomDetails> ChancgeRetentionRoom;
    int _targetLayer = 1 << LayerMask.NameToLayer("Room");

    public void SelectRoomObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _targetLayer)) return;
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj.tag != "Room") return;
        RoomDetails hitRoom = hitObj.GetComponent<RoomDetails>();
        InputNewRoom(hitRoom);
    }

    private void InputNewRoom(RoomDetails room)
    {
        ChancgeRetentionRoom?.Invoke(room);
        _retentionRoom = room;
    }

}
