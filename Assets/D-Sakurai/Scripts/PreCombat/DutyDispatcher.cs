using System;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public sealed class DutyDispatcher : MonoBehaviour
    {
        //  DATA CONTAINER
        // -------------------------
        public static DutyDispatcher SingletonInstance { get; private set; }

        public UnitAlly[] RegisteredAllies { get; private set; }
        public int DutyId { get; private set; }

        private void Awake()
        {
            if (SingletonInstance && this != SingletonInstance)
            {
                Destroy(gameObject);
            }

            SingletonInstance = this;
            
            DontDestroyOnLoad(this);
        }
        
        
        //  CONFIGURE
        // --------------------
        
        // 要らない気がしますが、経験がないので念のため
        private static bool DispatcherExists()
        {
            if (SingletonInstance) return true;
            
            Debug.Log($"Duty dispatcher(Singleton MonoBehavior) doesn't exist!");
            return false;

        }

        /// <summary>
        /// 依頼IDを設定する。依頼IDはScriptableObject: Dutiesのインデックス。
        /// </summary>
        /// <param name="id">ID</param>
        public static void RegisterDutyId(int id)
        {
            if (DispatcherExists()) SingletonInstance.DutyId = id;
        }

        /// <summary>
        /// 出発する味方を設定する。
        /// </summary>
        /// <param name="allies">味方を格納した配列。</param>
        public static void RegisterAllies(UnitAlly[] allies)
        {
            if (DispatcherExists()) SingletonInstance.RegisteredAllies = allies;
        }
    }
}