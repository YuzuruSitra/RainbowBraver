using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyIconHandler : MonoBehaviour
    {
        [SerializeField] private int DutyId;
        
        private Button _btn;

        private void Start()
        {
            _btn = GetComponent<Button>();
            _btn.clicked += () =>
            {
                SceneTransitioner.Transition("CombatSystem");
            };
        }
    }
}