using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigPanelVisibility : MonoBehaviour
{
    [SerializeField] Animator Background, Panel;

    public void Show(){
        Debug.Log("show");
        Background.SetTrigger("Show");
        Panel.SetTrigger("Show");
    }

    public void Hide(){
        Debug.Log("hide");
        Background.SetTrigger("Hide");
        Panel.SetTrigger("Hide");
    }
}
