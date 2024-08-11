using System;
using D_Sakurai.Scripts.PreCombat;
using UnityEngine;

namespace Resources.Duty
{
    [CreateAssetMenu(fileName = "Duties", menuName = "CombatSystem/Duties", order = 2)]
    public class Duties : ScriptableObject
    {
        public Duty[] DutiesData;
    }

    [System.Serializable]
    public struct Duty
    {
        public string Title;
        public string Description;
        
        public Phase[] Phases;

        public Duty(string title, string description, Phase[] phases)
        {
            Title = title;
            Description = description;
            Phases = phases;
        }

        public static Duty Empty()
        {
            return new Duty("Empty", "Empty", Array.Empty<Phase>());
        }
    }

    [System.Serializable]
    public struct Phase
    {
        public int[] EnemyIds;

        public Phase(int[] enemyIds)
        {
            EnemyIds = enemyIds;
        }

        public static Phase Empty()
        {
            return new Phase(Array.Empty<int>());
        }
    }
}