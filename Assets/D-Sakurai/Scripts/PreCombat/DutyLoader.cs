using D_Sakurai.Scripts.CombatSystem.Units;
using Resources.Duty;
using UnityEngine;
using UnityEngine.Serialization;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyLoader : MonoBehaviour
    {
        [SerializeField] private Terrain Terrain;
        // [SerializeField] private float HeightScale;
        [SerializeField] private IconSetter IconSetter;

        [SerializeField] private UnitAlly[] Allies;

        public UnitAlly[] GetAllies()
        {
            return Allies;
        }

        private Duty[] _duties;

        void Start()
        {
            var tScale = Terrain.terrainData.size;
            
            _duties = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData;

            IconSetter.SetIcons(this);
        }

        public DutyInfo GetDutyInfo(int idx)
        {
            var duty = _duties[idx];

            var result = new DutyInfo(duty.Title, duty.Description);
            return result;
        }
    }
}
