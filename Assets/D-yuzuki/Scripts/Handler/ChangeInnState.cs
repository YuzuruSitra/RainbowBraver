using UnityEngine;
using static InnGameController;

public class ChangeInnState
{
    private InnGameController _innGameController;
    public ChangeInnState()
    {
        _innGameController = GameObject.FindWithTag("InnGameController").GetComponent<InnGameController>();
    }

    public void ChangeState()
    {
        switch (_innGameController.CurrentState)
        {
            case InnState.DEFAULT:
                _innGameController.ChangeInnState(InnState.EDIT);
                break;
            case InnState.EDIT:
                _innGameController.ChangeInnState(InnState.DEFAULT);
                break;
        }    
    }
}
