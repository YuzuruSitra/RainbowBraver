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

        // �G���g���[ 
        braver.InnNPCMover.SetTarGetPos(_entryPos);
        while (!braver.InnNPCMover.IsAchieved)
        {
            braver.InnNPCMover.Moving();
            yield return null;
        }

        // 階層のワープ
        _targetObj.transform.position = _pairLift.EntryPos;
        yield return _waitTime;

        // �ޏo    
        braver.InnNPCMover.SetTarGetPos(_pairLift.NPCOutPos);
        while (!braver.InnNPCMover.IsAchieved)
        {
            braver.InnNPCMover.Moving();
            yield return null;
        }

        braver.IsFreedom = true;
        braver.FinWarpHandler(_pairLift.RoomDetails.RoomNum);
    }

}
