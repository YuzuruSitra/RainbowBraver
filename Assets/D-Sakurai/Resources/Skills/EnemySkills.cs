using UnityEngine;

namespace D_Sakurai.Resources.Skills
{
    [CreateAssetMenu(fileName = "EnemySkills", menuName = "CombatSystem/EnemySkills", order = 4)]
    public class EnemySkills : ScriptableObject
    {
        public SkillBase.EnemySkillData[] EnemySkillsData;
    }
}