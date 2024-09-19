namespace D_Sakurai.Resources.StatusEffects
{
    namespace StatusEffectBase
    {
        /// <summary>
        /// 状態効果の種類を表すEnum
        /// </summary>
        
        public enum StatusEffectType
        {
            Mp,
            PAtk,
            PDef,
            MAtk,
            MDef,
            Speed,
            Miasma,
            Weak,
            Sleep,
            Anger,
            BrainWash
        }

        /// <summary>
        /// 状態効果1種類を定義するクラス
        /// </summary>
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

        /// <summary>
        /// 状態効果1種類に含まれる具体的な効果1種類を定義する構造体
        /// </summary>
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