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

    [Header("部屋の中")]
    [SerializeField]
    private Transform _roomInPoints;
    public Transform RoomInPoints => _roomInPoints;

    [Header("部屋の外")]
    [SerializeField]
    private Transform _roomOutPoints;
    public Transform RoomOutPoints => _roomOutPoints;
}
