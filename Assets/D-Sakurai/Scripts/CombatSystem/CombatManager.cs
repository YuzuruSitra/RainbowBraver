using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Resources.Duty;
using D_Sakurai.Resources.Enemy;
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
            
            // TODO: Instantiate GameObjects and assign them to each Unit
            // TODO: Unitに使用する技を持たせる?
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
            // NOTE: phaseData.enemyIdsを使って.Selectか何かで必要な要素だけを直接配列に格納したら軽い気がしたけど、
            // 読みだす段階で一度メモリに置かれてそうで結局変わらない気がするのでやめた。
            EnemyData[] enemiesData = UnityEngine.Resources.Load<Enemies>("Enemy/EnemyData").EnemiesData;

            // Store needed EnemyData in enemies
            EnemyData[] neededEnemyData = new EnemyData[phaseData.EnemyIds.Length];
            foreach (var (value, idx) in phaseData.EnemyIds.Select((value, idx) => (value, idx)))
            {
                neededEnemyData[idx] = enemiesData[value];
            }

            // List of instantiated UnitEnemy
            UnitEnemy[] enemies = new UnitEnemy[neededEnemyData.Length];
            foreach ((var unt, int idx) in neededEnemyData.Select((unt, idx) => (unt, idx)))
            {
                enemies[idx] = new UnitEnemy(Affiliation.Enemy, unt.MaxHp, 999, unt.PAtk, unt.GenericAttackLabel, unt.PDef, unt.MAtk, unt.GenericAttackLabel, unt.MDef, unt.Speed);
            }
            
            // Make array of all units for convenience
            Unit[] allUnits = new Unit[_allies.Length + enemies.Length];
            Array.Copy(_allies, allUnits, _allies.Length);
            Array.Copy(enemies, 0, allUnits, _allies.Length, enemies.Length);

            //  Turn
            // ---------------
            var currentTurn = 0;
            (bool, Affiliation) annihilationData;
            do
            {
                Turn(allUnits, enemies, currentTurn);
                currentTurn++;

                annihilationData = CheckAnnihilation(_allies, enemies);
            } while (annihilationData.Item1);
            
            //  PostPhase
            // --------------------
            if (annihilationData.Item2 == Affiliation.Player)
            {
                Debug.Log("Player team win");
            }
            else
            {
                Debug.Log("Enemy team win");
            }
        }

        private void Turn(Unit[] allUnits, UnitEnemy[] enemies, int current)
        {
            //  PreTurn
            // ------------------
            Unit next = GetNextUnit(allUnits);
            
            
            //  Eval / Action
            // -----------------------
            if (next is UnitAlly unt)
            {
                switch (unt.Job)
                {
                    
                }
                // Eval code for Player
            }
        }
        
        private static (bool, Affiliation) CheckAnnihilation(UnitAlly[] allies, UnitEnemy[] enemies)
        {
            // Extract living UnitAlly
            UnitAlly[] livingAllies = Array.FindAll(allies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingAllies.Length is 0 or shorter
            if (livingAllies.Length <= 0) return (true, Affiliation.Player);

            // Extract living UnitEnemy
            UnitEnemy[] livingEnemies = Array.FindAll(enemies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingEnemies.Length is 0 or shorter
            if (livingEnemies.Length <= 0) return (true, Affiliation.Enemy);

            // Return false(dont annihilated)
            return (false, Affiliation.Player);
        }

        private static Unit GetNextUnit(Unit[] allUnits)
        {
            Unit result = allUnits[0];
            float currentFastest = allUnits[0].Speed;

            foreach (var unit in allUnits)
            {
                // If unit is not yet actioned and faster than current fastest unit
                if (unit.Actioned && unit.Speed > currentFastest)
                {
                    result = unit;
                }
            }

            return result;
        }

        // TODO: Unitに使用する技のインスタンスを含める
        // private static EvalAlly(Unit[] allUnits, UnitAlly[] allies, UnitEnemy[] enemies)
        // {
        //     
        // }
        
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