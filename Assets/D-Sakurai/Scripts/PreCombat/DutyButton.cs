using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyButton : MonoBehaviour
    {
        [SerializeField] private int DutyIdx;
        private DutyLoader _loader;

        public void SetLoader(DutyLoader loaderInstance)
        {
            _loader = loaderInstance;
        }

        public void SetEvent()
        {
            var btn = GetComponent<Button>();

            btn.clicked += () =>
            {
                SceneTransitioner.TransitionToCombat(DutyIdx, _loader.GetAllies());
            };
        }
    }
}