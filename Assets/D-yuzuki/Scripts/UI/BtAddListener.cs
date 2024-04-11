using UnityEngine;
using UnityEngine.UI;

public class BtAddListener : MonoBehaviour
{
    [SerializeField]
    private Button _changeViewButton;
    [SerializeField]
    private Button _changeVisibilButton;
    [SerializeField]
    private Button _changeEditModeButton;
    [SerializeField]
    private CamController _camController;
    // Start is called before the first frame update
    void Start()
    {
        _changeViewButton.onClick.AddListener(_camController.DefaultCamChanger);
        _changeVisibilButton.onClick.AddListener(VisibilityHandler.Instance.ChangeAllRoom);
        _changeEditModeButton.onClick.AddListener(new ChangeInnState().ChangeState);
    }

}
