using System.Collections;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public enum LiftInfo
    {
        UPPER,
        LOWER
    }

    [SerializeField] private LiftInfo _info;
    public LiftInfo Info => _info;

    [SerializeField] private Lift _pairLift;

    [Header("移動時の待機時間")]
    [SerializeField] private float _waitTime;

    private RoomDetails _roomDetails;
    public RoomDetails RoomDetails => _roomDetails;

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
        // NPC Controller の共通インターフェースを使う
        if (other.CompareTag("RoomBraver") || other.CompareTag("RoomMaid"))
        {
            // 手前からの侵入のみ許可
            var direction = (transform.position - other.transform.position).normalized;
            if (direction.z <= 0) return;
            var target = other.gameObject;
            StartCoroutine(AutoMoving(target));
        }
    }

    // 汎用的な自動移動処理
    private IEnumerator AutoMoving(GameObject target)
    {
        var npc = target.GetComponent<INPCController>();
        npc.IsFreedom = false;

        npc.InnNPCMover.SetTarGetPos(_entryPos);
        while (!npc.InnNPCMover.IsAchieved)
        {
            npc.InnNPCMover.Moving();
            yield return null;
        }

        // 階層のワープ
        target.transform.position = _pairLift.EntryPos;
        yield return new WaitForSeconds(_waitTime);

        npc.InnNPCMover.SetTarGetPos(_pairLift.NPCOutPos);
        while (!npc.InnNPCMover.IsAchieved)
        {
            npc.InnNPCMover.Moving();
            yield return null;
        }

        npc.IsFreedom = true;
        npc.FinWarpHandler(_pairLift.RoomDetails.RoomNum);
    }
}