using UnityEngine;

public class RoomChanger
{
    private float _zPosition;
    private Vector3 _tmpPos;
    private RoomDetails _firstTouch;
    private GameObject _firstTouchObj;
    private bool _isEditing = false;
    private int _targetLayer;

    public RoomChanger(int layer)
    {
        _targetLayer = layer;
    }

    // 部屋の交換開始と終了を切り替える
    public void ChangerSwitch()
    {
        if (_isEditing)
            FinishChanging();
        else
            LaunchChanging();
    }

    // 部屋をターゲットに移動
    public void MoveToTarget()
    {
        if (!_isEditing || _firstTouchObj == null) return;

        if (Input.GetMouseButton(0))
        {
            MoveRoomWithMouse();
            SwapRoomPositions();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinishRoomMove();
        }
    }

    // 部屋に触れたときの処理
    public void TouchRoom(RoomDetails target)
    {
        _firstTouch = target;
        _firstTouchObj = target.gameObject;
        _zPosition = _firstTouchObj.transform.position.z - Camera.main.transform.position.z;
        _tmpPos = _firstTouchObj.transform.position;
    }

    // 部屋交換の開始
    private void LaunchChanging()
    {
        _isEditing = true;
    }

    // 部屋交換の終了
    private void FinishChanging()
    {
        _isEditing = false;
    }

    // マウスによる部屋の移動
    private void MoveRoomWithMouse()
    {
        var inputVector = Input.mousePosition;
        inputVector.z = _zPosition;
        var targetPosition = Camera.main.ScreenToWorldPoint(inputVector);
        _firstTouchObj.transform.position = targetPosition;
    }

    // 部屋の位置を他の部屋と交換する処理
    private void SwapRoomPositions()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _targetLayer)) return;

        var hitObj = hit.collider.gameObject;
        if (hitObj.CompareTag("Room") && _firstTouchObj != hitObj)
        {
            // 位置交換
            var hitPos = hitObj.transform.position;
            hitObj.transform.position = _tmpPos;
            _tmpPos = hitPos;

            // 部屋番号交換
            var hitRoom = hitObj.GetComponent<RoomDetails>();
            var hitRoomNum = hitRoom.RoomNum;
            hitRoom.SetRoomNum(_firstTouch.RoomNum);
            _firstTouch.SetRoomNum(hitRoomNum);
        }
    }

    // 部屋移動の終了
    private void FinishRoomMove()
    {
        _firstTouchObj.transform.position = _tmpPos;
        _firstTouchObj = null;
    }

    // 終了処理
    public void FinRoomChange()
    {
        _isEditing = false;
    }
}
