using UnityEngine;
using CI.QuickSave;
using System; 

public class ParameterIO
{
    private QuickSaveReader _reader;
    private QuickSaveWriter _writer;

    public ParameterIO()
    {
        // データの保存先をApplication.persistentDataPathに変更
        QuickSaveGlobalSettings.StorageLocation = Application.persistentDataPath;
        // QuickSaveSettingsのインスタンスを作成
        QuickSaveSettings settings = new QuickSaveSettings();
        // 暗号化の方法
        settings.SecurityMode = SecurityMode.Aes;
        // 暗号化キー
        settings.Password = "Password";
        // 圧縮の方法
        settings.CompressionMode = CompressionMode.Gzip;

        _writer = QuickSaveWriter.Create("Player", settings);
        _reader = QuickSaveReader.Create("Player", settings);
    }

    // Save
    public void DoSave(int value)
    {
        _writer.Write("tmp", value);
        _writer.Commit();
    }

    // Load
    public int DoLoad()
    {
        try
        {
            return _reader.Read<int>("tmp");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("データの読み込みエラー: " + ex.Message);
            return 0; 
        }
    }

    // Delete
    public void DeleteData()
    {
        // 初期データ
        _writer.Write("tmp", 0);
        // データを書き込む
        _writer.Commit();
    }
}
