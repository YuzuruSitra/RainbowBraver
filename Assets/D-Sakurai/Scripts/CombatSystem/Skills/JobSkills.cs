using D_Sakurai.Scripts.CombatSystem.SKills.SkillBase;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    namespace JobSkills
    {
        public static class Swordsman
        {
            public static SKillData[] Skills = 
            {
                new SKillData(
                    "hoge",
                    new SkillProperty[]
                    {
                        new (true, SkillAttribute.Physical, SkillType.Attack, 20),
                        new (true, SkillAttribute.Magical, SkillType.Attack, 10),
                    }
                ),
                new SKillData(
                    "fuga",
                    new SkillProperty[]
                    {
                        new (true, SkillAttribute.Magical, SkillType.Heal, 30),
                    }
                )
            };
        }
    }
}