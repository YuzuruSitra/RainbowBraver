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

        public Vector3 IconPosition;
        
        public Phase[] Phases;
    }

    [System.Serializable]
    public struct Phase
    {
        public int[] EnemyIds;
    }
}