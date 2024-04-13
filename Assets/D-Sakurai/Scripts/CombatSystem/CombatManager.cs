using System;
using System.Collections.Generic;
using System.Linq;
using D_Sakurai.Resources.Enemy;
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
            _ongoing = true;
            
            // Load()が手違いで呼ばれても進行中の戦闘に支障が無いように
            Duty currentDuty = _data;

            foreach (var phase in currentDuty.Phases)
            {
                Phase(phase);
            }

            _ongoing = false;
        }

        private void Phase(Phase phaseData)
        {
            //  PrePhase
            // -------------------
            
            // Load all enemy data from scriptable
            // phaseData.enemyIdsを使って.Select?か何かで必要な要素だけを直接配列に格納したら軽い気がしたけど、
            // 結局読みだす段階で一度メモリに置かれてる気がするのでやめた。
            EnemyData[] enemiesData = UnityEngine.Resources.Load<Enemies>("Enemy/EnemyData").EnemiesData;

            // Store needed EnemyData in enemies
            List<EnemyData> neededEnemyData = new List<EnemyData>();
            foreach (var id in phaseData.EnemyIds)
            {
                neededEnemyData.Add(enemiesData[id]);
            }

            // List of instantiated UnitEnemy
            UnitEnemy[] enemies = new UnitEnemy[neededEnemyData.Count];
            // TODO: Select()がいまいちわかっていない
            foreach ((EnemyData val, int idx) enemy in neededEnemyData.Select((value, idx) => (value, idx)))
            {
                var unt = enemy.val;
                enemies[enemy.idx] = new UnitEnemy(Affiliation.Enemy, unt.MaxHp, 999, unt.PAtk, unt.GenericAttackLabel, unt.PDef, unt.MAtk, unt.GenericAttackLabel, unt.MDef, unt.Speed);
            }

            //  Turn
            // ---------------
            do
            {
                Turn(_allies, enemies);
            } while (CheckAnnihilation(_allies, enemies));
            
            //  PostPhase
            // --------------------
        }

        private void Turn(UnitAlly[] allies, UnitEnemy[] enemies)
        {
            
        }

        private bool CheckAnnihilation(UnitAlly[] allies, UnitEnemy[] enemies)
        {
            // Extract living UnitAlly
            UnitAlly[] livingAllies = Array.FindAll(allies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingAllies.Length is 0 or shorter
            if (livingAllies.Length <= 0) return true;

            // Extract living UnitEnemy
            UnitEnemy[] livingEnemies = Array.FindAll(enemies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingEnemies.Length is 0 or shorter
            if (livingEnemies.Length <= 0) return true;

            // Return false(dont annihilated)
            return false;
        }
        
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