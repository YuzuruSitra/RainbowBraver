using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [Header("デバッグモード")]
    [SerializeField]
    private bool _isDebug;
    [SerializeField]
    private GameObject _debugObj;
    [SerializeField]
    private Transform _canvasTransform;
    private DebugRoomSelecter _debugRoomSelecter;

    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;

    // 最大階数
    private int _topFloor => _roomDetails.Length / _floorRoomCount - 1;

    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
    }

    private void Start()
    {
        if (_isDebug) _debugRoomSelecter = Instantiate(_debugObj, _canvasTransform).GetComponent<DebugRoomSelecter>();
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


    /*-----ターゲットの部屋を選定-----*/
    public int SelectNextRoomNum(int NPCRoom, int currentRoomNum)
    {
        List<int> contenderRoom = CreateContenderRoomList(NPCRoom);
        contenderRoom = SearchStairs(contenderRoom);

        if (contenderRoom.Count == 0) return ERROR_ROOM_NUM;

        List<int> alternativeRooms = SelectAlternativeRooms(contenderRoom, currentRoomNum);
        int nextRoomNum = SelectNextRoom(alternativeRooms);
        // デバッグ
        if(_isDebug) _debugRoomSelecter.OutValueDebug(contenderRoom, alternativeRooms);

        return nextRoomNum;
    }

    // 侵入可能な部屋の選択肢を作成
    private List<int> CreateContenderRoomList(int NPCRoom)
    {
        List<int> contenderRoom;
        int calcPos = NPCRoom % _floorRoomCount;

        if (calcPos == 0)
            contenderRoom = new List<int>() { NPCRoom, NPCRoom + 1 };
        else
            contenderRoom = new List<int>() { NPCRoom, NPCRoom - 1, NPCRoom + 1 };

        List<int> outRooms = new List<int>(contenderRoom);
        foreach (int room in contenderRoom)
        {
            if (room == NPCRoom) continue;
            if (!_roomDetails[room].IsRoomAcceptance) outRooms.Remove(room);
        }

        return outRooms;
    }

    // 階段を考慮した探索
    private List<int> SearchStairs(List<int> rooms)
    {
        List<int> updatedRooms = new List<int>(rooms);

        foreach (int roomNum in rooms)
        {
            // 階段でない場合は次の部屋へ
            if (!_roomDetails[roomNum].IsStair) continue;
            bool isDel = true;
            int floor = roomNum / _floorRoomCount;

            // 上階の部屋を追加
            if (floor != _topFloor)
            {
                int upperStairRoom = roomNum + _floorRoomCount;
                if (_roomDetails[upperStairRoom - 1].IsRoomAcceptance)
                {
                    updatedRooms.Add(upperStairRoom);
                    updatedRooms.Add(upperStairRoom - 1);
                    isDel = false;
                }
            }
            if (floor != 0)
            {
                // 下階の部屋を追加
                int lowerStairRoom = roomNum - _floorRoomCount;
                if (_roomDetails[lowerStairRoom].IsRoomAcceptance)
                {
                    updatedRooms.Add(lowerStairRoom);
                    updatedRooms.Add(lowerStairRoom - 1);
                    isDel = false;
                }
            }
            
            // 上下階の部屋が共に不可ならば現在の部屋を削除
            if (isDel) updatedRooms.Remove(roomNum);
        }

        return updatedRooms;
    }

    // 最終的な選択肢を選定
    private List<int> SelectAlternativeRooms(List<int> rooms, int currentRoomNum)
    {
        List<int> alternativeRooms = new List<int>();
        int currentFloor = currentRoomNum / _floorRoomCount;

        foreach (int roomNum in rooms)
        {
            if (roomNum == currentRoomNum) continue;
            int calcFloor = roomNum / _floorRoomCount;
            if (currentFloor == calcFloor) alternativeRooms.Add(roomNum);
        }
        
        return alternativeRooms;
    }

    // ランダムで部屋を選定
    private int SelectNextRoom(List<int> rooms)
    {
        int randomIndex = Random.Range(0, rooms.Count);
        int nextRoomNum = rooms[randomIndex];

        return nextRoomNum;
    }
    /*------------------------*/
    
    // 階段の上下探索
    public List<int> SearchStairs(int floor, int npcRoom)
    {
        int npcFloor = npcRoom / _floorRoomCount + 1;
        List<int> stairList = new List<int> { };
        int roomNum = (floor * _floorRoomCount) + _floorRoomCount - 1;
        int upperStairRoom = roomNum + _floorRoomCount;
        int floorDifference = Mathf.Abs((upperStairRoom / _floorRoomCount) - npcFloor);
        if (floor != _topFloor && floorDifference <= 1)
        {
            if (_roomDetails[upperStairRoom - 1].IsRoomAcceptance)
                stairList.Add(upperStairRoom / _floorRoomCount);
        }
        if (floor != 0 && floorDifference <= 1)
        {
            int lowerStairRoom = roomNum - _floorRoomCount;
            if (_roomDetails[lowerStairRoom].IsRoomAcceptance)
                stairList.Add(lowerStairRoom / _floorRoomCount);
        }
        return stairList;
    }
}
