using System;
using UnityEngine;

// 一日の時間を保持するクラス
public class DayTimeKeeper : MonoBehaviour
{
    [Header("実際の1日の時間 (分)")]
    [SerializeField]
    private float _dayMinute;
    private int MAX_DAY_TIME = 24;
    // 時間の経過速度/分
    private float _elapsedTimeSpeed;
    private TimeSpan _currentTime = TimeSpan.Zero;
    // 現在の時間
    public TimeSpan CurrentTime => _currentTime;
    private float _currentHourRatio;
    // 現在の割合
    public float CurrentHourRatio => _currentHourRatio;

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTimeSpeed = MAX_DAY_TIME / _dayMinute * 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = _currentTime.Add(TimeSpan.FromSeconds(_elapsedTimeSpeed * Time.deltaTime));
        // 経過時間の割合計算
        float currentHourInDay = _currentTime.Days * MAX_DAY_TIME + (float)_currentTime.TotalHours % MAX_DAY_TIME;
        _currentHourRatio = currentHourInDay / MAX_DAY_TIME;
    }
}
