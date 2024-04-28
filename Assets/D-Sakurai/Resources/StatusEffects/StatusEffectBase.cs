namespace D_Sakurai.Resources.StatusEffects
{
    namespace StatusEffectBase
    {
        public enum StatusEffectType
        {
            Mp,
            PAtk,
            PDef,
            MAtk,
            MDef,
            Speed,
            Weak,
            Skip,
            Anger,
            BrainWashed
        }

        [System.Serializable]
        public class StatusEffectData
        {
            // 状態効果を付与された主体及び主体が属する勢力にとって好ましい効果であるか
            public bool IsFriendly;
            // 状態効果名
            public string Name;
            // 状態効果の解説
            public string Description;
            // 継続ターン数
            public int Durability;
            // 経過ターン数
            public int Elapsed;
            // 状態効果に含まれる効果の配列
            public StatusEffectProperty[] Properties;
        }

        [System.Serializable]
        public struct StatusEffectProperty
        {
            // 効果の種類
            public StatusEffectType Type;
            // 必要な場合効果量
            public float Amount;
        }
    }
}