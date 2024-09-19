using D_Sakurai.Scripts.CombatSystem.Units;
using Resources.Duty;
using UnityEngine;
using UnityEngine.Serialization;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyLoader : MonoBehaviour
    {
        [SerializeField] private Terrain Terrain;
        [SerializeField] private IconSetter IconSetter;
        
        [SerializeField] private UnitAlly[] Allies;

        public UnitAlly[] GetAllies()
        {
            return Allies;
        }

        private Duty[] _duties;

        void Start()
        {
            var tSize = Terrain.terrainData.size;
            
            _duties = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData;

            IconSetter.SetIcons(this);
        }

        public Duty GetDuty(int idx)
        {
            if (idx < 0 || idx >= _duties.Length )
            {
                Debug.LogError($"Given duty index {idx} is out of range. (_duties.Length: {_duties.Length})");
                return Duty.Empty();
            }
            
            var result = _duties[idx];
            return result;
        }
    }
}
