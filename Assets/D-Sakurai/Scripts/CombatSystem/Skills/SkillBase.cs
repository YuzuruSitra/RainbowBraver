using System.Collections;

namespace D_Sakurai.Scripts.CombatSystem.SKills
{
    namespace SkillBase
    {
        public enum SkillAttribute
        {
            Physical,
            Magical
        }

        public enum SkillType
        {
            Attack,
            Heal,
            Effect
        };

        public class SKillData
        {
            public string Name { get; }
            public SkillProperty[] SkillProperties { get; }

            public SKillData(string name, SkillProperty[] skillProperties)
            {
                Name = name;
                SkillProperties = skillProperties;
            }
        }
        
        public class SkillProperty
        {
            public bool IsBasic { get; }
            
            public SkillAttribute SkillAttribute { get; }
            public SkillType Type { get; }

            public float Amount { get; }

            public int CostMp { get; }
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