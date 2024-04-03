using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveControl : MonoBehaviour
{
    public static void Activate(GameObject target){
        target.SetActive(true);
    }

    public static void Deactivate(GameObject target){
        target.SetActive(false);
    }
}
