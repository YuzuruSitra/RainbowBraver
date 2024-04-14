using UnityEngine;

public class RoomEditor
{
    private static RoomEditor _instance;
    public static RoomEditor Instance => _instance ?? (_instance = new RoomEditor());
    
    private RoomClicker _roomClicker;
    public RoomClicker RoomClicker => _roomClicker;
    private RoomOutliner _roomOutliner;
    public RoomOutliner RoomOutliner => _roomOutliner;
    public RoomDetails SelectObj => _roomClicker.RetentionRoom;

    public RoomEditor()
    {
        _roomClicker = new RoomClicker();
        _roomOutliner = new RoomOutliner();
        _roomClicker.ChangeRetentionRoom += _roomOutliner.ChangeOutLine;
    }

    public void InputRoomSelect()
    {
        if (Input.GetMouseButtonDown(0)) _roomClicker.SelectRoomObj();
    }

}
