using UnityEngine;

// ルートの選定
public class RoomSelecter : MonoBehaviour
{
    [Header("各階層の部屋数")]
    [SerializeField]
    private int _floorRoomCount;
    public int FloorRoomCount => _floorRoomCount;

    [Header("部屋を格納")]
    [SerializeField]
    private RoomDetails[] _roomDetails;

    [Header("npcの目標座標エラー値(エマの部屋)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;

    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
    }

    // ターゲットの部屋を選定
    public int SelectNextRoomNum(int NPCRoom, int currentRoomNum)
    {
        // 端部屋を考慮した選択肢を作成
        int[] contenderRoom;
        int calcPos = NPCRoom % _floorRoomCount;

        // 左端
        if (calcPos == 0)
            contenderRoom = new int[] { NPCRoom, NPCRoom + 1 };
        // 右端
        else if (calcPos == _floorRoomCount - 1)
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1 };
        else
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1, NPCRoom + 1 };
        　

        // 受け入れ可能な部屋の数をカウント
        int acceptableRoomCount = 0;
        foreach (int roomNum in contenderRoom)
            if (_roomDetails[roomNum].IsRoomAcceptance && roomNum != NPCRoom)
                acceptableRoomCount++;

        // 受け入れ可能な部屋がない場合はnullを返す
        if (acceptableRoomCount == 0) return ERROR_ROOM_NUM;

        // 前の部屋と違う部屋を選定
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || (contenderRoom[targetRoomNum] != NPCRoom && !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance));

        return contenderRoom[targetRoomNum];
    }

    // 指定座標の払い出し
    public Vector3 TargetPosSelection(int roomNum, PointKind pointKind, float npcPosY)
    {
        Vector3 outPos = Vector3.zero;
        // 不正なルーム番号
        if (roomNum >= _roomDetails.Length || roomNum < 0) return ErrorVector;

        if (pointKind == PointKind.IN_POINT) outPos = _roomDetails[roomNum].RoomInPoints.position;
        else if (pointKind == PointKind.EXIT_POINT) outPos = _roomDetails[roomNum].RoomExitPoints.position;
        else if (pointKind == PointKind.OUT_POINT) outPos = _roomDetails[roomNum].RoomOutPoints.position;
        outPos.y = npcPosY;

        return outPos;
    }

}
