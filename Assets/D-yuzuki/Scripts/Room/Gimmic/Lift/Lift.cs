using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public enum LiftInfo
    {
        UPPER,
        LOWER
    }
    [SerializeField]
    private LiftInfo _info;
    public LiftInfo Info => _info;

    [SerializeField]
    private Lift _pairLift;

    [Header("移動時の待機時間")]
    [SerializeField] 
    private float _waitTime;
    private RoomDetails _roomDetails;
    public RoomDetails RoomDetails => _roomDetails;
    private GameObject _targetObj;
    
    private Vector3 _entryPos;
    public Vector3 EntryPos => _entryPos;

    private Vector3 _npcOutPos;
    public Vector3 NPCOutPos => _npcOutPos;

    // Start is called before the first frame update
    void Start()
    {
        _roomDetails = GetComponent<RoomDetails>();
        _entryPos = _roomDetails.RoomInPoints.position;
        _npcOutPos = _roomDetails.RoomOutPoints.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomNPC"))
        {
            // 手前からの侵入のみ許可
            Vector3 direction = (transform.position - other.transform.position).normalized;
            if (direction.z <= 0) return;
            _targetObj = other.gameObject;
            StartCoroutine(AutoMoving_NPC());
        }
    }

    private IEnumerator AutoMoving_NPC()
    {
        BraverController braver = _targetObj.GetComponent<BraverController>();
        braver.IsFreedom = false;

        // エントリー
        while (Vector3.Distance(_targetObj.transform.position, _entryPos) >= braver.StoppingDistance)
        {
            Vector3 direction1 = (_entryPos - _targetObj.transform.position).normalized;
            direction1.y = 0f;
            _targetObj.transform.position += direction1 * braver.MoveSpeed * Time.deltaTime;

            Quaternion targetRotation1 = Quaternion.LookRotation(-direction1);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation1, braver.RotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 階層のワープ
        _targetObj.transform.position = _pairLift.EntryPos;
        yield return _waitTime;

        // 退出    
        while (Vector3.Distance(_targetObj.transform.position, _pairLift.NPCOutPos) >= braver.StoppingDistance)
        {
            Vector3 direction2 = (_pairLift.NPCOutPos - _targetObj.transform.position).normalized;
            direction2.y = 0f;
            _targetObj.transform.position += direction2 * braver.MoveSpeed * Time.deltaTime;
            Quaternion targetRotation2 = Quaternion.LookRotation(-direction2);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation2, braver.RotationSpeed * Time.deltaTime);
            yield return null;
        }
        braver.IsFreedom = true;
        braver.FinWarpHandler(RoomAIState.EXIT_ROOM, _pairLift.RoomDetails.RoomNum);
    }

}
