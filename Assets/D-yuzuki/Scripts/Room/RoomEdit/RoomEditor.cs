using System.Collections.Generic;
using UnityEngine;
using static InnGameController;

public class RoomEditor : MonoBehaviour
{
    private RoomBunker _roomBunker;
    private RoomOutliner _roomOutliner;
    // RoomClicker
    private RoomClicker _roomClicker;
    [Header("部屋選択に必要なパラメーター")]
    [SerializeField]
    private LayerMask _targetLayer;
    // RoomChanger
    private RoomChanger _roomChanger;
    public RoomChanger RoomChanger => _roomChanger;
    [Header("部屋交換に必要なパラメーター")]
    [SerializeField]
    private Vector3 _offSet;
    [SerializeField]
    private GameObject _selectionObj;
    // RoomBuilder
    private RoomBuilder _roomBuilder;
    [Header("部屋建築に必要なパラメーター")]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private GameObject _roomsFPrefab;
    // 部屋情報
    [System.Serializable]
    private struct RoomsPair
    {
        public RoomType type;
        public GameObject prefab;
    }
    [Header("部屋の情報")]
    [SerializeField]
    private RoomsPair[] _roomsPair; 


    void Awake()
    {
#if UNITY_EDITOR
        CheckRoomsPair();
#endif
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _roomOutliner = new RoomOutliner();
        _roomClicker = new RoomClicker(_targetLayer);
        _roomChanger = new RoomChanger(_targetLayer);
        _roomBuilder = new RoomBuilder(_roomBunker);
        _roomClicker.ChangeRetentionRoom += _roomOutliner.ChangeOutLine;
        _roomClicker.ChangeRetentionRoom += _roomChanger.TouchRoom;
    }

    // ルーム選択
    public void InputRoomSelect()
    {
        if (Input.GetMouseButtonDown(0)) _roomClicker.SelectRoomObj();
        _roomChanger.MoveToTarget();
    }

    // 階の増築
    public void BuildFloor()
    {
        if (_roomBunker == null)  return;
        if (!_roomBuilder.IsBuildValid()) return;

        Vector3 insPos = _roomBuilder.CalculateInstantiatePosition();
        GameObject newFloor = Instantiate(_roomsFPrefab, insPos, Quaternion.identity, _parent);
        _roomBuilder.UpdateRoomBunker(newFloor);
        newFloor.name = "Rooms" + _roomBunker.TopFloor + "F";
    }

    // 編集モードの終了
    public void FinishEditing(InnState newState)
    {
        if (newState == InnState.EDIT) return;
        _roomOutliner.FinOutLine();
        _roomChanger.FinRoomChange();
    }

    // 設定ミスをチェック
    private void CheckRoomsPair()
    {
        HashSet<RoomType> uniqueStates = new HashSet<RoomType>();
        foreach (RoomsPair pair in _roomsPair)
            if (!uniqueStates.Add(pair.type))
                Debug.LogError("Duplicate RoomType detected: " + pair.type.ToString());
    }

}
