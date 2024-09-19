using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.PreCombat
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private DutyLoader Loader;

        [SerializeField] private TextMeshProUGUI TitleObj;
        [SerializeField] private TextMeshProUGUI DescObj;

        [SerializeField] private Button CommenceBtn;
        private CommenceScript _commenceScript;
        
        private string _title;
        private string _description;
        private int _difficulty;

        private Animator _animator;
        
        // 何これ知らなかった
        private static readonly int TriggerOpen = Animator.StringToHash("Open");
        private static readonly int TriggerClose = Animator.StringToHash("Close");

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _commenceScript = CommenceBtn.gameObject.GetComponent<CommenceScript>();
            
            CommenceBtn.onClick.AddListener(() => _commenceScript.Dispatch());
        }

        public async UniTask Open(int dutyIdx)
        {
            await CloseIfNeeded();
            
            SetContent(dutyIdx);
            _animator.SetTrigger(TriggerOpen);
        }

        public async UniTask CloseIfNeeded()
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("DutyPanelIn"))
            {
                return;
            }
            
            _animator.SetTrigger(TriggerClose);
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("DutyPanelOut"));
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }

        public bool IsOpening()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName("DutyPanelIn");
        }
        
        private void SetContent(int dutyIdx)
        {
            // キャッシュしてあるから案ずるな
            var data = Loader.GetDuty(dutyIdx);

            TitleObj.text = data.Title;
            DescObj.text = data.Description;

            _commenceScript.DutyIdx = dutyIdx;
        }
    }
}