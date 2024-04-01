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
    private int _topFloor => _stairs.Length - 1;
    public int TopFloor => _topFloor;

    [Header("npc‚Ì–Ú•WÀ•WƒGƒ‰[’l(ƒGƒ}‚Ì•”‰®)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;

}
