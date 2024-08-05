using D_Sakurai.Resources.Skills.SkillBase;
using UnityEngine;

namespace D_Sakurai.Resources.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "CombatSystem/Enemies", order = 3)]
    public class Enemies : ScriptableObject
    {
        public EnemyData[] EnemiesData;
    }

    /// <summary>
    /// 敵1種類を定義する構造体
    /// </summary>
    [System.Serializable]
    public struct EnemyData
    {
        public string Name;
        
        public int MaxHp;

        public float PAtk;
        public float MAtk;
        public string GenericAttackLabel;

        public float PDef;
        public float MDef;

        public int Speed;

        // 通常攻撃の属性
        public SkillAttribute GenericSkillAttribute;

        // スキルを使用するか決定する際の閾値(0. - 1.)
        public float SkillThreshold;

        public int[] EnemySkillIds;
    }
}