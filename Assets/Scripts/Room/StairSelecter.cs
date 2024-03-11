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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 出ていく座標の選定
    public Stair FloorSelecter(int calledFloor)
    {
        if (calledFloor == 0) return _stairs[calledFloor + 1];
        if (calledFloor + 1 == _maxFloor) return _stairs[calledFloor - 1];
        int rnd = Random.Range(0, 2);
        if (rnd == 0) return _stairs[calledFloor - 1];
        else return _stairs[calledFloor + 1];
    }
}
