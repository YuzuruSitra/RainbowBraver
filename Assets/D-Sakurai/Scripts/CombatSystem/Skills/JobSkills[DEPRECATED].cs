using D_Sakurai.Scripts.CombatSystem.SKills.SkillBase;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    // 職業固有スキル
    namespace JobSkills
    {
        /// <summary>
        /// 戦士の職業固有スキル
        /// </summary>
        public static class Swordsman
        {
            public static SkillData[] Skills = 
            {
                new SkillData(
                    "hoge",
                    "test skill 0",
                    new SkillProperty[]
                    {
                        new (true, SkillAttribute.Physical, SkillType.Attack, 20),
                        new (true, SkillAttribute.Magical, SkillType.Attack, 10),
                    }
                ),
                new SkillData(
                    "fuga",
                    "test skill 1",
                    new SkillProperty[]
                    {
                        new (true, SkillAttribute.Magical, SkillType.Heal, 30),
                    }
                )
            };
        }
    }
}