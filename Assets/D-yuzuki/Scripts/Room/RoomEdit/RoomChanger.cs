using UnityEngine;

// 部屋の入れ替え処理
public class RoomChanger
{
    private Vector3 _offSet;
    private GameObject _selectionObj;
    private RoomDetails _touchObj;
    private RoomDetails _cloneTouchObj;
    private bool _isEditing = false;

    public RoomChanger(Vector3 offSet, GameObject selectionObj)
    {
        _offSet = offSet;
        _selectionObj = selectionObj;
    }

    public void ChangerSwitch()
    {
        if (_isEditing)
            FinishChanging();
        else
            LaunchChanging();
    }

    public void ChangeRoom(RoomDetails target)
    {
        _touchObj = target;
        if (!_isEditing) return;
        if (!IsEditingEnabled(target)) return;
        
        // 場所を交換
        Vector3 tmpPos = _cloneTouchObj.transform.position;
        _cloneTouchObj.transform.position = target.transform.position;
        target.transform.position = tmpPos;
        // 部屋の番号を交換
        int tmpNum = _cloneTouchObj.RoomNum;
        _cloneTouchObj.SetRoomNum(target.RoomNum);
        target.SetRoomNum(tmpNum);

        // 予測エリアの更新
        _selectionObj.transform.position = target.transform.position - _offSet;
        FinishChanging();
    }

    // 部屋交換の開始
    private void LaunchChanging()
    {
        if (_touchObj == null) return;
        _cloneTouchObj = _touchObj;
        _selectionObj.SetActive(true);
        _selectionObj.transform.position = _touchObj.transform.position - _offSet;            
        _isEditing = true;    
    }

    // 部屋交換の終了
    private void FinishChanging()
    {
        _selectionObj.SetActive(false);
        _isEditing = false;
    }

    // 特定の部屋の時は選択不可
    private bool IsEditingEnabled(RoomDetails target)
    {
        if (target.RoomType == RoomType.Lift) return false;
        return true;
    }

    // 終了処理
    public void FinRoomChange()
    {
        _touchObj = null;
    }
}
