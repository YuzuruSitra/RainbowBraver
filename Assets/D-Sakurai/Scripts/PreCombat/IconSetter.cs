using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using D_Sakurai.Scripts.PreCombat;

public class IconSetter : MonoBehaviour
{
    [SerializeField] private Transform SetterParent;
    [SerializeField] private Transform ButtonParent;

    [SerializeField] private GameObject ButtonPrefab;

    [SerializeField] private Camera MainCam;

    public void SetIcons(DutyLoader loaderInstance)
    {
        var nSetters = SetterParent.childCount;
        var setters = new Transform[nSetters];
        for (int i = 0; i < nSetters; i++)
        {
            setters[i] = SetterParent.GetChild(i);
        }
        
        foreach (var str in setters)
        {
            var btn = Instantiate(ButtonPrefab, ButtonParent);

            btn.transform.position = MainCam.WorldToScreenPoint(str.position);
            
            str.gameObject.SetActive(false);
        }
    }
}
