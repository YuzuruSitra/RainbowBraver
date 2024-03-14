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
    private int _topFloor => _stairs.Length - 1;
    public int TopFloor => _topFloor;

    [Header("npcの目標座標エラー値(エマの部屋)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;

}
