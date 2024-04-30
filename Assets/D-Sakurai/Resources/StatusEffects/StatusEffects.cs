using UnityEngine;

namespace D_Sakurai.Resources.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffects", menuName = "CombatSystem/StatusEffects", order = 5)]
    public class StatusEffects : ScriptableObject
    {
        public StatusEffectBase.StatusEffectData[] StatusEffectsData;
    }
}