using System;
using UnityEngine;

// 宿全体の部屋情報を保持
public class RoomBunker : MonoBehaviour
{
    [Header("各階層の部屋数")]
    [SerializeField]
    private int _floorRoomCount;
    public int FloorRoomCount => _floorRoomCount;
    [Header("部屋を格納")]
    [SerializeField]
    private RoomDetails[] _roomDetails;
    public RoomDetails[] RoomDetails => _roomDetails;
    [Header("階段を格納")]
    [SerializeField]
    private Stair[] _stairs;
    public Stair[] Stairs => _stairs;
    public int TopFloor => _stairs.Length - 1;

    [Header("npcの目標座標エラー値(エマの部屋)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;
    [Header("部屋の各座標の係数")]
    [SerializeField]
    private float _factorX;
    public float FactorX => _factorX;
    [SerializeField]
    private float _factorY;
    public float FactorY => _factorY;
    [SerializeField]
    private float _basePosZ;
    public float BasePosZ => _basePosZ;

    public void UpdateRoomIndex(RoomDetails[] newRooms)
    {
        int currentCount = _roomDetails.Length;
        Array.Resize(ref _roomDetails, currentCount + newRooms.Length);
        for (int i = 0; i < newRooms.Length; i++)
            _roomDetails[currentCount + i] = newRooms[i];
        UpdateStairIndex(_roomDetails[_roomDetails.Length - 1]);
    }

    private void UpdateStairIndex(RoomDetails room)
    {
        Array.Resize(ref _stairs, _stairs.Length + 1);
        _stairs[_stairs.Length - 1] = room.GetComponent<Stair>();
    }

}
