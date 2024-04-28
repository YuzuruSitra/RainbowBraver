using System;
using UnityEngine;
using System.Linq;
using Vector4 = UnityEngine.Vector4;

using Resources.Duty;
using D_Sakurai.Resources.Enemy;

using D_Sakurai.Scripts.CombatSystem.Units;
using Unit = D_Sakurai.Scripts.CombatSystem.Units.Unit;
using CUtil = D_Sakurai.Scripts.CombatSystem.CombatUtilities;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        private Duty _data;
        private UnitAlly[] _allies;
        
        private bool _ongoing;

        private float _specialAttackGauge;
        
        [System.Serializable]
        public struct DecisionThreshData
        {
            [Header("行動の閾値(%)の設定。x: 状態異常の解除確率, y: 回復の発動基準体力残量, z: バフ・デバフがかかっていない際の使用確率, w: MPを消費する攻撃スキルの仕様確率")]
            [Header("ガンガンいこうぜ")]
            [SerializeField] public Vector4 Offensive;
            [Header("命大事に")]
            [SerializeField] public Vector4 Defensive;
            [Header("搦め手優先")]
            [SerializeField] public Vector4 Technical;
            [Header("おまかせ")]
            [SerializeField] public Vector4 Default;
        }

        [SerializeField] private DecisionThreshData Thresholds;
        private Vector4 _decisionThreshold;

        private enum Strategies {Offensive, Defensive, Technical, Default};
        [SerializeField] private Strategies Strategy;
        
        /// <summary>
        /// 開始する依頼のセットアップ
        /// </summary>
        /// <param name="id">開始する依頼のID</param>
        /// <param name="allies">味方のUnitAllyを格納した配列</param>
        /// <exception cref="Exception">既に別の依頼が進行中である場合</exception>
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
            
            // 各種閾値を0. - 1.に
            // ここ処理ダサい
            switch (Strategy)
            {
                case Strategies.Offensive:
                    _decisionThreshold = Thresholds.Offensive / 100;
                    break;
                case Strategies.Defensive:
                     _decisionThreshold = Thresholds.Defensive / 100;
                    break;
                case Strategies.Technical:
                     _decisionThreshold = Thresholds.Technical / 100;
                    break;
                case Strategies.Default:
                     _decisionThreshold = Thresholds.Default / 100;
                    break;
            }

            // TODO: Instantiate GameObjects and assign them to each Unit
            // TODO: 技データに発動させるエフェクト情報を仕込む予定
        }
        
        /// <summary>
        /// 依頼の開始
        /// </summary>
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

        /// <summary>
        /// 敵1グループが出現してから、プレイヤー側か敵側のどちらかが全滅するまでの期間
        /// </summary>
        /// <param name="phaseData">Phaseの内容の定義</param>
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
                enemies[idx] = new UnitEnemy(
                    Affiliation.Enemy,
                    unt.MaxHp,
                    9999,// 敵にはMP概念が無いので
                    unt.PAtk,
                    unt.GenericAttackLabel,
                    unt.PDef,
                    unt.MAtk,
                    unt.GenericAttackLabel,
                    unt.MDef,
                    unt.Speed,
                    unt.EnemySkillIds
                    );
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

                annihilationData = CUtil.CheckAnnihilation(_allies, enemies);
            } while (annihilationData.Item1);
            
            //  PostPhase
            // --------------------
            Debug.Log(annihilationData.Item2 == Affiliation.Player ? "Player team win" : "Enemy team win");
        }

        /// <summary>
        /// 戦闘に参加しているUnitが行動を行う期間
        /// </summary>
        /// <param name="allUnits">全てのUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        /// <param name="current">現在の経過ターン数</param>
        private void Turn(Unit[] allUnits, UnitEnemy[] enemies, int current)
        {
            //  PreTurn
            // ------------------
            Unit next = CUtil.GetNextUnit(allUnits);
            
            
            //  Eval / Action
            // -----------------------
            if (next is UnitAlly ally)
            {
                // Eval / Action code for Player
                CUtil.EvalAlly(ally, allUnits, _allies, enemies, _decisionThreshold);
            }
            else if (next is UnitEnemy enemy)
            {
                CUtil.EvalEnemy(enemy, allUnits, enemies, _allies);
            }
            else
            {
                Debug.LogError("[CombatManager -> Duty -> Phase -> Turn -> Eval]: Unit with unexpected type was given.");
            }
        }
        
        // Unit.GiveDamage -> Unit.ReceiveDamage等にリプレイス予定
        // TODO: 前述の仕組みが無事に動いたら消す
        void PhysicalAttack(Unit subject, Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
        void MagicalAttack(Unit subject, Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Heal(Unit subject, Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Effect(Unit subject, Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
    }
}