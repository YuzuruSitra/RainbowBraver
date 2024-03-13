using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSelecter : MonoBehaviour
{
    [Header("階段を格納")]
    [SerializeField]
    private Stair[] _stairs;
    [Header("npcの目標座標エラー値(エマの部屋)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    private int _maxFloor => _stairs.Length;
    private RoomSelecter _roomSelecter;

    // Start is called before the first frame update
    void Start()
    {
        _roomSelecter = GameObject.FindWithTag("PathSelecter").GetComponent<RoomSelecter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 出ていく座標の選定
    public Stair FloorSelecter(int calledFloor, int baseRoom)
    {
        List<int> stairList = _roomSelecter.SearchStairs(calledFloor, baseRoom);
        int rnd = Random.Range(0, stairList.Count);
        return _stairs[stairList[rnd]];
    }
}
