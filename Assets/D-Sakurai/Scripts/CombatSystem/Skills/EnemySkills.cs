using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "EnemySkills", menuName = "CombatSystem/EnemySkills", order = 4)]
    public class EnemySkills : ScriptableObject
    {
        public SkillBase.EnemySkillData[] EnemySkillsData;
    }
}