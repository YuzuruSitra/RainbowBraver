using System;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

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

        [SerializeField] private int FocusAnimLengthInFr;
        [SerializeField] private float IconFocusDistance;
        private bool _focusing = false;
        private Vector3 _unfocusedCamPosition;

        private void Start()
        {
            _prevMousePos = Input.mousePosition;
        }

        private void Update()
        {
            if (_focusing)
            {
                IconSetter.RepositionIcons();
                return;
            }
            
            // ����
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
            
            // �h���b�O�������
            if (Input.GetMouseButton(0))
            {
                var diff3 = mousePos - _prevMousePos;
                diff = new Vector2(diff3.x, diff3.y);
            }
            
            _prevMousePos = mousePos;

            // �h���b�O�ʂ�臒l�ȏ�ł����
            if (diff.magnitude > DragThresh)
            {
                _velocity += diff * (DragScale * -1);

                // ��������ꍇ
                if (_velocity.magnitude > MaxSpeed)
                {
                    _velocity = _velocity.normalized * MaxSpeed;
                }
            }
            
            // ���ɒx���ꍇ�͍Ĕz�u�����ɗ��E
            if (_velocity.magnitude < .001) return;
            
            // �K�p
            var cp = new Vector2(MainCam.position.x, MainCam.position.z);
            cp += _velocity;

            // �͈͊O�Ȃ�~�߂�
            var clampedX = Mathf.Clamp(cp.x, CamRangeX.x, CamRangeX.y);
            var clampedY = Mathf.Clamp(cp.y, CamRangeZ.x, CamRangeZ.y);

            MainCam.position = new Vector3(clampedX, MainCam.position.y, clampedY);
            
            IconSetter.RepositionIcons();
        }

        public void Focus(Vector3 targetPosition)
        {
            _focusing = true;

            var shiftedCamPosition = new Vector3(targetPosition.x, MainCam.position.y, MainCam.position.z);
            // 少し手前
            var actualTarget = targetPosition + (shiftedCamPosition - targetPosition).normalized * IconFocusDistance;
            
            _unfocusedCamPosition = MainCam.position;
            FocusAnim(_unfocusedCamPosition, actualTarget).Forget();
        }

        public async UniTask UnFocus()
        {
            if (!_focusing) return;
            
            await FocusAnim(MainCam.position, _unfocusedCamPosition);
            _focusing = false;
        }

        private async UniTask FocusAnim(Vector3 start, Vector3 end)
        {
            // Debug.Log("start");
            // _focusing = true;

            var animIdx = 0;
            var animProgress = 0f;

            while (animProgress <= 1f)
            {
                MainCam.position = Vector3.Lerp(start, end, EaseOutCubic(animProgress));
                animIdx++;
                animProgress = (float) animIdx / FocusAnimLengthInFr;
                
                await UniTask.Yield();
            }

            // _focusing = false;
            Debug.Log("end");
        }

        private static float EaseOutCubic(float idx)
        {
            // from https://easings.net/ja#easeOutCubic
            return 1 - Mathf.Pow(1 - idx, 3);
        }
    }
}