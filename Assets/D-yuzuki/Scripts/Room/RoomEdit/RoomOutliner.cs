using UnityEngine;
using static InnGameController;

// �����̃A�E�g���C���؂�ւ��N���X
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
                if (newRoom != _currentRoom)
                    obj.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
        }
        _currentRoom = newRoom;
    }

    public void FinOutLine(InnState newState)
    {
        if (newState == InnState.EDIT) return;
        if (_currentRoom != null)
        {
            foreach (GameObject obj in _currentRoom.OutlineObj)
                obj.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
        }
        _currentRoom = null;
    }

}