using UnityEngine;

namespace D_Sakurai.Resources.Skills
{
    [CreateAssetMenu(fileName = "JobSkills", menuName = "CombatSystem/JobSkills", order = 1)]
    public class JobSkills : ScriptableObject
    {
        public SkillBase.BraverSkillData[] Swordsman;
        public SkillBase.BraverSkillData[] Gladiator;
        public SkillBase.BraverSkillData[] Lancer;
        public SkillBase.BraverSkillData[] Hunter;
        public SkillBase.BraverSkillData[] Oracle;
        public SkillBase.BraverSkillData[] Sorcerer;
    }
}