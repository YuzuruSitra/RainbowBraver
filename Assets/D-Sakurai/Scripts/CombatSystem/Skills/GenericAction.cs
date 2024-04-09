using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericAction{
    public static void PhysicalAttack(Unit subject, Unit target){
        Debug.Log("sub: " + subject + "\nobj: " + target);
        return;
    }
    public static void MagicalAttack(Unit subject, Unit target){
        Debug.Log("sub: " + subject + "\nobj: " + target);
        return;
    }

    public static void Heal(Unit subject, Unit target){
        Debug.Log("sub: " + subject + "\nobj: " + target);
        return;
    }

    public static void Effect(Unit subject, Unit target){
        Debug.Log("sub: " + subject + "\nobj: " + target);
        return;
    }
}
