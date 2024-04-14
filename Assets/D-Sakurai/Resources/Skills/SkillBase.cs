namespace D_Sakurai.Resources.Skills
{
    // スキルの情報を構成する基本的な要素
    namespace SkillBase
    {
        // スキルの属性
        public enum SkillAttribute
        {
            Physical,
            Magical
        }

        // スキルの種類
        public enum SkillType
        {
            Attack,
            Heal,
            Effect// 状態異常・バフ・デバフ
        };

        //  スキル1種類を定義する構造体
        // ----------------------------------
        [System.Serializable]
        public struct BraverSkillData
        {
            // スキル名
            public string Name;
            // スキルの説明文
            public string Description;
            // スキル実行時に増える友情ゲージ量
            public float SpecialGaugeIncreaseRate;
            // スキルに含まれる行動の配列
            public BraverSkillProperty[] SkillProperties;
        }
        
        //  SkillDataに1つ以上含まれる、実際の行動1つを定義する構造体
        // -------------------------------------------------------------
        [System.Serializable]
        public struct BraverSkillProperty
        {
            // 汎用的な行動か(いらなくなりそう)
            public bool IsBasic;
            
            // 行動の属性
            public SkillAttribute SkillAttribute;
            // 行動の種類
            public SkillType Type;

            // 行動の素の効果量
            public float Amount;

            // 消費するMP
            public int CostMp;
            // 消費するHP(一応)
            public int CostHp;
        }

        [System.Serializable]
        public struct EnemySkillData
        {
            // スキル名
            public string Name;
            // スキルの説明文
            public string Description;
            // スキルに含まれる行動の配列
            public EnemySkillProperty[] SkillProperties;
        }

        [System.Serializable]
        public struct EnemySkillProperty
        {
            // 行動の属性
            public SkillAttribute SkillAttribute;
            // 行動の種類
            public SkillType Type;

            // 行動の素の効果量
            public float Amount;
        }
    }
}