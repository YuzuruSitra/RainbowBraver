using System.Collections.Generic;
using UnityEngine;

public class MaidRoomSelecter
{
    // ƒVƒ“ƒOƒ‹ƒgƒ“
    private static MaidRoomSelecter instance;
    public static MaidRoomSelecter Instance => instance ?? (instance = new MaidRoomSelecter());
    private RoomBunker _roomBunker;

    private MaidRoomSelecter()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public List<int> CreateRoomList(int floor)
    {
        var roomList = new List<int>();
        var minNum = (floor - 1) * _roomBunker.FloorRoomCount;
        var maxNum = minNum + _roomBunker.FloorRoomCount;
        for (var i = minNum; i < maxNum; i++)
        {
            var room = _roomBunker.RoomDetails[i];
            if (room.RoomType == RoomType.Private)
                roomList.Add(i);
        }
        return roomList;
    }
}
