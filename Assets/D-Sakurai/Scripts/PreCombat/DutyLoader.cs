using Resources.Duty;
using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public class DutyLoader : MonoBehaviour
    {
        [SerializeField] private GameObject iconParent;
        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private Terrain terrain;
        [SerializeField] private float heightScale;

        private Duty[] _duties;

        void Start()
        {
            var tScale = terrain.terrainData.size;
            
            _duties = UnityEngine.Resources.Load<Duties>("Duty/Duties").DutiesData;

            foreach (var d in _duties)
            {
                var icon = Instantiate(iconPrefab, iconParent.transform, true);

                var ip = d.IconPosition;
                var wPos = new Vector3(ip.x * tScale.x, ip.y * heightScale, ip.z * tScale.z);

                icon.transform.position = wPos;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
