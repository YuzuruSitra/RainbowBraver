using UnityEngine;
using static InnGameController;

public class ChangeInnStateHandler
{
    private InnGameController _innGameController;
    public ChangeInnStateHandler()
    {
        _innGameController = GameObject.FindWithTag("InnGameController").GetComponent<InnGameController>();
    }

    public void ChangeDefaultState()
    {
        _innGameController.ChangeInnState(InnState.DEFAULT);
    }

    public void ChangeEditState()
    {
        _innGameController.ChangeInnState(InnState.EDIT);
    }
}
