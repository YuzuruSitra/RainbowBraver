using UnityEngine;

// 部屋のアウトライン切り替えクラス
public class RoomOutliner
{
    private RoomDetails _currentRoom;
    private const string LAYER_OUTLINE = "Outline";
    private const string LAYER_DEFAULT = "Default";

    public void ChangeOutLine(RoomDetails newRoom)
    {
        if (newRoom != null)
        {
            foreach (GameObject obj in newRoom.OutlineObj)
                obj.layer = LayerMask.NameToLayer(LAYER_OUTLINE);
        }
        if (_currentRoom != null)
        {
            foreach (GameObject obj in _currentRoom.OutlineObj)
                obj.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
        }
        _currentRoom = newRoom;
    }
}
