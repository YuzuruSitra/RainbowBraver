using System;
using Cysharp.Threading.Tasks;
using D_Sakurai.Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace D_Sakurai.Scripts.PreCombat
{
    public class PanelCloseCollider : MonoBehaviour
    {
        [SerializeField] private InfoPanel PanelCtrl;
        [SerializeField] private DragCameraControl CameraController;
        
        public void TryClose()
        {
            CameraController.UnFocus();
            PanelCtrl.CloseIfNeeded().Forget();
        }
    }
}