using UnityEngine;

public class RoomPosAllocation
{
    // シングルトン
    private static RoomPosAllocation instance;
    public static RoomPosAllocation Instance => instance ?? (instance = new RoomPosAllocation());

    private RoomBunker _roomBunker;
    public Vector3 ErrorVector => _roomBunker.ErrorVector;

    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
    }

    private RoomPosAllocation()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    // 指定座標の払い出し
    public Vector3 TargetPosSelection(int roomNum, PointKind pointKind, float npcPosY)
    {
        Vector3 outPos = Vector3.zero;
        // 不正なルーム番号
        if (roomNum >= _roomBunker.RoomDetails.Length || roomNum < 0) return ErrorVector;

        if (pointKind == PointKind.IN_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomInPoints.position;
        else if (pointKind == PointKind.EXIT_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomExitPoints.position;
        else if (pointKind == PointKind.OUT_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomOutPoints.position;
        outPos.y = npcPosY;

        return outPos;
    }
}
