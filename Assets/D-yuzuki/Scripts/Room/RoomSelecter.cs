using System.Collections.Generic;
using UnityEngine;

// 目的部屋の選定
public class RoomSelecter
{
    // シングルトン
    private static RoomSelecter instance;
    public static RoomSelecter Instance => instance ?? (instance = new RoomSelecter());

    private RoomBunker _roomBunker;
    public Vector3 ErrorVector => _roomBunker.ErrorVector;

    private RoomSelecter()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
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

    /*-----ターゲットの部屋を選定-----*/
    public int SelectNextRoomNum(int npcRoom, int currentRoomNum)
    {
        List<int> contenderRoom = CreateContenderRoomList(npcRoom);
        contenderRoom = SearchStairs(contenderRoom, npcRoom);
        contenderRoom = SearchLift(contenderRoom, npcRoom);
        if (contenderRoom.Count == 0) return RoomBunker.ERROR_ROOM_NUM;
        List<int> alternativeRooms = SelectAlternativeRooms(contenderRoom, currentRoomNum);
        int nextRoomNum = SelectNextRoom(alternativeRooms);
        return nextRoomNum;
    }

    // 侵入可能な部屋の選択肢を作成
    private List<int> CreateContenderRoomList(int npcRoom)
    {
        List<int> contenderRoom;
        int calcPos = npcRoom % _roomBunker.FloorRoomCount;

        if (calcPos == 0)
            contenderRoom = new List<int>() { npcRoom, npcRoom + 1 };
        else
            contenderRoom = new List<int>() { npcRoom, npcRoom - 1, npcRoom + 1 };

        List<int> outRooms = new List<int>(contenderRoom);
        foreach (int room in contenderRoom)
            if (!_roomBunker.RoomDetails[room].IsRoomAcceptance(npcRoom)) 
                outRooms.Remove(room);
        return outRooms;
    }

    // 階段を考慮した探索
    private List<int> SearchStairs(List<int> rooms, int npcRoom)
    {
        List<int> updatedRooms = new List<int>(rooms);
        foreach (int roomNum in rooms)
        {
            RoomType roomType = _roomBunker.RoomDetails[roomNum].RoomType;
            if (roomType != RoomType.Stair) continue;
            bool isDel = true;
            int floor = roomNum / _roomBunker.FloorRoomCount;

            // 上階の部屋を追加
            if (floor != _roomBunker.TopFloor)
            {
                int upperStairRoom = roomNum + _roomBunker.FloorRoomCount;
                if (_roomBunker.RoomDetails[upperStairRoom - 1].IsRoomAcceptance(npcRoom))
                {
                    updatedRooms.Add(upperStairRoom);
                    updatedRooms.Add(upperStairRoom - 1);
                    isDel = false;
                }
            }
            if (floor != 0)
            {
                // 下階の部屋を追加
                int lowerStairRoom = roomNum - _roomBunker.FloorRoomCount;
                if (_roomBunker.RoomDetails[lowerStairRoom].IsRoomAcceptance(npcRoom))
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

    // リフトを考慮した探索
    private List<int> SearchLift(List<int> rooms, int npcRoom)
    {
        List<int> updatedRooms = new List<int>(rooms);
        foreach (int roomNum in rooms)
        {
            RoomType roomType = _roomBunker.RoomDetails[roomNum].RoomType;
            bool isAdd = false;
            if (roomType != RoomType.Lift) continue;
            GameObject room = _roomBunker.RoomDetails[roomNum].gameObject;
            Lift lift = room.GetComponent<Lift>();
            // 階層を検索
            int targetLiftNum;
            if (lift.Info == Lift.LiftInfo.UPPER)
                targetLiftNum = roomNum - _roomBunker.FloorRoomCount;
            else
                targetLiftNum = roomNum + _roomBunker.FloorRoomCount;
            // 端かどうかの判定
            int outerNum = targetLiftNum % _roomBunker.FloorRoomCount; 
            // リフトの左右を候補に追加
            if (outerNum != 0 && _roomBunker.RoomDetails[targetLiftNum - 1].IsRoomAcceptance(npcRoom))
            {
                isAdd = true;
                updatedRooms.Add(targetLiftNum - 1);
            }
            if (outerNum != _roomBunker.FloorRoomCount - 1 && _roomBunker.RoomDetails[targetLiftNum + 1].IsRoomAcceptance(npcRoom))
            {
                isAdd = true;
                updatedRooms.Add(targetLiftNum + 1);
            }
            if (isAdd) updatedRooms.Add(targetLiftNum);
        }

        return updatedRooms;
    }

    // 最終的な選択肢を選定
    private List<int> SelectAlternativeRooms(List<int> rooms, int currentRoomNum)
    {
        List<int> alternativeRooms = new List<int>();
        int currentFloor = currentRoomNum / _roomBunker.FloorRoomCount;

        foreach (int roomNum in rooms)
        {
            if (roomNum == currentRoomNum) continue;
            int calcFloor = roomNum / _roomBunker.FloorRoomCount;
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
}
