using UnityEngine;

// 部屋情報の保持
public class RoomDetails : MonoBehaviour
{
    [Header("部屋番号")]
    [SerializeField]
    private int _roomNum;
    public int RoomNum => _roomNum;

    [Header("入室許可")]
    [SerializeField]
    private bool _isRoomAcceptance;
    public bool IsRoomAcceptance => _isRoomAcceptance;

    [Header("階段か否か")]
    [SerializeField]
    private bool _isStair;
    public bool IsStair => _isStair;

    [Header("ワープか否か")]
    [SerializeField]
    private bool _isWarp;
    public bool IsWarp => _isWarp;


    [Header("部屋の中")]
    [SerializeField]
    private Transform _roomInPoints;
    public Transform RoomInPoints => _roomInPoints;
    [Header("部屋の口")]
    [SerializeField]
    private Transform _roomExitPoints;
    public Transform RoomExitPoints => _roomExitPoints;
    [Header("部屋の外")]
    [SerializeField]
    private Transform _roomOutPoints;
    public Transform RoomOutPoints => _roomOutPoints;

    [SerializeField]
    private MeshRenderer _frontWall;
    public MeshRenderer FrontWall => _frontWall;
    [SerializeField]
    private MeshRenderer _frontDoor;
    public MeshRenderer FrontDoor => _frontDoor;
    

}
