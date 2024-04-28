using System.Collections.Generic;
using System.Linq;
using D_Sakurai.Resources.Skills;
using D_Sakurai.Resources.Skills.SkillBase;
using D_Sakurai.Resources.StatusEffects.StatusEffectBase;
using UnityEngine;

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

        public struct UnitStatusEffectData
        {
            public float _hp { get; }
            public float _mp{ get; }

            public float _pAtk{ get; }
            public float _pDef{ get; }

            public float _mAtk{ get; }
            public float _mDef{ get; }

            public float _speed{ get; }

            public UnitStatusEffectData(float value = 0)
            {
                _hp = value;
                _mp = value;

                _pAtk = value;
                _pDef = value;

                _mAtk = value;
                _mDef = value;

                _speed = value;
            }
        }

        /// <summary>
        /// ユニットのインターフェイス
        /// </summary>
        interface IUnitData
        {
            Affiliation Affiliation { get; }

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

            // UnitStatusEffectData StatusEffects { get; }
        }

        /// <summary>
        /// ユニットの基底クラス
        /// ユニットは、敵味方の別なく戦闘に参加する1主体を指す単位
        /// </summary>
        public class Unit : IUnitData
        {
            // 所属
            public Affiliation Affiliation{ get; }

            // 最大HP
            public int MaxHp{ get; }
            // 最大MP
            public int MaxMp{ get; }
            
            // HP
            // 戦闘中実際に変動するのはこちらの値
            public int Hp{ get; private set; }
            // MP
            // 戦闘中実際に変動するのはこちらの値
            public int Mp{ get; private set; }

            // 物理攻撃力
            public float PAtk{ get; }
            // 通常物理攻撃用の技名
            public string PAtkLabel{ get; }
            // 物理防御力
            public float PDef{ get; }

            // 魔法攻撃力
            public float MAtk{ get; }
            // 通常魔法攻撃用の技名
            public string MAtkLabel{ get; }
            // 魔法防御力
            public float MDef{ get; }

            // 素早さ
            public int Speed { get; }

            // そのターン行動したか
            public bool Actioned { get;  set; }

            public List<StatusEffectData> StatusEffects { get; set; }

            // public UnitStatusEffectData StatusEffects { get; }

            private GameObject GameObject { get; set; }
            private Animator Animator { get; set; }

            // このUnitとして扱うGameObjectを取得する
            public void AssignGameObject(GameObject obj)
            {
                GameObject = obj;
                AssignAnimator(obj);
            }
            
            // このUnitとして扱うGameObjectのAnimatorを取得する
            private void AssignAnimator(GameObject obj)
            {
                Animator = obj.GetComponent<Animator>();
            }
            
            // Animatorの特定のステートを呼び出す(Async/Awaitを利用してステートを抜けた際に戻すように書く予定)
            public void CallAnimState(string trigger)
            {
                Animator.SetTrigger(trigger);
            }

            /// <summary>
            /// 状態効果を付与する
            /// </summary>
            /// <param name="ef">付与する状態効果</param>
            public void ApplyStatusEffect(StatusEffectData ef)
            {
                ef.Elapsed = 0;
                StatusEffects.Add(ef);
            }

            public void UpdateStatusEffects()
            {
                foreach (var ef in StatusEffects)
                {
                    ef.Elapsed++;
                }

                StatusEffectData[] removals = StatusEffects.FindAll(ef => ef.Elapsed >= ef.Durability).ToArray();
                foreach (var ef in removals)
                {
                    RemoveStatusEffect(ef);
                }
            }

            /// <summary>
            /// リストの最も上にあるネガティブな状態効果を無効化する
            /// </summary>
            private void RemoveFirstNegativeStatusEffect()
            {
                var target = StatusEffects.Find(ef => !ef.IsFriendly);
                
                RemoveStatusEffect(target);

                // nullりそう　参照が入ってそうだから
                Debug.Log(target);
            }

            private void RemoveStatusEffect(StatusEffectData target)
            {
                StatusEffects.Remove(target);
            }

            public void GiveDeEffect(Unit target)
            {
                target.RemoveFirstNegativeStatusEffect();
            }

            public void GiveHeal(Unit target, float amount)
            {
                var adjustedAmount = amount;
                // 値を補正
                
                target.ReceiveHeal(adjustedAmount);
            }

            public void ReceiveHeal(float amount)
            {
                var adjustedAmount = amount;
                // 値を補正

                Hp += Mathf.RoundToInt(adjustedAmount);
            }
            
            public void GiveDamage(Unit target, float amount)
            {
                var adjustedAmount = amount;
                // 値を補正
                
                target.ReceiveDamage(adjustedAmount);
            }

            public void ReceiveDamage(float amount)
            {
                var adjustedAmount = amount;
                // 値を補正

                Hp -= Mathf.RoundToInt(adjustedAmount);
            }

            public void GiveEffect(Unit target, StatusEffectData ef)
            {
                target.ReceiveEffect(ef);
            }

            public void ReceiveEffect(StatusEffectData ef)
            {
                StatusEffects.Add(ef);
            }

            public void GenericAttack(Unit target)
            {
                // 値を算出
            }

            protected Unit(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed){
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

            public float FriendshipLevel;

            // 回復技、エフェクト技(状態異常・バフ・デバフ)、エフェクト解除技をそれぞれ持っているか
            public bool HasHeal{ get; private set; }
            public bool HasEffect{ get; private set; }
            public bool HasDeEffect{ get; private set; }

            private void RecognizeSelfSkills()
            {
                // HasHeal = HasEffect = HasDeEffect = false;
            }

            public UnitAlly(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, Job job, Personality personality, int jobSkillIndex, int personalitySkillIndex, float friendShipLevel) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                Job = job;
                Personality = personality;
                FriendshipLevel = friendShipLevel;

                JobSkill = UnityEngine.Resources.Load<JobSkills>("Skills/JobSkills").JobSkillArray[jobSkillIndex];
                PersonalitySkill = UnityEngine.Resources.Load<PersonalitySkills>("Skills/PersonalitySkills").PersonalitySkillArray[personalitySkillIndex];
                
                // ヒール・バフデバフ・状態異常技を持っているか先に判定しておく
                RecognizeSelfSkills();
            }
        }

        /// <summary>
        /// 敵のUnit
        /// </summary>
        public class UnitEnemy : Unit{
            // 敵ユニットの種類
            // ScriptableObjectの情報を元に生成する方針に変更したため不要
            // public int Kind { get; private set; }

            public EnemySkillData[] Skills { get; private set; }
            
            public UnitEnemy(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, int[] enemySkillIdxs) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                EnemySkillData[] allSkills = UnityEngine.Resources.Load<EnemySkills>("Skills/EnemySkills").EnemySkillsData;
                Skills = allSkills.Where((value, idx) => enemySkillIdxs.Contains(idx)).Select(val => val).ToArray();
            }
        }        
    }
}
