using UnityEngine;

// •”‰®î•ñ‚Ì•ÛŽ
public class RoomDetails : MonoBehaviour
{
    [Header("•”‰®”Ô†")]
    [SerializeField]
    private int _roomNum;
    public int RoomNum => _roomNum;

    [Header("“üŽº‹–‰Â")]
    [SerializeField]
    private bool _isRoomAcceptance;
    public bool IsRoomAcceptance => _isRoomAcceptance;

    [Header("•”‰®‚Ì’†")]
    [SerializeField]
    private Transform _roomInPoints;
    public Transform RoomInPoints => _roomInPoints;

    [Header("•”‰®‚ÌŒû")]
    [SerializeField]
    private Transform _roomExitPoints;
    public Transform RoomExitPoints => _roomExitPoints;

    [Header("•”‰®‚ÌŠO")]
    [SerializeField]
    private Transform _roomOutPoints;
    public Transform RoomOutPoints => _roomOutPoints;
}
