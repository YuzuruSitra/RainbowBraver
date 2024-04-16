using UnityEngine;

namespace D_Sakurai.Resources.Skills
{
    [CreateAssetMenu(fileName = "JobSkills", menuName = "CombatSystem/JobSkills", order = 1)]
    public class JobSkills : ScriptableObject
    {
        // 結局IDで機械的に管理するのでここっちの方が良い気がしてきた。
        // 結局戻すような気がしなくもないので一応コメントアウトしておく(消し忘れそ～)
        public SkillBase.BraverSkillData[] JobSkillArray;
        // public SkillBase.BraverSkillData[] Swordsman;
        // public SkillBase.BraverSkillData[] Gladiator;
        // public SkillBase.BraverSkillData[] Lancer;
        // public SkillBase.BraverSkillData[] Hunter;
        // public SkillBase.BraverSkillData[] Oracle;
        // public SkillBase.BraverSkillData[] Sorcerer;
    }
}