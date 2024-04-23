using UnityEngine;

public enum RoomType
{
    Enpty,
    Private,
    Facility,
    Stair,
    Lift
}

// 部屋情報の保持
public class RoomDetails : MonoBehaviour
{
    [Header("部屋番号")]
    [SerializeField]
    private int _roomNum;
    public int RoomNum => _roomNum;

    [Header("ルームタイプを選択")]
    [SerializeField]
    private RoomType _roomType;
    public RoomType RoomType => _roomType;

    // 後々Scriptableオブジェクトから引く予定
    [Header("部屋に居座る時間")]
    [SerializeField]
    private float _remainTime;
    public float RemainTime => _remainTime;

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

    [Header("透過したいメッシュ")]
    [SerializeField]
    private MeshRenderer[] _frontMesh;
    public MeshRenderer[] FrontMesh => _frontMesh;    
    [Header("アウトラインをつけたいメッシュ")]
    [SerializeField]
    private GameObject[] _outlineObj;
    public GameObject[] OutlineObj => _outlineObj;  

    // 侵入可能かの検閲
    public bool IsRoomAcceptance(int npcRoom)
    {
        bool outBool = false;
        switch (_roomType)
        {
            case RoomType.Private:
                outBool = npcRoom == _roomNum;
                break;
            case RoomType.Facility:
            case RoomType.Stair:
            case RoomType.Lift:
                outBool = true;
                break;
        }
        return outBool;
    }

    public void SetRoomNum(int num)
    {
        _roomNum = num;
    }

}
