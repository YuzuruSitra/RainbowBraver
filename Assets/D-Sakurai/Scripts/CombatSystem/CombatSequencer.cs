using System;
using D_Sakurai.Scripts.CombatSystem.Units;
using D_Sakurai.Scripts.PreCombat;
using Resources.Duty;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatSequencer : MonoBehaviour
    {
        private CombatManager _manager;
        
        [SerializeField] private bool DontUseSingleton;

        [SerializeField] private int DutyId;
        [SerializeField] private Tester.TestBraver[] Allies;

        private int _id;
        private UnitAlly[] _allies;
        
        public void Start()
        {
            if (DontUseSingleton)
            {
                // [DEBUG] インスペクタで設定した値で依頼を開始する際の処理
                _id = DutyId;
                _allies = Tester.GetInstancedBravers(Allies);
            }
            else
            {
                // Singletonから得た情報で依頼を開始する際の処理
                var inst = DutyDispatcher.SingletonInstance;
                
                if (!inst)
                {
                    Debug.LogError(
                        "DutyDispatcher doesn't exist!" +
                        "Enable 'DontUseSingleton' if you want to use parameters set in inspector."
                        );
                }
                
                _id = DutyDispatcher.SingletonInstance.DutyId;
                _allies = DutyDispatcher.SingletonInstance.RegisteredAllies;
            }
         
            // マネージャーを取得
            _manager = GetComponent<CombatManager>();
            
            if (_manager == null)
            {
                Debug.LogError("Reference to Combat Manager is null! Make sure you attached combatManager.cs to this GameObject.\nTrying to get instance...");
            }
            
            // 依頼をセットアップ
            _manager.Setup(_id, _allies);
            
            // 依頼開始
            _manager.Commence();
        }
    }
}