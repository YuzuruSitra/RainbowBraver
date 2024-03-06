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
        int[] contenderRoom;
        int calcPos = NPCRoom % _floorRoomCount;
        
        // 左端
        if (calcPos == 0)
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom + 1 };
        }
        // 右端
        else if (calcPos == _floorRoomCount - 1)
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1 };
        }
        else
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1, NPCRoom + 1 };
        }

        // 受け入れ可能な部屋の数をカウント
        int acceptableRoomCount = 0;
        foreach (int roomNum in contenderRoom)
            if (_roomDetails[roomNum].IsRoomAcceptance && roomNum != NPCRoom) 
                acceptableRoomCount++;

        // 受け入れ可能な部屋がない場合はnullを返す
        if (acceptableRoomCount == 0) return null;

        // 前の部屋と違う部屋を選定
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || (contenderRoom[targetRoomNum] != NPCRoom && !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance));

        return _roomDetails[contenderRoom[targetRoomNum]];
    }
}
