using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using System;
using UnityEditor.Build.Content;

public class SaveDataHandler : MonoBehaviour
{
    [Header("セーブスロット数")]
    [SerializeField] int n_slots;

    /// <summary>
    /// セーブ/ロード処理の実行結果を保持する構造体
    /// </summary>
    public struct SaveLoadResult
    {
        public bool Successful;
        public string StatusText;
    }

    /// <summary>
    /// [デバッグ用]
    /// Eventを利用して呼ぶ際、void以外の型を受け付けてくれないためテスト用に実装。
    /// 結果が返るので、実際の実装の際は可能な限りHardSave()を呼んでください。
    /// </summary>
    /// <param name="slot">セーブするスロット</param>
    public void HardSaveVoid(int slot){
        SaveLoadResult result = _Save(slot, true);
        Debug.Log("Save successful?: " + result.Successful + "\n" + result.StatusText);
    }

    /// <summary>
    /// [デバッグ用]
    /// Eventを利用して呼ぶ際、void以外の型を受け付けてくれないためテスト用に実装。
    /// 結果が返るので、実際の実装の際は可能な限りSoftSave()を呼んでください。
    /// </summary>
    /// <param name="slot">セーブするスロット</param>
    public void SoftSaveVoid(int slot){
        SaveLoadResult result = _Save(slot, false);
        Debug.Log("Save successful?: " + result.Successful + "\n" + result.StatusText);
    }

    /// <summary>
    /// [デバッグ用]
    /// Eventを利用して呼ぶ際、void以外の型を受け付けてくれないためテスト用に実装。
    /// 結果が返るので、実際の実装の際は可能な限りLoad()を呼んでください。
    /// </summary>
    /// <param name="slot">セーブするスロット</param>
    public void LoadVoid(int slot){
        SaveLoadResult result = _Load(slot);
        Debug.Log("Load successful?: " + result.Successful + "\n" + result.StatusText);
    }

    /// <summary>
    /// セーブ処理を行う際に外部から呼ぶ関数。
    /// 目的のスロットにデータが存在していても上書きを行う。行わない場合はSoftSave()。
    /// </summary>
    /// <param name="slot">セーブするスロット</param>
    /// <returns>セーブ処理の実行結果</returns>
    public SaveLoadResult HardSave(int slot){
        SaveLoadResult result = _Save(slot, true);
        Debug.Log("Save successful?: " + result.Successful + "\n" + result.StatusText);

        return result;
    }

    /// <summary>
    /// セーブ処理を行う際に外部から呼ぶ関数。
    /// 目的のスロットにデータが存在していた場合上書きを行わない。行う場合はHardSave()。
    /// </summary>
    /// <param name="slot"セーブするスロット></param>
    /// <returns>セーブ処理の実行結果</returns>
    public SaveLoadResult SoftSave(int slot){
        SaveLoadResult result = _Save(slot, false);
        Debug.Log("Save successful?: " + result.Successful + "\n" + result.StatusText);

        return result;
    }

    /// <summary>
    /// ロード処理を行う際に外部から呼ぶ関数。
    /// </summary>
    /// <param name="slot">ロードするスロット</param>
    /// <returns>ロード処理の実行結果</returns>
    public SaveLoadResult Load(int slot){
        SaveLoadResult result = _Load(slot);
        Debug.Log("Load successful?: " + result.Successful + "\n" + result.StatusText);

        return result;
    }

    /// <summary>
    /// 実際にセーブ処理を行う関数。
    /// </summary>
    /// <param name="slot">セーブするスロット</param>
    /// <param name="allowOverwrite">上書きを行うか</param>
    /// <returns>実行結果</returns>
    private SaveLoadResult _Save(int slot, bool allowOverwrite){
        
        // Handle out of slot range error
        if (slot + 1 > n_slots)
        {
            return new SaveLoadResult {
                Successful = false,
                StatusText = "Given slot id " + slot + " is out of range! (number of slots: " + n_slots + " )"
            };
        }


        // -- SAVE --

        // Name of slot key
        string slotKey = "SaveSlot" + slot;

        // Create writer
        var _writer = QuickSaveWriter.Create(slotKey);
        
        // Overwrite check
        bool _exists = _writer.Exists("CreatedAt");

        if (_exists && !allowOverwrite)
        {
            return new SaveLoadResult {
                Successful = false,
                StatusText = "Slot [ " + slot + " ] already exists. Use HardSave() if you want to overwite it."
            };
        }


        // Write current Date/Time
        _writer.Write<string>("CreatedAt", DateTime.Now.ToString());

        // Actual write
        _writer.Commit();

        return new SaveLoadResult {
            Successful = true,
            StatusText = "Data saved to slot " + slot + " ( Key: " + slotKey + " ) ! Overwrited: " + _exists
        };
    }


    /// <summary>
    /// 実際のロード処理を行う関数。
    /// </summary>
    /// <param name="slot">読み込むスロット</param>
    /// <returns>実行結果</returns>
    private SaveLoadResult _Load(int slot){
        
        // Handle out of slot range error
        if (slot + 1 > n_slots)
        {
            return new SaveLoadResult {
                Successful = false,
                StatusText = "Given slot id " + slot + " is out of range! (number of slots: " + n_slots + " )"
            };
        }


        // -- LOAD --

        // Name of slot key
        string slotKey = "SaveSlot" + slot;

        // Create Reader
        // 実体を作らないとreader.Exists()を呼べないが、ルートになるキーが無い時に例外を吐くタイミングがこのCreate()なので防げない気がする。　うーん。
        var _reader = QuickSaveReader.Create("SaveSlot" + slot);

        // Read saved Date/Time and log it
        string _createdAt = _reader.Read<string>("CreatedAt");
        Debug.Log("[ DEBUG ] Last save: " + _createdAt);

        return new SaveLoadResult {
            Successful = true,
            StatusText = "Data loaded from slot " + slot + " ( Key: " + slotKey +" ) !"
        };
    }
}