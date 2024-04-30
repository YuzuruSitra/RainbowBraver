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

        public int[] EnemySkillIds;
    }
}