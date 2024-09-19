using System;
using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.PreCombat
{
    public class IconDataHolder : MonoBehaviour
    {
        [SerializeField] private int DutyIdx;

        public int GetDutyIdx() => DutyIdx;

        private Button _btn;

        private void Start()
        {
            _btn = GetComponent<Button>();
        }
    }
}