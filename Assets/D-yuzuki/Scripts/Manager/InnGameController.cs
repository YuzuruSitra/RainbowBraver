using System;
using UnityEngine;

public class InnGameController : MonoBehaviour
{
    public enum InnState
    {
        DEFAULT,
        EDIT,
        SETTING
    }
    private InnState _currentState = InnState.DEFAULT;
    private event Action<InnState> ChancgeInnState;
    [SerializeField]
    private RoomEditor _roomEditor;
    [SerializeField]
    private PanelChanger _panelChanger;

    void Start()
    {
        ChancgeInnState += _roomEditor.FinishEditing;
        if (_panelChanger != null) ChancgeInnState += _panelChanger.ChangePanel;
    }

    public void ChangeInnState(InnState newState)
    {
        if (_currentState == newState) return;
        ChancgeInnState?.Invoke(newState);
        _currentState = newState;
    }

    void Update()
    {
        switch (_currentState)
        {
            case InnState.EDIT:
                _roomEditor.InputRoomSelect();
                break;
        }
    }

}
