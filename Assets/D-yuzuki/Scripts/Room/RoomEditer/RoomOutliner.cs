using UnityEngine;

// 部屋のアウトライン切り替えクラス
public class RoomOutliner : MonoBehaviour
{
    private RoomBunker _roomBunker;
    [SerializeField]
    private RoomClicker _roomClicker;
    private RoomDetails _currentRoom;
    private const string LAYER_OUTLINE = "Outline";
    private const string LAYER_DEFAULT = "Default";

    void Start()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _roomClicker.ChancgeRetentionRoom += ChangeOutLine;
    }

    void ChangeOutLine(GameObject newRoom)
    {
        RoomDetails newRoomInfo = null;
        if (newRoom != null)
        {
            newRoomInfo = newRoom.GetComponent<RoomDetails>();
            foreach (GameObject obj in newRoomInfo.OutlineObj)
                obj.layer = LayerMask.NameToLayer(LAYER_OUTLINE);
        }
        if (_currentRoom != null)
        {
            foreach (GameObject obj in _currentRoom.OutlineObj)
                obj.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
        }
        _currentRoom = newRoomInfo;
    }
}
