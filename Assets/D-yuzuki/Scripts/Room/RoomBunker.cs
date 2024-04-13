using System;
using UnityEngine;

// h‘S‘Ì‚Ì•”‰®î•ñ‚ð•ÛŽ
public class RoomBunker : MonoBehaviour
{
    [Header("ŠeŠK‘w‚Ì•”‰®”")]
    [SerializeField]
    private int _floorRoomCount;
    public int FloorRoomCount => _floorRoomCount;
    [Header("•”‰®‚ðŠi”[")]
    [SerializeField]
    private RoomDetails[] _roomDetails;
    public RoomDetails[] RoomDetails => _roomDetails;
    [Header("ŠK’i‚ðŠi”[")]
    [SerializeField]
    private Stair[] _stairs;
    public Stair[] Stairs => _stairs;
    public int TopFloor => _stairs.Length - 1;

    [Header("npc‚Ì–Ú•WÀ•WƒGƒ‰[’l(ƒGƒ}‚Ì•”‰®)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;
    [Header("•”‰®‚ÌŠeÀ•W‚ÌŒW”")]
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
