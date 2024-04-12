using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "./Scripts/Scriptables/BraversList", menuName = "Bravers List", order = 0)]
    public class BraversList : ScriptableObject
    {
        public List<BraverData> braversData;
    }

    [System.Serializable]
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

    [System.Serializable]
    public struct FriendShipLevel
    {
        public string id;
        public float level;
    }
}