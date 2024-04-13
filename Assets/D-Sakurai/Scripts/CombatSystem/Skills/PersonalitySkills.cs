using System.Collections.Generic;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "PersonalitySkills", menuName = "CombatSystem/PersonalitySkills", order = 0)]
    public class PersonalitySkills : ScriptableObject
    {
        public SkillBase.BraverSkillData[] Active;
        public SkillBase.BraverSkillData[] Sociable;
    }
}