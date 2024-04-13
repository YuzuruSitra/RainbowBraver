using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [Header("部屋生成時の親オブジェクト")]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private GameObject _roomsFPrefab;
    private RoomBunker _roomBunker;

    void Start()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }
    
    public void BuildFloor()
    {
        if (_roomBunker == null)  return;

        if (!IsBuildValid()) return;

        Vector3 insPos = CalculateInstantiatePosition();
        GameObject newFloor = Instantiate(_roomsFPrefab, insPos, Quaternion.identity, _parent);
        UpdateRoomBunker(newFloor);
        newFloor.name = "Rooms" + _roomBunker.TopFloor + "F";
    }

    private Vector3 CalculateInstantiatePosition()
    {
        float basePosY = _roomBunker.RoomDetails[0].transform.position.y;
        float newFloorPosY = basePosY + (_roomBunker.TopFloor + 1) * _roomBunker.FactorY;
        Vector3 returnPos = Vector3.zero;
        returnPos.x = _roomBunker.RoomDetails[0].transform.position.x;
        returnPos.y = newFloorPosY;
        returnPos.z = _roomBunker.BasePosZ;
        return returnPos;
    }

    private void UpdateRoomBunker(GameObject newFloor)
    {
        int childCount = newFloor.transform.childCount;
        RoomDetails[] newRooms = new RoomDetails[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = newFloor.transform.GetChild(i);
            RoomDetails room = child.GetComponent<RoomDetails>();
            newRooms[i] = room;
            UpdateRoomInfo(room, i);
        }

        _roomBunker.UpdateRoomIndex(newRooms);
    }

    private void UpdateRoomInfo(RoomDetails room, int num)
    {
        int roomNum = _roomBunker.RoomDetails.Length + num;
        room.SetRoomNum(roomNum);
    }

    // 建設可能かどうかをチェック
    private bool IsBuildValid()
    {
        ///
        /// 条件を追加
        ///
        return true;
    }

}
