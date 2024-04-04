using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField]
    private Lift _pairLift;

    [Header("階 (1F=0)")]
    [SerializeField]
    private int _roomFloor;
    [Header("移動時の待機時間")]
    [SerializeField] 
    private float _waitTime;
    [SerializeField]
    private RoomDetails _roomDetails;
    private GameObject _targetObj;
    private StairSelecter _stairSelecter;
    
    private Vector3 _entryPos;
    public Vector3 EntryPos => _entryPos;

    private Vector3 _npcOutPos;
    public Vector3 NPCOutPos => _npcOutPos;
    
    private Vector3 _playerOutPos;
    public Vector3 PlayerOutPos => _playerOutPos;

    // Start is called before the first frame update
    void Start()
    {
        _entryPos = _roomDetails.RoomInPoints.position;
        _npcOutPos = _roomDetails.RoomOutPoints.position;
        _playerOutPos = _roomDetails.RoomExitPoints.position;
        _stairSelecter = StairSelecter.Instance;
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
        NPCController npc = _targetObj.GetComponent<NPCController>();
        npc.IsFreedom = false;

        // エントリー
        while (Vector3.Distance(_targetObj.transform.position, _entryPos) >= npc.StoppingDistance)
        {
            Vector3 direction1 = (_entryPos - _targetObj.transform.position).normalized;
            direction1.y = 0f;
            _targetObj.transform.position += direction1 * npc.MoveSpeed * Time.deltaTime;

            Quaternion targetRotation1 = Quaternion.LookRotation(-direction1);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation1, npc.RotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 階層のワープ
        Stair targetFloor = _stairSelecter.FloorSelecter(_roomFloor, npc.BaseRoom);
        _targetObj.transform.position = targetFloor.EntryPos;

        yield return _waitTime;

        // 退出    
        while (Vector3.Distance(_targetObj.transform.position, targetFloor.NPCOutPos) >= npc.StoppingDistance)
        {
            Vector3 direction2 = (targetFloor.NPCOutPos - _targetObj.transform.position).normalized;
            direction2.y = 0f;
            _targetObj.transform.position += direction2 * npc.MoveSpeed * Time.deltaTime;
            Quaternion targetRotation2 = Quaternion.LookRotation(-direction2);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation2, npc.RotationSpeed * Time.deltaTime);
            yield return null;
        }
        npc.IsFreedom = true;
        npc.FinWarpHandler(RoomAIState.EXIT_ROOM, _roomDetails.RoomNum);
    }

}
