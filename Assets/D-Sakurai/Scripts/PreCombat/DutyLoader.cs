using Resources.Duty;
using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyLoader : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;
        [SerializeField] private float heightScale;
        [SerializeField] private IconSetter _iconSetter;

        private Duty[] _duties;

        void Start()
        {
            var tScale = terrain.terrainData.size;
            
            _duties = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData;

            _iconSetter.SetIcons();
        }

        public DutyInfo GetDutyInfo(int idx)
        {
            var duty = _duties[idx];

            var result = new DutyInfo(duty.Title, duty.Description);
            return result;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
