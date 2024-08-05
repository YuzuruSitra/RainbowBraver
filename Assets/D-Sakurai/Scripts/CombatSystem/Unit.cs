using System;
using System.Collections.Generic;
using System.Linq;
using D_Sakurai.Resources.Skills;
using D_Sakurai.Resources.Skills.SkillBase;
using D_Sakurai.Resources.StatusEffects.StatusEffectBase;
using UnityEngine;

using CUtil = D_Sakurai.Scripts.CombatSystem.CombatUtilities;

namespace D_Sakurai.Scripts.CombatSystem
{
    namespace Units
    {
        /// <summary>
        /// Unitの所属を表すEnum
        /// </summary>
        public enum Affiliation{ Player, Enemy }
        
        /// <summary>
        /// UnitAllyの職業を表すEnum
        /// </summary>
        public enum Job{ Swordsman, Gladiator, Lancer, Hunter, Oracle, Sorcerer }
        
        /// <summary>
        /// [値は仮置き]
        /// UnitAllyの性格を表すEnum
        /// </summary>
        public enum Personality{Active, Sociable, Humble, Intelligent}

        /// <summary>
        /// ユニットのインターフェイス
        /// </summary>
        interface IUnitData
        {
            string Name { get; }
            
            Affiliation Affiliation { get; }
            
            bool IsDead { get; }

            int MaxHp { get; }
            int MaxMp { get; }

            float PAtk { get; }
            string PAtkLabel { get; }
            float PDef { get; }

            float MAtk { get; }
            string MAtkLabel { get; }
            float MDef { get; }

            int Speed { get; }
            
            bool Actioned { get; }

            GameObject GameObject { get; }
            UnitViewer Viewer { get; }
            Animator Animator { get; }
            
            SkillAttribute GenericSkillAttribute { get; }

            // UnitStatusEffectData StatusEffects { get; }
        }

        /// <summary>
        /// ユニットの基底クラス
        /// ユニットは、敵味方の別なく戦闘に参加する1主体を指す単位
        /// </summary>
        public class Unit : IUnitData
        {
            public string Name { get; }

            // 所属
            public Affiliation Affiliation { get; }

            // 倒されたか
            public bool IsDead { get; private set; }

            // 最大HP
            public int MaxHp { get; }

            // 最大MP
            public int MaxMp { get; }

            // HP
            // 戦闘中実際に変動するのはこちらの値
            public int Hp { get; private set; }

            // MP
            // 戦闘中実際に変動するのはこちらの値
            public int Mp { get; private set; }

            // 物理攻撃力
            public float PAtk { get; }

            // 通常物理攻撃用の技名
            public string PAtkLabel { get; }

            // 物理防御力
            public float PDef { get; }

            // 魔法攻撃力
            public float MAtk { get; }

            // 通常魔法攻撃用の技名
            public string MAtkLabel { get; }

            // 魔法防御力
            public float MDef { get; }

            // 素早さ
            public int Speed { get; }

            public SkillAttribute GenericSkillAttribute { get; protected set; }

            // 状態効果の影響を受けるステータスの、補正値
            public int MpModifier { get; set; }
            public int PAtkModifier { get; set; }
            public int PDefModifier { get; set; }
            public int MAtkModifier { get; set; }
            public int MDefModifier { get; set; }
            public int SpeedModifier { get; set; }
            public List<float> WeakMultipliers { get; set; } = new();

        // そのターン行動したか
            public bool Actioned { get; set; }

            public List<StatusEffectData> StatusEffects { get; set; }

            // public UnitStatusEffectData StatusEffects { get; }

            public GameObject GameObject { get; private set; }
            public UnitViewer Viewer { get; private set; }
            public Animator Animator { get; private set; }

            // このUnitとして扱うGameObjectを割り当てる
            // ほかの実装を優先すべきと判断したのでいったん先送り
            public void AssignGameObject(GameObject obj)
            {
                GameObject = obj;
                Viewer = obj.GetComponent<UnitViewer>();
                Animator = obj.GetComponent<Animator>();
            }
            
            // Animatorの特定のステートを呼び出す
            // TODO: Async/Awaitを利用してステートを抜けた際に戻すように書く
            public void CallAnimState(string trigger)
            {
                Animator.SetTrigger(trigger);
            }

            /// <summary>
            /// Unitが倒されたか判定する。Unit.Hpを読んで0以下ならUnit.IsDeadを更新する。
            /// </summary>
            public void HealthCheck()
            {
                if (Hp > 0) return;
                
                Debug.Log($"{Name}: [ DEAD ]");
                IsDead = true;
            }

            /// <summary>
            /// 状態効果を付与する
            /// </summary>
            /// <param name="ef">付与する状態効果</param>
            public void ApplyStatusEffect(StatusEffectData ef)
            {
                ef.Elapsed = 0;
                
                CUtil.InitStatusEffect(this, ef);
                StatusEffects.Add(ef);
            }

            /// <summary>
            /// 保持している状態効果を実行し、更新する
            /// </summary>
            public void UpdateStatusEffects()
            {
                foreach (var ef in StatusEffects)
                {
                    // CUtil.ExecuteStatusEffect(this, ef);
                    ef.Elapsed++;
                }

                var removals = StatusEffects.FindAll(ef => ef.Elapsed >= ef.Durability).ToArray();
                foreach (var ef in removals)
                {
                    RemoveStatusEffect(ef);
                }
            }

            /// <summary>
            /// リストの最も上にあるネガティブな状態効果を除去する
            /// </summary>
            private void RemoveFirstNegativeStatusEffect()
            {
                var target = StatusEffects.Find(ef => !ef.IsFriendly);
                
                RemoveStatusEffect(target);
            }

            /// <summary>
            /// 最初に見つかった特定の状態効果を除去する
            /// </summary>
            /// <param name="target">無効化する状態効果</param>
            private void RemoveStatusEffect(StatusEffectData target)
            {
                Debug.Log($"{Name}: [ EFFECT REMOVED ] {target.Name}");
                
                // パラメータの変更を伴う効果の場合調整パラメータを戻す
                CUtil.RemoveStatusEffect(this, target);
                
                StatusEffects.Remove(target);
            }

            /// <summary>
            /// 特定の対象のネガティブな状態効果を1つ除去する
            /// </summary>
            /// <param name="target"></param>
            public void GiveDeEffect(Unit target)
            {
                Debug.Log($"{Name}: [ REMOVE EFFECT ] -> {target.Name}");
                target.RemoveFirstNegativeStatusEffect();
            }

            /// <summary>
            /// 特定の対象を回復する
            /// </summary>
            /// <param name="target">回復する対象</param>
            /// <param name="amount">素の回復量(ステータスによる補正前)</param>
            public void GiveHeal(Unit target, float amount)
            {
                var adjustedAmount = amount;
                // 値を補正

                Debug.Log($"{Name}: [ GIVE HEAL ] amount: {amount} -> {target.Name}");
                
                target.ReceiveHeal(adjustedAmount);
            }

            /// <summary>
            /// 回復を受ける
            /// </summary>
            /// <param name="amount">素の回復量(発動者のステータス補正が乗った、被回復主体が受ける量)</param>
            public void ReceiveHeal(float amount)
            {
                var adjustedAmount = amount;
                // 値を補正
                
                var newHp = Math.Min(Hp, Hp + Mathf.RoundToInt(adjustedAmount));
                
                Debug.Log($"{Name}: [ RECEIVE HEAL ] amount: {amount}, HP REMAINING: {newHp}");

                Hp = newHp;
            }
            
            /// <summary>
            /// 特定の対象にダメージを与える
            /// </summary>
            /// <param name="target">ダメージを与える対象</param>
            /// <param name="amount">素のダメージ量(ステータスによる補正前)</param>
            /// <param name="attr">使用する技の属性(物/魔)</param>
            public void GiveDamage(Unit target, float amount, SkillAttribute attr)
            {
                // 基本ステータスによる補正
                var adjustedAmount =
                    GenericSkillAttribute is SkillAttribute.Physical ?
                        amount + PAtkModifier:
                        amount + MAtkModifier;
                
                Debug.Log($"{Name}: [ GIVE DAMAGE ] amount: {adjustedAmount} -> {target.Name}");
                
                target.ReceiveDamage(adjustedAmount, attr);
            }

            /// <summary>
            /// ダメージを受ける
            /// </summary>
            /// <param name="amount">素のダメージ量(発動者のステータス補正が乗った、被ダメージ主体が受ける量)</param>
            /// /// <param name="attr">使用された技の属性(物/魔)</param>
            public void ReceiveDamage(float amount, SkillAttribute attr)
            {
                // 基本ステータスによる補正
                var adjustedAmount =
                    attr is SkillAttribute.Physical ?
                    amount - PDef - PDefModifier:
                    amount - MDef - MDefModifier;

                // 衰弱補正
                var multiplier = 1f;
                if (WeakMultipliers.Count > 0)
                {
                    multiplier = WeakMultipliers.Aggregate((prev, cur) => prev + (cur - 1f)) + 1f;
                }

                adjustedAmount *= multiplier;

                var newHp = Math.Max(0, Hp - Mathf.RoundToInt(adjustedAmount));
                
                Debug.Log($"{Name}: [ RECEIVE DAMAGE ] amount: {amount}, HP REMAINING: {newHp}");

                Hp = newHp;
            }

            /// <summary>
            /// 特定の対象に状態効果を付与する
            /// </summary>
            /// <param name="target">除去する対象</param>
            /// <param name="ef">付与する状態効果</param>
            public void GiveEffect(Unit target, StatusEffectData ef)
            {
                Debug.Log($"{Name}: [ GIVE EFFECT ] {ef.Name} -> {target.Name}");
                target.ReceiveEffect(ef);
            }
            
            /// <summary>
            /// 特定の状態効果を受ける
            /// </summary>
            /// <param name="ef">受ける状態効果</param>
            public void ReceiveEffect(StatusEffectData ef)
            {
                Debug.Log($"{Name}: [ RECEIVE EFFECT ] {ef.Name} for {ef.Durability} turns");
                ApplyStatusEffect(ef);
            }

            /// <summary>
            /// 一般的な攻撃
            /// </summary>
            /// <param name="target">攻撃対象</param>
            public void GenericAttack(Unit target)
            {
                var anger = CUtil.HasEffectType(StatusEffects, StatusEffectType.Anger);
                
                var amount = (PAtk + PAtkModifier) * (anger ? CombatManager.AngerDamageMultiplier : 1f);
                
                GiveDamage(target, amount, GenericSkillAttribute);
            }

            protected Unit(string name, Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed)
            {
                Name = name;
                
                Affiliation = affiliation;
                MaxHp = maxHp;
                MaxMp = maxMp;

                // HP, MPは生成時にmax値を代入して初期化
                Hp = maxHp;
                Mp = maxMp;

                PAtk = pAtk;
                PAtkLabel = pAtkLabel;
                PDef = pDef;

                MAtk = mAtk;
                MAtkLabel = mAtkLabel;
                MDef = mDef;

                Speed = speed;

                StatusEffects = new List<StatusEffectData>();
                
                Actioned = false;
            }
        }
        
        /// <summary>
        /// 味方のUnit
        /// </summary>
        [Serializable]
        public class UnitAlly : Unit
        {
            // ユニットの職業
            public Job Job { get; private set; }
            // ユニットの性格
            public Personality Personality { get; private set; }
            
            // ユニットが持つ職業固有スキルのインデックス(複数の技を用意する場合に備えて)
            // public int JobSkillIndex{ get; private set; }
            public BraverSkillData JobSkill { get; private set; }

            // ユニットが持つ性格固有スキルのインデックス
            // public int PersonalitySkillIndex{ get; private set; }
            public BraverSkillData PersonalitySkill { get; private set; }

            public float FriendshipLevel { get; private set; }

            // 回復技、エフェクト技(状態異常・バフ・デバフ)、エフェクト解除技をそれぞれ持っているか
            public bool HasHeal{ get; private set; }
            public bool HasEffect{ get; private set; }
            public bool HasDeEffect{ get; private set; }

            private void RecognizeSelfSkills()
            {
                // HasHeal = HasEffect = HasDeEffect = false;
            }

            public UnitAlly(string name, Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, Job job, Personality personality, int jobSkillIndex, int personalitySkillIndex, float friendShipLevel) : base(name, affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                Job = job;
                Personality = personality;
                FriendshipLevel = friendShipLevel;

                JobSkill = UnityEngine.Resources.Load<JobSkills>("Skills/JobSkills").JobSkillArray[jobSkillIndex];
                PersonalitySkill = UnityEngine.Resources.Load<PersonalitySkills>("Skills/PersonalitySkills").PersonalitySkillArray[personalitySkillIndex];
                
                // ヒール・バフデバフ・状態異常技を持っているか先に判定しておく
                RecognizeSelfSkills();

                GenericSkillAttribute =
                    Job is 
                    Job.Gladiator or
                    Job.Hunter or
                    Job.Lancer or
                    Job.Swordsman ? SkillAttribute.Physical : SkillAttribute.Magical;
            }
        }

        /// <summary>
        /// 敵のUnit
        /// </summary>
        public class UnitEnemy : Unit
        {
            public bool IsUnderSkillCooldown { get; set; }
            
            public float SkillThreshold { get; private set; }

            public EnemySkillData[] Skills { get; private set; }
            
            public UnitEnemy(string name, Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, int[] enemySkillIdxs, SkillAttribute genericSkillAttribute, float skillThreshold) : base(name, affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                EnemySkillData[] allSkills = UnityEngine.Resources.Load<EnemySkills>("Skills/EnemySkills").EnemySkillsData;
                Skills = allSkills.Where((value, idx) => enemySkillIdxs.Contains(idx)).Select(val => val).ToArray();

                IsUnderSkillCooldown = false;

                SkillThreshold = skillThreshold;
                
                GenericSkillAttribute = genericSkillAttribute;
            }
        }        
    }
}
