using UnityEngine;

namespace D_Sakurai.Resources.Skills
{
    [CreateAssetMenu(fileName = "PersonalitySkills", menuName = "CombatSystem/PersonalitySkills", order = 0)]
    public class PersonalitySkills : ScriptableObject
    {
        public SkillBase.BraverSkillData[] PersonalitySkillArray;
        // public SkillBase.BraverSkillData[] Active;
        // public SkillBase.BraverSkillData[] Sociable;
    }
}