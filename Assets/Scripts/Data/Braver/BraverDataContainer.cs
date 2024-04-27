using System.Collections.Generic;
using UnityEngine;

// Braverのデータ保持クラス
public class BraverDataContainer
{
    // シングルトン
    private static BraverDataContainer instance;
    public static BraverDataContainer Instance => instance ??= (instance = new BraverDataContainer());

    private List<BraverData> _braversData = new List<BraverData>();
    public List<BraverData> BraversData => _braversData;

    private BraverDataContainer()
    {
        // セーブデータの読み込み処理
        ////
    }

    public struct BraverData
    {
        public int jobId;

        public int maxHp;
        public int maxMp;
        public int hp;
        public int mp;

        public float pAtk;
        public float pDef;

        public float mAtk;
        public float mDef;

        public float speed;

        public List<FriendShipLevel> friendShipLevel;
    }

    public struct FriendShipLevel
    {
        public string id;
        public int level;
        public float exp;
    }

    public void ChangeBraverData(object caller, int id, BraverData newData)
    {
        if (!(caller is BraverDataChanger))
        {
            Debug.LogError($"Access denied: Only BraverDataChanger can change the data.");
            return;
        }

        _braversData[id] = newData;
    }

    public void AddBraver()
    {
        // テンプレートから情報をリストに追加
        ////
    }

}
