using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        // TODO: 戦闘を実際に進行するマネージャー
        void PhysicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
        void MagicalAttack(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Heal(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }

        void Effect(Units.Unit subject, Units.Unit target){
            Debug.Log("sub: " + subject + "\nobj: " + target);
            return;
        }
    }
}