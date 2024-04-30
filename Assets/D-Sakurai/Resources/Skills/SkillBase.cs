using System.Linq;
using D_Sakurai.Resources.StatusEffects.StatusEffectBase;
using UnityEngine.Serialization;

namespace D_Sakurai.Resources.Skills
{
    // スキルの情報を構成する基本的な要素
    namespace SkillBase
    {
        /// <summary>
        /// スキルの属性
        /// </summary>
        public enum SkillAttribute
        {
            Physical,
            Magical
        }

        /// <summary>
        /// スキルの種類
        /// </summary>
        public enum SkillType
        {
            Attack,
            Heal,
            Effect, // 状態異常・バフ・デバフ
            DeEffect // 状態異常・デバフの解除
        };

        /// <summary>
        /// スキル1種類を定義するクラス
        /// </summary>
        [System.Serializable]
        public class BraverSkillData
        {
            // スキルがヒール/状態効果付与/状態効果解除機能を持つか 自動生成されるため記入不要
            public bool IsHealSkill { get; private set; }
            public bool IsEffectSkill { get; private set; }
            public bool IsDeEffectSkill { get; private set; }
            public bool IsAttackSkill { get; private set; }

            // スキル名
            public string Name;

            // スキルの説明文
            public string Description;

            // スキル実行時に増える友情ゲージ量
            public float SpecialGaugeIncreaseRate;

            // 消費するMP
            public int CostMp;

            // 消費するHP(一応)
            public int CostHp;

            // スキルに含まれる行動の配列
            public BraverSkillProperty[] SkillProperties;

            public void OnEnable()
            {
                try
                {
                    foreach (var property in SkillProperties)
                    {
                        switch (property.Type)
                        {
                            case SkillType.Heal:
                                IsHealSkill = true;
                                break;
                            case SkillType.Effect:
                                IsEffectSkill = true;
                                break;
                            case SkillType.DeEffect:
                                IsDeEffectSkill = true;
                                break;
                            case SkillType.Attack:
                                IsAttackSkill = true;
                                break;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogError("Parse of BraverSKill failed: " + e);
                    throw;
                }
            }
        }

        /// <summary>
        /// SkillDataに1つ以上含まれる、実際の行動1つを定義するクラス
        /// </summary>
        [System.Serializable]
        public class BraverSkillProperty
        {
            // 汎用的な行動か(いらなくなりそう)
            public bool IsBasic;

            // 行動が行動主体が属する勢力にとって望ましいものであるか
            public bool IsFriendly;

            // 行動の属性
            public SkillAttribute SkillAttribute;

            // 行動の種類
            public SkillType Type;

            // 行動の素の効果量
            public float Amount;

            public int StatusEffectIndex;

            public StatusEffectData StatusEffect { get; private set; }

            public void OnEnable()
            {
                StatusEffect = UnityEngine.Resources.Load<StatusEffects.StatusEffects>("StatusEffects/StatusEffects").StatusEffectsData[StatusEffectIndex];
            }
        }

        /// <summary>
        /// 敵が使用するスキル1種類を定義するクラス
        /// </summary>
        [System.Serializable]
        public class EnemySkillData
        {
            // スキル名
            public string Name;

            // スキルの説明文
            public string Description;

            // スキルに含まれる行動の配列
            public EnemySkillProperty[] SkillProperties;
        }

        /// <summary>
        /// EnemySkillDataに1種類以上含まれる、実際の行動1つを定義する構造体
        /// </summary>
        [System.Serializable]
        public class EnemySkillProperty
        {
            // 行動の属性
            public SkillAttribute SkillAttribute;

            // 行動の種類
            public SkillType Type;

            // 行動の素の効果量
            public float Amount;

            public StatusEffectData StatusEffect;
        }
    }
}