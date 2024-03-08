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
    [Header("ŠK’i‚©”Û‚©")]
    [SerializeField]
    private bool _isStair;
    public bool IsStair => _isStair;

}
