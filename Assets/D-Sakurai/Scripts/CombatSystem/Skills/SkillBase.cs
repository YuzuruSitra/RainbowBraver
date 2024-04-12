namespace D_Sakurai.Scripts.CombatSystem.SKills
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

        /// <summary>
        /// スキル1種類を定義するクラス
        /// </summary>
        public class SkillData
        {
            // スキル名
            public string Name { get; }
            // スキルの説明文
            public string Description { get; }
            // スキルに含まれる行動の配列
            public SkillProperty[] SkillProperties { get; }

            public SkillData(string name, string description, SkillProperty[] skillProperties)
            {
                Name = name;
                Description = description;
                SkillProperties = skillProperties;
            }
        }
        
        /// <summary>
        /// SkillDataに1つ以上含まれる、実際の行動1つを定義するクラス
        /// </summary>
        public class SkillProperty
        {
            // 汎用的な行動か(いらなくなりそう)
            public bool IsBasic { get; }
            
            // 行動の属性
            public SkillAttribute SkillAttribute { get; }
            // 行動の種類
            public SkillType Type { get; }

            // 行動の素の効果量
            public float Amount { get; }

            // 消費するMP
            public int CostMp { get; }
            // 消費するHP(一応)
            public int CostHp { get; }

            public SkillProperty(bool isBasic, SkillAttribute attribute, SkillType type, float amount, int costHp = 0, int costMp = 0)
            {
                IsBasic = isBasic;

                SkillAttribute = attribute;
                Type = type;

                Amount = amount;

                CostHp = costHp;
                CostMp = costMp;
            }
        }
    }
}