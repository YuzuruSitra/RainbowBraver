using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public class IconDataHolder : MonoBehaviour
    {
        [SerializeField] private int DutyIdx;

        public int GetDutyIdx() => DutyIdx;
    }
}