using UnityEngine;

// 部屋の入れ替え処理
public class RoomChanger
{
//     float _zPosition;
//     Vector3 tmpPos;
//     private RoomDetails _firstTouch;
//     private GameObject _currentHitRoom;
//     private bool _isEditing = false;
//     private int _targetLayer;

//     public RoomChanger(int layer)
//     {
//         _targetLayer = layer;
//     }

//     public void ChangerSwitch()
//     {
//         if (_isEditing)
//             FinishChanging();
//         else
//             LaunchChanging();
//     }

//     public void MoveToTarget()
//     {
//         if (!_isEditing) return;
//         if (_firstTouch == null) return;
//         if (Input.GetMouseButtonDown(0))
//         {
//             _zPosition = _firstTouch.transform.position.z - Camera.main.transform.position.z;
//             tmpPos = _firstTouch.transform.position;
//         }
//         if (Input.GetMouseButton(0))
//         {
//             Vector3 inputVector = Input.mousePosition;
//             inputVector.z = _zPosition;
//             Vector3 targetPosition = Camera.main.ScreenToWorldPoint(inputVector);
//             // オブジェクトを移動させる
//             _firstTouch.transform.position = targetPosition;

//             // 他の部屋の位置をずらす
// /*            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             RaycastHit hit;
//             if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _targetLayer)) return;
//             GameObject hitObj = hit.collider.gameObject;
//             if (hitObj.tag == "Room" && _currentHitRoom != hitObj)
//             {
//                 Debug.Log(hitObj.transform.position);
//                 _currentHitRoom = hitObj;
//                 RoomDetails hitRoom = hitObj.GetComponent<RoomDetails>();
//                 Vector3 tmp = hitRoom.transform.position;
//                 hitRoom.transform.position = tmpPos;
//                 //tmpPos = tmp;
//                 //Debug.Log("最初の部屋 : " + tmpPos);
//             }*/
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             _firstTouch.transform.position = tmpPos;
//             _firstTouch = null;
//         }
//     }

//     public void ChangeRoom(RoomDetails target)
//     {
//         _firstTouch = target;        
//     }

//     // 部屋交換の開始
//     private void LaunchChanging()
//     {            
//         _isEditing = true;
//     }

//     // 部屋交換の終了
//     private void FinishChanging()
//     {
//         _isEditing = false;
//     }

//     // 終了処理
//     public void FinRoomChange()
//     {
//         _isEditing = false;
//     }

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
