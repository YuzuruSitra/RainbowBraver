using UnityEngine;

// 部屋の建築処理
public class RoomBuilder
{
    private RoomBunker _roomBunker;

    public RoomBuilder(RoomBunker roomBunker)
    {
        _roomBunker = roomBunker;
    }
    
    public Vector3 CalculateInstantiatePosition()
    {
        float basePosY = _roomBunker.RoomDetails[0].transform.position.y;
        float newFloorPosY = basePosY + (_roomBunker.TopFloor + 1) * _roomBunker.FactorY;
        Vector3 returnPos = Vector3.zero;
        returnPos.x = _roomBunker.RoomDetails[0].transform.position.x;
        returnPos.y = newFloorPosY;
        returnPos.z = _roomBunker.BasePosZ;
        return returnPos;
    }

    public void UpdateRoomBunker(GameObject newFloor)
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
    public bool IsBuildValid()
    {
        ///
        /// 条件を追加
        ///
        return true;
    }

}
