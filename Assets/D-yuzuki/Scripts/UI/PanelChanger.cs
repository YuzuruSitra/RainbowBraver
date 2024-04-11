using System.Collections.Generic;
using UnityEngine;
using static InnGameController;

public class PanelChanger : MonoBehaviour
{
    [System.Serializable]
    private struct StatePanelPair
    {
        public InnState state;
        public GameObject panel;
    }

    [Header("ステートとパネルのセット")]
    [SerializeField]
    private StatePanelPair[] _panels;

    void Start()
    {
#if UNITY_EDITOR
        CheckCameraStatePair();
#endif
    }

    public void ChangePanel(InnState state)
    {
        foreach (StatePanelPair pair in _panels)
        {
            if (pair.state == state)
                pair.panel.SetActive(true);
            else
                pair.panel.SetActive(false);
        }
    }

    // 設定ミスをチェック
    private void CheckCameraStatePair()
    {
        HashSet<InnState> uniqueStates = new HashSet<InnState>();
        foreach (StatePanelPair pair in _panels)
            if (!uniqueStates.Add(pair.state))
                Debug.LogError("Duplicate CamState detected: " + pair.state.ToString());
    }
}
