using System;
using Unity.Mathematics;
using UnityEngine;

namespace D_Sakurai.Scripts.Utility
{
    public class DragCameraControl : MonoBehaviour
    {
        [SerializeField] private Transform MainCam;

        [SerializeField] private PreCombat.IconSetter IconSetter;
        
        [SerializeField] private float DragThresh;
        [SerializeField] private float MaxSpeed;
        [SerializeField] private float DragScale;
        [SerializeField] private float FrictionRate;

        [SerializeField] private int2 CamRangeX;
        [SerializeField] private int2 CamRangeZ;

        private Vector3 _prevMousePos;

        private Vector2 _velocity;

        private void Start()
        {
            _prevMousePos = Input.mousePosition;
        }

        private void Update()
        {
            // 減速
            if (_velocity.magnitude > .001)
            {
                _velocity *= FrictionRate;
            }
            else
            {
                _velocity = Vector2.zero;
            }

            
            var diff = Vector2.zero;
            
            var mousePos = Input.mousePosition;
            
            // ドラッグがあれば
            if (Input.GetMouseButton(0))
            {
                var diff3 = mousePos - _prevMousePos;
                diff = new Vector2(diff3.x, diff3.y);
            }
            
            _prevMousePos = mousePos;

            // ドラッグ量が閾値以上であれば
            if (diff.magnitude > DragThresh)
            {
                _velocity += diff * (DragScale * -1);

                // 速すぎる場合
                if (_velocity.magnitude > MaxSpeed)
                {
                    _velocity = _velocity.normalized * MaxSpeed;
                }
            }
            
            // 非常に遅い場合は再配置せずに離脱
            if (_velocity.magnitude < .001) return;
            
            // 適用
            var cp = new Vector2(MainCam.position.x, MainCam.position.z);
            cp += _velocity;

            // 範囲外なら止める
            var clampedX = Mathf.Clamp(cp.x, CamRangeX.x, CamRangeX.y);
            var clampedY = Mathf.Clamp(cp.y, CamRangeZ.x, CamRangeZ.y);

            MainCam.position = new Vector3(clampedX, MainCam.position.y, clampedY);
            
            IconSetter.RepositionIcons();
        }
    }
}