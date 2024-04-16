using D_Sakurai.Resources.Skills;
using D_Sakurai.Resources.Skills.SkillBase;
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
            public int Speed{ get; }
            
            // そのターン行動したか
            public bool Actioned { get; }

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
            public int JobSkillIndex{ get; private set; }
            // public BraverSkillData JobSkill { get; private set; }

            // ユニットが持つ性格固有スキルのインデックス
            public int PersonalitySkillIndex{ get; private set; }
            // public BraverSkillData PersonalitySkill { get; private set; }

            public float FriendshipLevel;

            // 回復技、エフェクト技(状態異常・バフ・デバフ)、エフェクト解除技をそれぞれ持っているか
            public bool HasHeal{ get; private set; }
            public bool HasEffect{ get; private set; }
            public bool HasDeEffect{ get; private set; }

            private void RecognizeSelfSkills()
            {
                HasHeal = HasEffect = HasDeEffect = false;
                
                JobSkills jobSkills = UnityEngine.Resources.Load<JobSkills>("Skills/JobSkills");
                foreach (var property in jobSkills.JobSkillArray[JobSkillIndex].SkillProperties)
                {
                    switch (property.Type)
                    {
                        case SkillType.Heal: HasHeal = true;
                            break;
                        case SkillType.Effect: HasEffect = true;
                            break;
                        case SkillType.DeEffect: HasDeEffect = true;
                            break;
                    }
                }

                PersonalitySkills personalitySkills = UnityEngine.Resources.Load<PersonalitySkills>("Skills/PersonalitySkills");
                foreach (var property in personalitySkills.PersonalitySkillArray[PersonalitySkillIndex].SkillProperties)
                {
                    switch (property.Type)
                    {
                        case SkillType.Heal: HasHeal = true;
                            break;
                        case SkillType.Effect: HasEffect = true;
                            break;
                        case SkillType.DeEffect: HasDeEffect = true;
                            break;
                    }
                }
            }

            public UnitAlly(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, Job job, Personality personality, int jobSkillIndex, int personalitySkillIndex, float friendShipLevel) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                Job = job;
                Personality = personality;
                JobSkillIndex = jobSkillIndex;
                PersonalitySkillIndex = personalitySkillIndex;
                FriendshipLevel = friendShipLevel;

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
            
            public UnitEnemy(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                // Kind = kind;
            }
        }        
    }
}
