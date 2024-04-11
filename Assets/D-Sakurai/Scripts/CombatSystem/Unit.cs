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
        }

        /// <summary>
        /// ユニットの基底クラス
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
            }
        }

        /// <summary>
        /// 味方のUnit
        /// </summary>
        public class UnitAlly : Unit
        {
            // ユニットの職業
            public Job Job { get; private set; }
            
            // ユニットが持つ職業固有スキルのインデックス(複数の技を用意する場合に備えて)
            public int JobSkillIndex{ get; private set; }
            // ユニットが持つ性格固有スキルのインデックス
            public int PersonalitySkillIndex{ get; private set; }

            public UnitAlly(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, Job job, int jobSkillIndex, int personalitySkillIndex) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                Job = job;
                JobSkillIndex = jobSkillIndex;
                PersonalitySkillIndex = personalitySkillIndex;
            }
        }

        /// <summary>
        /// 敵のUnit
        /// </summary>
        public class UnitEnemy : Unit{
            // 敵ユニットの種類
            public int Kind { get; private set; }
            
            public UnitEnemy(Affiliation affiliation, int maxHp, int maxMp, float pAtk, string pAtkLabel, float pDef, float mAtk, string mAtkLabel, float mDef, int speed, int kind) : base(affiliation, maxHp, maxMp, pAtk, pAtkLabel, pDef, mAtk, mAtkLabel, mDef, speed)
            {
                Kind = kind;
            }
        }        
    }
}
