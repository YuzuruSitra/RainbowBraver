using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using D_Sakurai.Scripts.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyButton : MonoBehaviour
    {
        private DutyLoader _loader;
        private Vector3 _3dPosition;

        private DragCameraControl _cameraController;

        public void SetEvent(DutyLoader loader, InfoPanel panel, int dutyIdx, DragCameraControl cameraController, Vector3 threeDimensionalPosition)
        {
            _loader = loader;

            _cameraController = cameraController;
            _3dPosition = threeDimensionalPosition;
            
            var btn = GetComponent<Button>();
            
            btn.onClick.AddListener(() =>
            {
                cameraController.Focus(_3dPosition);
                
                panel.Open(dutyIdx).Forget();
                
                Debug.Log($"Call Scenetransitioner.TransitionToCombat().\nDuty idx: {dutyIdx}, allies: {_loader.GetAllies()}");
                SceneTransitioner.TransitionToCombat(dutyIdx, _loader.GetAllies());
            });
        }
    }
}