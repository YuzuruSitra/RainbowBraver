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
    public InnState CurrentState => _currentState;
    // public event Action<InnState> ChancgeInnState;
    private RoomEditor _roomEditor;

    void Start()
    {
        _roomEditor = new RoomEditor();    
    }

    public void ChangeInnState(InnState newState)
    {
        if (_currentState == newState) return;
        // ChancgeInnState?.Invoke(newState);
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
