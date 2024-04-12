using System.Collections.Generic;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "JobSkills", menuName = "JobSkills", order = 1)]
    public class JobSkills : ScriptableObject
    {
        public List<SkillBase.SkillData> swordsman;
        public List<SkillBase.SkillData> gladiator;
        public List<SkillBase.SkillData> lancer;
        public List<SkillBase.SkillData> hunter;
        public List<SkillBase.SkillData> oracle;
        public List<SkillBase.SkillData> sorcerer;
    }
}