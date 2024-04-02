using System;
using System.Collections;
using System.Collections.Generic;
using CI.QuickSave;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 設定を保持・管理するクラス
/// </summary>
public class PreferenceManager : MonoBehaviour
{
    [field: Header("テキストの速さ (int, 文字/秒想定)")]
    [field: SerializeField] public int TextSpeed {get; private set;}


    [field: Space(20)]


    [field: Header("解像度 (int2)")]
    [field: SerializeField] public int2 Resolution {get; private set;}

    [field: Header("ウィンドウ設定 (int, 0: window, 1: borderless, 2: fullscreen)")]
    [field: SerializeField] public int WindowMode {get; private set;}


    [field: Space(20)]


    [field: Header("マスター音量 (float, 0-1, clampされます)")]
    [field: SerializeField] public float VolMaster {get; private set;}
    [field: Header("SE音量 (float, 0-1, clampされます)")]
    [field: SerializeField] public float VolSe {get; private set;}
    [field: Header("BGM音量 (float, 0-1, clampされます)")]
    [field: SerializeField] public float VolBgm {get; private set;}
    [field: Header("環境音音量 (float, 0-1, clampされます)")]
    [field: SerializeField] public float VolEnv {get; private set;}

    /// <summary>
    /// 既に保存されている設定データがある場合それをロード
    /// </summary>
    void Start(){
        try
        {
            // 既に設定が保存されている場合ロード
            // 保存されていなかった場合QuickSaveReader.Create()の時点で例外が投げられるのでTryCatchで対応
            var _reader = QuickSaveReader.Create("Preference");

            TextSpeed = _reader.Read<int>("TextSpeed");

            Resolution = _reader.Read<int2>("Resolution");
            WindowMode = _reader.Read<int>("WindowMode");

            VolMaster = _reader.Read<float>("VolMaster");
            VolSe = _reader.Read<float>("VolSe");
            VolBgm = _reader.Read<float>("VolBgm");
            VolEnv = _reader.Read<float>("VolEnv");
        }
        catch (System.Exception e)
        {
            Debug.Log("No saved preferences. Exception catched:\n" + e);
        }

        VolMaster = Mathf.Clamp01(VolMaster);
        VolSe = Mathf.Clamp01(VolSe);
        VolBgm = Mathf.Clamp01(VolBgm);
        VolEnv = Mathf.Clamp01(VolEnv);
    }

    // ここもうちょっと知性を感じられる書き方に改めたいです　考え直しておきます　さくらい
    public void ChangeTextSpeed(float value) {
        Debug.Log(value);

        TextSpeed = Mathf.RoundToInt(value);
    }
    
    public void ChangeResolution(int2 value) {
        Debug.Log(value);

        Resolution = value;
        
        Screen.SetResolution(Resolution.x, Resolution.y, Screen.fullScreenMode);
    }
    public void ChangeWindowMode(int value) {
        Debug.Log(value);

        WindowMode = value;

        switch (WindowMode)
        {
            case 0:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.Windowed);
                break;
            case 1:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(Resolution.x, Resolution.y, FullScreenMode.ExclusiveFullScreen);
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Window mode index out of range! (0: windowed, 1: borderless, 2: fullscreen)"
                );
        }
    }
    
    public void ChangeVolMaster(float value) {
        Debug.Log(value);

        VolMaster = Mathf.Clamp01(value);
    }
    public void ChangeVolSe(float value) {
        Debug.Log(value);

        VolSe = Mathf.Clamp01(value);
    }
    public void ChangeVolBgm(float value) {
        Debug.Log(value);

        VolBgm = Mathf.Clamp01(value);
    }
    public void ChangeVolEnv(float value) {
        Debug.Log(value);

        VolEnv = Mathf.Clamp01(value);
    }
    
    /// <summary>
    /// 変更を保存
    /// </summary>
    public void SaveChange(){
        // var _writer = QuickSaveWriter.Create("Preference");
    
        // _writer.Write("TextSpeed", TextSpeed);
    
        // _writer.Write("Resolution", Resolution);
        // _writer.Write("WindowMode", WindowMode);
    
        // _writer.Write("VolMaster", VolMaster);
        // _writer.Write("VolSe", VolSe);
        // _writer.Write("VolBgm", VolBgm);
        // _writer.Write("VolEnv", VolEnv);
    }
}
