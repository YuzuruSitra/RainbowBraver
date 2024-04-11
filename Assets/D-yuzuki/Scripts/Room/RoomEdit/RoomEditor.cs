using UnityEngine;

public class RoomEditor
{
    private RoomDetails _retentionRoom;
    private RoomClicker _roomClicker;
    private RoomOutliner _roomOutliner;

    public RoomEditor()
    {
        _roomClicker = new RoomClicker();
        _roomOutliner = new RoomOutliner();
        _roomClicker.ChancgeRetentionRoom += _roomOutliner.ChangeOutLine;
    }

    public void InputRoomSelect()
    {
        if (Input.GetMouseButtonDown(0)) _roomClicker.SelectRoomObj();
    }

}
