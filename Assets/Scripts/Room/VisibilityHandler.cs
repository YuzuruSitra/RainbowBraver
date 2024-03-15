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
            for (int u = 0; u < rooms[i].FrontMesh.Length; u++)
                rooms[i].FrontMesh[i].enabled = !rooms[i].FrontMesh[i].enabled;
    }

    public void ChangeTargetRoom(bool state, int roomNum)
    {
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        for (int i = 0; i < rooms[roomNum].FrontMesh.Length; i++)
                rooms[roomNum].FrontMesh[i].enabled = state;
    }
}

