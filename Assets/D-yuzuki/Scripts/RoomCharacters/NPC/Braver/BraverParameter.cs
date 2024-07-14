using System;
using System.Collections.Generic;
using UnityEngine;

namespace D_yuzuki.Scripts.RoomCharacters.NPC.Braver
{
    public class BraverParameter : MonoBehaviour
    {
        public enum Parameter
        {
            Hp,
            Mp,
            Strength,
            Defense,
            MagicPower,
            MagicDefence,
            Speed,
        }
        
        // _upValueはParameterに対応した7要素
        [Serializable]
        public struct RoomEffect
        {
            public RoomType _roomType;
            public float[] _upValue;
            public float _friendPoint;
        }

        [Header("部屋のステータス上昇値")] [SerializeField] private RoomEffect[] _roomEffects;
        public RoomEffect[] RoomEffects => _roomEffects;
        // 今後ロード予定
        private int _braverCount = 2;
        public float[,] Parameters { get; private set; }
        public List<List<float>> Friendship { get; private set; }
        private void Start()
        {
            Parameters = new float[_braverCount, Enum.GetValues(typeof(Parameter)).Length];
            InitializeFriendship(_braverCount);
        }

        private void InitializeFriendship(int braverCount)
        {
            Friendship = new List<List<float>>(braverCount);
            for (var i = 0; i < braverCount; i++)
            {
                Friendship.Add(new List<float>(braverCount));
                for (var j = 0; j < braverCount; j++)
                {
                    Friendship[i].Add(0f); // 初期値として0を設定
                }
            }
        }

        public void UpdateStatus(int braverNum, Parameter targetParam, float newValue)
        {
            Parameters[braverNum, (int)targetParam] = newValue;
        }
        
        public void UpdateFriendship(int braverNum, int targetNum, float newValue)
        {
            if (braverNum >= Friendship.Count || targetNum >= Friendship[braverNum].Count) return;
            Friendship[braverNum][targetNum] = newValue;
        }
        
        // ブレーバーの人数が変わったとき用の処理を追記予定
        
    }
}
