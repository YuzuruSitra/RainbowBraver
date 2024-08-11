using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyButton : MonoBehaviour
    {
        private DutyLoader _loader;

        public void SetLoader(DutyLoader loaderInstance)
        {
            _loader = loaderInstance;
        }

        public void SetEvent(int dutyIdx)
        {
            var btn = GetComponent<Button>();
            
            btn.onClick.AddListener(() =>
            {
                Debug.Log($"Call Scenetransitioner.TransitionToCombat().\nDuty idx: {dutyIdx}, allies: {_loader.GetAllies()}");
                SceneTransitioner.TransitionToCombat(dutyIdx, _loader.GetAllies());
            });
        }
    }
}