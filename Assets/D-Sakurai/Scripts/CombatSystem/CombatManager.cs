using System;
using System.Linq;
using D_Sakurai.Scripts.CombatSystem.Enemy;
using Resources.Duty;
using UnityEngine;

using D_Sakurai.Scripts.CombatSystem.Units;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        private Duty _data;
        private UnitAlly[] _allies;
        
        private bool _ongoing;
        
        public void Setup(int id, UnitAlly[] allies)
        {
            if (!_ongoing)
            {
                _data = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData[id];
                _allies = allies;
            }
            else
            {
                throw new Exception("Another duty ongoing!");
            }
        }
        
        public void Commence()
        {
            // Load()が手違いで呼ばれても進行中の戦闘に支障が無いように
            Duty currentDuty = _data;

            foreach (var phase in currentDuty.Phases)
            {
                Phase(phase);
            }
        }

        private void Phase(Phase phaseData)
        {
            //  PrePhase
            // -------------------
            
            // TODO: 敵をインスタンシングしてUnitEnemy[]に格納、Turnに渡して戦闘を行わせる
            // UnityEngine.Resources.Load<Enemies>("")
            
            // Instantiate Enemy Units
            // UnitEnemy[] enemies = phaseData.EnemyIds.Select(enemyId => new UnitEnemy[] new UnitEnemy(Affiliation.Enemy, ))
            
            
            // Turn
            // Turn(phaseData);
        }

        private void Turn(Unit[] units, UnitEnemy[] enemies)
        {
            
        }
        
        // TODO: 戦闘を実際に進行するマネージャー
        void PhysicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
        void MagicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Heal(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Effect(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
    }
}