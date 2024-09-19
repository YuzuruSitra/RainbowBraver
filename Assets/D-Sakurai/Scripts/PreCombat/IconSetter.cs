using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using D_Sakurai.Scripts.Utility;

namespace D_Sakurai.Scripts.PreCombat
{
    public class IconSetter : MonoBehaviour
    {
        [SerializeField] private Transform SetterParent;
        [SerializeField] private Transform ButtonParent;

        [SerializeField] private GameObject ButtonPrefab;

        [SerializeField] private InfoPanel InfoPanel;

        [SerializeField] private Camera MainCam;
        [SerializeField] private DragCameraControl CameraController;

        private (Transform, RectTransform)[] _btnData;

        public void SetIcons(DutyLoader loaderInstance)
        {
            var nSetters = SetterParent.childCount;
            _btnData = new (Transform, RectTransform)[nSetters];

            for (int i = 0; i < nSetters; i++)
            {
                var setter = SetterParent.GetChild(i);

                // btn.Item1: icon position in 3d space
                // btn.Item2: icon position in screen space

                var holder = setter.gameObject.GetComponent<IconDataHolder>();

                // UIのボタンを表示する
                var uiBtn = Instantiate(ButtonPrefab, ButtonParent);

                uiBtn.transform.position = MainCam.WorldToScreenPoint(setter.position);

                // UIのボタンのイベントを設定する
                var btnScript = uiBtn.GetComponent<DutyButton>();
                btnScript.SetEvent(loaderInstance, InfoPanel, holder.GetDutyIdx(), CameraController, setter.position);

                // プレビュー用のキューブを消す
                setter.GetChild(0).gameObject.SetActive(false);

                // UIボタンのTransformを保持
                _btnData[i] = (setter, uiBtn.GetComponent<RectTransform>());
            }
        }

        public void RepositionIcons()
        {
            foreach (var data in _btnData)
            {
                if (!data.Item2) continue;

                var outOfFrame = new Vector3(Screen.width, Screen.height, 0f);
                var screenPos = MainCam.WorldToScreenPoint(data.Item1.position);

                data.Item2.position = screenPos.z > 0 ? screenPos : outOfFrame;
            }
        }
    }
}