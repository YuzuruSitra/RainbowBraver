using System.Collections.Generic;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "PersonalitySkills", menuName = "PersonalitySkills", order = 0)]
    public class PersonalitySkills : ScriptableObject
    {
        public List<SkillBase.SkillData> active;
        public List<SkillBase.SkillData> sociable;
    }
}