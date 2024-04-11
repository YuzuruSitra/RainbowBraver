using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    public static class GenericAction{
        public static void PhysicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
        public static void MagicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        public static void Heal(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        public static void Effect(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
    }
}
