using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem.Skills
{
    // スキルの情報を構成する基本的な要素
    namespace SkillBase
    {
        // スキルの属性
        public enum SkillAttribute
        {
            Physical,
            Magical
        }

        // スキルの種類
        public enum SkillType
        {
            Attack,
            Heal,
            Effect// 状態異常・バフ・デバフ
        };

        //  スキル1種類を定義する構造体
        // ----------------------------------
        [System.Serializable]
        public struct SkillData
        {
            // スキル名
            public string name;
            // スキルの説明文
            public string description;
            // スキルに含まれる行動の配列
            public SkillProperty[] skillProperties;
        }
        
        //  SkillDataに1つ以上含まれる、実際の行動1つを定義する構造体
        // -------------------------------------------------------------
        [System.Serializable]
        public struct SkillProperty
        {
            // 汎用的な行動か(いらなくなりそう)
            public bool isBasic;
            
            // 行動の属性
            public SkillAttribute skillAttribute;
            // 行動の種類
            public SkillType type;

            // 行動の素の効果量
            public float amount;

            // 消費するMP
            public int costMp;
            // 消費するHP(一応)
            public int costHp;
        }
    }
}