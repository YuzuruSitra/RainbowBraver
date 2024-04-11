using System;
using UnityEngine;

// •”‰®‘I‘ğƒNƒ‰ƒX
public class RoomClicker
{
    public event Action<GameObject> ChancgeRetentionRoom;
    int _targetLayer = 1 << LayerMask.NameToLayer("Room");

    public void SelectRoomObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _targetLayer)) return;
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj.tag != "Room") return;
        ChancgeRetentionRoom?.Invoke(hitObj);
    }

}
