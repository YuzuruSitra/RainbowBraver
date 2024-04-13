using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "CombatSystem/Enemies", order = 3)]
    public class Enemies : ScriptableObject
    {
        public EnemyData[] EnemiesData;
    }

    [System.Serializable]
    public struct EnemyData
    {
        public int MaxHp;

        public float PAtk;
        public float MAtk;
        public string GenericAttackLabel;

        public int[] EnemySkillIds;
    }
}