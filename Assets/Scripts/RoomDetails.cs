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

}
