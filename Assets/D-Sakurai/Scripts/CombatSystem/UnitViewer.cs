using UnityEngine;
using UnityEngine.UI;

namespace D_Sakurai.Scripts.CombatSystem
{
    // 表示機能の実装は優先度が低いと判断したためいったん保留
    public class UnitViewer : MonoBehaviour
    {
        [SerializeField] private Text nameDisp;
        [SerializeField] private Slider hpSlider;

        private float _maxHp;
    
        public void Init(string unitName, float maxHp)
        {
            nameDisp.text = name;
            _maxHp = maxHp;
        }

        public void UpdateHp(float newHp)
        {
            hpSlider.value = newHp / _maxHp;
        }

        public void UpdateState(bool newState)
        {
            if (newState)
            {
                
            }
            else
            {
                
            }
        }
    }
}
