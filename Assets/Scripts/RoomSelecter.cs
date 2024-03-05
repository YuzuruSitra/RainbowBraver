using UnityEngine;

// ルートの選定
public class RoomSelecter: MonoBehaviour
{
    [Header("各階層の部屋数")]
    [SerializeField]
    private int _floorRoomCount;
    [SerializeField]
    private RoomDetails[] _roomDetails;

    // 最初にいるルームを取得
    public RoomDetails OutRemainRoom(int NPCRoom)
    {
        return _roomDetails[NPCRoom];
    }

    // ターゲットの部屋を選定
    public RoomDetails SelectTargetRoom(int NPCRoom, int currentRoomNum)
    {
        // 端部屋を考慮した選択肢を作成
        int[] contenderRoom = NPCRoom % _floorRoomCount == 0 || NPCRoom % _floorRoomCount == (_floorRoomCount - 1) ? 
            new int[] { NPCRoom, NPCRoom + 1 } : new int[] { NPCRoom, NPCRoom - 1, NPCRoom + 1 };

        // 受け入れ可能な部屋の数をカウント
        int acceptableRoomCount = 0;
        foreach (int roomNum in contenderRoom)
            if (_roomDetails[roomNum].IsRoomAcceptance) 
                acceptableRoomCount++;

        // 受け入れ可能な部屋がない場合はnullを返す
        if (acceptableRoomCount == 0)
            return null;

        // 前の部屋と違う部屋を選定
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance);

        return _roomDetails[contenderRoom[targetRoomNum]];
    }
}
