using System;
using UnityEngine;

// •”‰®‘I‘ðƒNƒ‰ƒX
public class RoomClicker : MonoBehaviour
{
    public event Action<GameObject> ChancgeRetentionRoom;
    [SerializeField] 
    private LayerMask roomLayer;
    private GameObject _retentionRoom;
    public GameObject RetentionRoom => _retentionRoom;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) SelectRoomObj();
    }

    void SelectRoomObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, roomLayer)) return;
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj.tag != "Room") return;
        ChangeRetentionRoom(hitObj);
    }

    private void ChangeRetentionRoom(GameObject obj)
    {
        _retentionRoom = obj;
        ChancgeRetentionRoom?.Invoke(_retentionRoom);
    }

}
