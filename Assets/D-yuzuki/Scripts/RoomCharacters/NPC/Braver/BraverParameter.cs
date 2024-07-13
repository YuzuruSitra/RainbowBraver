using System;
using Unity.Mathematics;
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
            Speed
        }
        
        // _upValueはParameterに対応した7要素
        [Serializable]
        public struct RoomEffect
        {
            public RoomType _roomType;
            public float[] _upValue;
        }

        [Header("部屋のステータス上昇値")] [SerializeField] private RoomEffect[] _roomEffects;
        public RoomEffect[] RoomEffects => _roomEffects;
        // 今後ロード予定
        private int _braverCount = 1;
        public float[,] Parameters { get; private set; }
        
        private void Start()
        {
            Parameters = new float[_braverCount, Enum.GetValues(typeof(Parameter)).Length];
        }

        public void UpdateStatus(int braverNum, Parameter targetParam, float newValue)
        {
            Parameters[braverNum, (int)targetParam] = newValue;
        }
        
    }
}
