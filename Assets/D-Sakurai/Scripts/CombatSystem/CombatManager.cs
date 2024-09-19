using System;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using Vector4 = UnityEngine.Vector4;

using Resources.Duty;
using D_Sakurai.Resources.Enemy;
using D_Sakurai.Resources.StatusEffects.StatusEffectBase;
using D_Sakurai.Scripts.CombatSystem.Units;
using D_Sakurai.Scripts.PreCombat;
using Unit = D_Sakurai.Scripts.CombatSystem.Units.Unit;
using CUtil = D_Sakurai.Scripts.CombatSystem.CombatUtilities;

namespace D_Sakurai.Scripts.CombatSystem
{
    /// <summary>
    /// 戦闘を管理するクラス
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        public const float MiasmaDamageRate = .3f;
        public const float AngerDamageMultiplier = 1.5f;

        private Duty _data;
        private UnitAlly[] _allies;
        
        private bool _ongoing;

        private float _specialAttackGauge;
        
        [Serializable]
        public struct DecisionThreshData
        {
            [Header("行動の閾値(%)の設定。\nx: 状態異常の解除確率\ny: 回復の発動基準体力残量\nz: バフ・デバフがかかっていない際の使用確率\nw: MPを消費する攻撃スキルの仕様確率")]
            [Header("ガンガンいこうぜ")]
            [SerializeField] public Vector4 offensive;
            [Header("命大事に")]
            [SerializeField] public Vector4 defensive;
            [Header("搦め手優先")]
            [SerializeField] public Vector4 technical;
            [Header("おまかせ")]
            [SerializeField] public Vector4 normal;
        }

        [SerializeField] private DecisionThreshData thresholds;
        private Vector4 _decisionThreshold;

        private enum Strategies {Offensive, Defensive, Technical, Default};
        [SerializeField] private Strategies strategy;

        [SerializeField] private bool logCombat; 
        
        /// <summary>
        /// 開始する依頼のセットアップ
        /// </summary>
        /// <param name="id">開始する依頼のID</param>
        /// <param name="allies">味方のUnitAllyを格納した配列</param>
        public void Setup(int id, UnitAlly[] allies)
        {
            if (_ongoing) Debug.LogError("Another duty ongoing!");
            
            _data = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData[id];
            _allies = allies;
            
            // 各種閾値を0. - 1.に
            // ここ処理ダサい
            switch (strategy)
            {
                case Strategies.Offensive:
                    _decisionThreshold = thresholds.offensive / 100;
                    break;
                case Strategies.Defensive:
                     _decisionThreshold = thresholds.defensive / 100;
                    break;
                case Strategies.Technical:
                     _decisionThreshold = thresholds.technical / 100;
                    break;
                case Strategies.Default:
                     _decisionThreshold = thresholds.normal / 100;
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
        private async void Phase(Phase phaseData)
        {
            //  PrePhase
            // -------------------
            
            // Load all enemy data from scriptable
            // あるとしたら一度全て読んでフィルタする(現行)よりすぐGCしてくれる？ 不要な変数が存在しなくなるから
            EnemyData[] enemiesData = UnityEngine.Resources.Load<Enemies>("Enemy/EnemyData").EnemiesData;

            // Store needed EnemyData in enemies
            EnemyData[] neededEnemyData = new EnemyData[phaseData.EnemyIds.Length];
            foreach (var (enemyIdx, arrayIdx) in phaseData.EnemyIds.Select((value, idx) => (value, idx)))
            {
                neededEnemyData[arrayIdx] = enemiesData[enemyIdx];
            }

            // List of instantiated UnitEnemy
            UnitEnemy[] enemies = new UnitEnemy[neededEnemyData.Length];
            foreach ((var unt, int idx) in neededEnemyData.Select((unt, idx) => (unt, idx)))
            {
                enemies[idx] = new UnitEnemy(
                    unt.Name,
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
                    unt.EnemySkillIds,
                    unt.GenericSkillAttribute,
                    unt.SkillThreshold
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
                var aliveAllies = _allies.Where(unt => !unt.IsDead).ToArray();
                var aliveEnemies = enemies.Where(unt => !unt.IsDead).ToArray();
                
                foreach (var unt in allUnits)
                {
                    unt.UpdateStatusEffects();
                }
                
                // TURN
                currentTurn++;
                var currentUnit = CUtil.GetNextUnit(allUnits);
                
                // 眠り
                if (CUtil.HasEffectType(currentUnit.StatusEffects, StatusEffectType.Sleep))
                {
                    Debug.Log($"{currentUnit.Name}: [ SKIP TURN ]");
                }
                else
                {
                    Turn(allUnits, aliveAllies, aliveEnemies, currentTurn, currentUnit);
                }
                
                // 瘴気
                if (
                    !currentUnit.IsDead &&
                    CUtil.HasEffectType(currentUnit.StatusEffects, StatusEffectType.Miasma)
                    )
                {
                    CUtil.Miasma(currentUnit, MiasmaDamageRate);
                }
                
                // check unit health
                foreach (var unt in allUnits.Where(unt => !unt.IsDead).ToArray())
                {
                    unt.HealthCheck();
                }
                
                annihilationData = CUtil.CheckAnnihilation(_allies, enemies);

                // (DEV) delay for preview
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            } while (!annihilationData.Item1);
            
            //  PostPhase
            // --------------------
            Debug.Log(annihilationData.Item2 == Affiliation.Enemy ? $"Player team win, Elapsed turns: {currentTurn}" : $"Enemy team win, Elapsed turns: {currentTurn}");
        }

        /// <summary>
        /// 戦闘に参加しているUnit一体が行動を行う期間
        /// </summary>
        /// <param name="allUnits">全てのUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        /// <param name="allies">プレイヤー側のUnitを格納した配列</param>
        /// <param name="current">現在の経過ターン数</param>
        /// <param name="next">このターン行動するUnit</param>
        private void Turn(Unit[] allUnits, UnitAlly[] allies, UnitEnemy[] enemies, int current, Unit next)
        {
            //  Eval / Action
            // -----------------------
            if (next is UnitAlly ally)
            {
                // Eval / Action code for Player
                CUtil.EvalAlly(ally, allUnits, allies, enemies, _decisionThreshold);
            }
            else if (next is UnitEnemy enemy)
            {
                CUtil.EvalEnemy(enemy, allUnits, enemies, allies);
            }
            else
            {
                Debug.LogError("[CombatManager -> Duty -> Phase -> Turn -> Eval]: Unit with unexpected type was given.");
            }
        }
    }
}