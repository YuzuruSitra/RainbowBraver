using UnityEngine;

namespace D_Sakurai.Resources.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffects", menuName = "CombatSystem/PersonalitySkills", order = 5)]
    public class StatusEffects : ScriptableObject
    {
        public StatusEffectBase.StatusEffectData[] StatusEffectsData;
    }
}