using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public class CommenceScript : MonoBehaviour
    {
        public int DutyIdx { get; set; }
        
        public void Dispatch(){
            DutyDispatcher.Dispatch(DutyIdx);
        }
    }
}