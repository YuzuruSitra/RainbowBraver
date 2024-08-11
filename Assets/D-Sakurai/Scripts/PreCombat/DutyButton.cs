using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyButton : MonoBehaviour
    {
        private DutyLoader _loader;

        public void SetEvent(DutyLoader loader, InfoPanel panel, int dutyIdx)
        {
            _loader = loader;
            
            var btn = GetComponent<Button>();
            
            btn.onClick.AddListener(async () =>
            {
                Debug.Log("close");
                await panel.Close();
                Debug.Log("open");
                panel.Open(dutyIdx);
                
                Debug.Log($"Call Scenetransitioner.TransitionToCombat().\nDuty idx: {dutyIdx}, allies: {_loader.GetAllies()}");
                SceneTransitioner.TransitionToCombat(dutyIdx, _loader.GetAllies());
            });
        }
    }
}