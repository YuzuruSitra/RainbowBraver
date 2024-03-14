using UnityEngine;

// 部屋の可視性変更
public class VisibilityHandler
{
    // シングルトン
    private static VisibilityHandler instance;
    public static VisibilityHandler Instance => instance ?? (instance = new VisibilityHandler());
    private RoomBunker _roomBunker;

    private VisibilityHandler()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }


    public void ChangeAllRoom()
    {
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].FrontWall.enabled = !rooms[i].FrontWall.enabled;
            rooms[i].FrontWall.enabled = !rooms[i].FrontDoor.enabled;
        }
    }

    public void ChangeTargetRoom(bool state, int roomNum)
    {
        _roomBunker.RoomDetails[roomNum].FrontWall.enabled = state;
        _roomBunker.RoomDetails[roomNum].FrontDoor.enabled = state;
    }
}

