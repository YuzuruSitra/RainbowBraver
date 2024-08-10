using System;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatSequencer : MonoBehaviour
    {
        private CombatManager _manager;
        
        public void Start()
        {
            if (_manager == null)
            {
                Debug.LogError("Reference to Combat Manager is null! Make sure you attached combatManager.cs to this GameObject.\nTrying to get instance...");
                _manager = GetComponent<CombatManager>();
            }
            
            _manager.Commence();
        }

        public void Setup(int id, UnitAlly[] allies)
        {
            _manager = GetComponent<CombatManager>();
            
            _manager.Setup(id, allies);
        }
    }
}