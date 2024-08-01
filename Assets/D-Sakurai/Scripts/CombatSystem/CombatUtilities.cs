using System;
using System.Collections.Generic;
using System.Linq;
using D_Sakurai.Resources.Skills.SkillBase;
using D_Sakurai.Resources.StatusEffects.StatusEffectBase;
using Vector4 = UnityEngine.Vector4;
using Random = UnityEngine.Random;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem
{
    /// <summary>
    /// CombatManagerが使用するユーティリティ群
    /// </summary>
    public static class CombatUtilities
    {
        /// <summary>
        /// どちらかの勢力が全滅しているか確認する
        /// </summary>
        /// <param name="allies">味方のUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        /// <returns>(全滅しているか(bool), 全滅した勢力(Unit.Affiliation))</returns>
        public static (bool, Affiliation) CheckAnnihilation(UnitAlly[] allies, UnitEnemy[] enemies)
        {
            var alliesAnnihilated = allies.All(unit => unit.Hp <= 0);
            if (alliesAnnihilated) return (true, Affiliation.Player);

            var enemiesAnnihilated = enemies.All(unit => unit.Hp <= 0);
            if (enemiesAnnihilated) return (true, Affiliation.Enemy);

            // Return false(don't annihilated)
            return (false, Affiliation.Player);
        }

        /// <summary>
        /// 次に行動するUnitを取得する
        /// </summary>
        /// <param name="allUnits">全Unitを格納した配列</param>
        /// <returns>次に行動するUnit</returns>
        public static Unit GetNextUnit(Unit[] allUnits)
        {
            // unit can perform action
            Unit[] actionables = allUnits.Where(unt => !unt.IsDead && !unt.Actioned).ToArray();
            
            // check if all units are actioned unit or not
            if (actionables.Length == 0)
            {
                foreach (var unit in allUnits)
                {
                    unit.Actioned = false;
                }
                
                actionables = allUnits;
            }

            Unit result = actionables[0];
            float currentFastest = result.Speed;

            foreach (var unit in actionables)
            {
                // If unit is not yet actioned, is fastest, and is alive
                if (
                    !unit.Actioned &&
                    unit.Speed > currentFastest &&
                    !unit.IsDead
                    )
                {
                    result = unit;
                }
            }

            result.Actioned = true;
            return result;
        }

        /// <summary>
        /// 味方の行動を決定する
        /// </summary>
        /// <param name="subject">行動の主体</param>
        /// <param name="allUnits">全Unitを格納した配列</param>
        /// <param name="allies">味方のUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        /// <param name="decisionThresh">判断に用いる閾値を格納したvec4</param>
        public static void EvalAlly(UnitAlly subject, Unit[] allUnits, UnitAlly[] allies, UnitEnemy[] enemies, Vector4 decisionThresh)
        {
            // 生存している味方 / 敵
            var aliveAllies = allies.Where(e => !e.IsDead).ToArray();
            var aliveEnemies = enemies.Where(e => !e.IsDead).ToArray();
            
            // 洗脳
            var brainWashed = HasEffectType(subject.StatusEffects, StatusEffectType.BrainWash);
            
            // 怒り
            if (HasEffectType(subject.StatusEffects, StatusEffectType.Anger))
            {
                UnitEnemy target = aliveEnemies[0];

                foreach (var en in aliveEnemies)
                {
                    if (en.Hp < target.Hp)
                    {
                        target = en;
                    }
                }
                
                subject.GenericAttack(target);

                return;
            }
            
            // Get ally unit that has negative effect
            var deEffectables = Array.FindAll(allies,
                ally => ally.StatusEffects.FindAll(status => !status.IsFriendly).Count > 0);

            // Array of unitAlly that can be healed
            var healables = Array.FindAll(allies, ally => (float) ally.Hp / ally.MaxHp < decisionThresh.y);

            // ここヤバい。なんとかしたいけど思いつかない。
            var enoughMpForJobSkill = subject.Mp >= subject.JobSkill.CostMp;
            var enoughMpForPersonalitySkill = subject.Mp >= subject.PersonalitySkill.CostMp;

            var hasUsableDeEffect = (subject.JobSkill.IsDeEffectSkill && enoughMpForJobSkill) ||
                                    (subject.PersonalitySkill.IsDeEffectSkill && enoughMpForPersonalitySkill);

            var hasUsableHeal = (subject.JobSkill.IsHealSkill && enoughMpForJobSkill) ||
                                (subject.PersonalitySkill.IsHealSkill && enoughMpForPersonalitySkill);

            var hasUsableEffect = (subject.JobSkill.IsEffectSkill && enoughMpForJobSkill) ||
                                  (subject.PersonalitySkill.IsEffectSkill && enoughMpForPersonalitySkill);

            var hasUsableAttack = (subject.JobSkill.IsAttackSkill && enoughMpForJobSkill) ||
                                  (subject.PersonalitySkill.IsAttackSkill && enoughMpForPersonalitySkill);
            
            // DEEFFECT (remove debuff)
            // is not brainwashed &&
            // party has ally with bad effect &&
            // has usable deEffect skill &&
            // random value is less than decision threshold
            if (!brainWashed && deEffectables.Length > 0 && hasUsableDeEffect && Random.value < decisionThresh.x)
            {
                CallBraverSkill(subject,
                    aliveEnemies[0],
                    deEffectables[0],
                    enoughMpForJobSkill ? subject.JobSkill : subject.PersonalitySkill
                );
            }
            // JOB / PERSONALITY SKILL (HEAL)
            // is not brainwashed &&
            // party has ally can receive heal &&
            // has usable heal skill
            else if (!brainWashed && healables.Length > 0 && hasUsableHeal)
            {
                Unit healTarget = healables[0];

                // if there's healables more than 1 and latter one has less hp than former one 
                if (healables.Length > 1)
                {
                    if (healables[1].Hp < healables[0].Hp)
                    {
                        healTarget = healables[1];
                    }
                }

                CallBraverSkill(subject,
                    enemies[0],
                    healTarget,
                    enoughMpForJobSkill ? subject.JobSkill : subject.PersonalitySkill
                );
            }
            // JOB / PERSONALITY SKILL (EFFECT)
            // is not brainwashed &&
            // has usable effect skill &&
            // random value is less than decision threshold 
            else if (!brainWashed && hasUsableEffect && Random.value < decisionThresh.z)
            {
                CallBraverSkill(subject,
                    enemies[0],
                    allies[0],
                    enoughMpForJobSkill ? subject.JobSkill : subject.PersonalitySkill
                );
            }
            // JOB / PERSONALITY SKILL (ATTACK)
            // has usable attack skill &&
            // random value is less than decision threshold
            else if (hasUsableAttack && Random.value > decisionThresh.w)
            {
                Unit attackTarget = aliveEnemies[0];

                // 洗脳
                if (brainWashed)
                {
                    attackTarget = aliveAllies[Random.Range(0, aliveAllies.Length - 1)];
                }
                else
                {
                    // get enemy with the lowest hp
                    foreach (var en in aliveEnemies)
                    {
                        if (en.Hp > attackTarget.Hp) continue;
                        
                        attackTarget = en;
                    }
                }

                BraverSkillData useSkill;

                if (subject.JobSkill.IsAttackSkill && subject.PersonalitySkill.IsAttackSkill)
                {
                    useSkill = Random.value > .5 ? subject.JobSkill : subject.PersonalitySkill;
                }
                else
                {
                    useSkill = subject.JobSkill.IsAttackSkill ? subject.JobSkill : subject.PersonalitySkill;
                }
                
                CallBraverSkill(subject,
                    attackTarget,
                    subject,
                    useSkill
                    );
            }
            // GENERIC ATTACK
            else
            {
                Unit target = aliveEnemies[0];

                // 洗脳
                if (brainWashed)
                {
                    target = aliveAllies[Random.Range(0, aliveAllies.Length - 1)];
                }
                else
                {
                    // get enemy with the lowest hp
                    foreach (var en in aliveEnemies)
                    {
                        if (en.Hp > target.Hp) continue;
                        
                        target = en;
                    }
                }
                
                subject.GenericAttack(target);
            }
        }

        /// <summary>
        /// 敵の行動を決定する
        /// </summary>
        /// <param name="subject">行動主体</param>
        /// <param name="allUnits">戦闘に参加しているUnit全てを格納した配列</param>
        /// <param name="allies">味方のUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        public static void EvalEnemy(UnitEnemy subject, Unit[] allUnits, UnitEnemy[] allies, UnitAlly[] enemies)
        {
            // 敵から見たロジックなので敵味方が反転することに注意
            Unit targetEnemy = enemies[Random.Range(0, enemies.Length - 1)];
            var targetAlly = allies[Random.Range(0, allies.Length - 1)];
            
            // 洗脳
            if (HasEffectType(subject.StatusEffects, StatusEffectType.BrainWash))
            {
                targetEnemy = allies[Random.Range(0, allies.Length - 1)];
            }
            
            // 怒り
            if (HasEffectType(subject.StatusEffects, StatusEffectType.Anger))
            {
                subject.GenericAttack(targetEnemy);
            }
            
            if (subject.IsUnderSkillCooldown)
            {
                subject.GenericAttack(targetEnemy);
                subject.IsUnderSkillCooldown = false;
                return;
            }

            if (Random.value < subject.SkillThreshold)
            {
                var chosenSkill = subject.Skills[
                    Random.Range(0, subject.Skills.Length - 1)
                ];
                
                CallEnemySkill(subject, targetEnemy, targetAlly, chosenSkill);
                return;
            }
            
            subject.GenericAttack(targetEnemy);
        }

        // Interfaceを使ってBraverSkillDataとEnemySkillDataを共通化したいが、Interfaceを継承するとSerializableでなくなってしまうっぽい
        // この辺理解が足りていないので多分もっとちゃんとした形があると思われる
        private static void CallBraverSkill(Unit subject, Unit targetEnemy, Unit targetAlly, BraverSkillData skill)
        {
            foreach (var property in skill.SkillProperties)
            {
                switch (property.Type)
                {
                    case SkillType.DeEffect:
                        subject.GiveDeEffect(targetAlly);
                        break;
                    case SkillType.Heal:
                        subject.GiveHeal(targetAlly, property.Amount);
                        break;
                    case SkillType.Effect:
                        subject.GiveEffect(
                            property.StatusEffect.IsFriendly ? targetAlly : targetEnemy,
                            property.StatusEffect
                        );
                        break;
                    case SkillType.Attack:
                        subject.GiveDamage(targetEnemy, property.Amount, property.SkillAttribute);
                        break;
                    default:
                        Debug.LogError("CombatUtilities > CallBraverSkill(): invalid skill property type. ( " + property.Type + " )");
                        break;
                }
            }
        }

        private static void CallEnemySkill(Unit subject, Unit targetEnemy, Unit targetAlly, EnemySkillData skill)
        {
            foreach (var property in skill.SkillProperties)
            {
                switch (property.Type)
                {
                    case SkillType.DeEffect:
                        subject.GiveDeEffect(targetAlly);
                        break;
                    case SkillType.Heal:
                        subject.GiveHeal(targetAlly, property.Amount);
                        break;
                    case SkillType.Effect:
                        subject.GiveEffect(
                            property.StatusEffect.IsFriendly ? targetAlly : targetEnemy,
                            property.StatusEffect
                        );
                        break;
                    case SkillType.Attack:
                        subject.GiveDamage(targetEnemy, property.Amount, property.SkillAttribute);
                        break;
                }
            }
        }

        public static void InitStatusEffect(Unit target, StatusEffectData ef)
        {
            foreach (var prop in ef.Properties)
            {
                switch (prop.Type)
                {
                    case StatusEffectType.Mp:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] MP modified by {prop.Amount}");
                        target.MpModifier += (int) Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.PAtk:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] PATK modified by {prop.Amount}");
                        target.PAtkModifier += (int)Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.PDef:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] PDEF modified by {prop.Amount}");
                        target.PDefModifier += (int)Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.MAtk:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] MATK modified by {prop.Amount}");
                        target.MAtkModifier += (int)Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.MDef:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] MDEF modified by {prop.Amount}");
                        target.MDefModifier += (int)Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.Speed:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] SPEED modified by {prop.Amount}");
                        target.SpeedModifier += (int)Math.Round(prop.Amount);
                        break;
                    case StatusEffectType.Weak:
                        Debug.Log($"{target.Name}: [ ADD EFFECT ] MULTIPLE DAMAGE (Weak) {prop.Amount} time(s)");
                        target.WeakMultipliers.Add(prop.Amount);
                        break;
                    default:
                        Debug.Log($"{target.Name}: [ REGISTER EFFECT ] {prop.Type}");
                        break;
                    
                }
            }
        }
        
        public static void RemoveStatusEffect(Unit target, StatusEffectData ef)
        {
            foreach (var prop in ef.Properties)
            {
                switch (prop.Type)
                {
                    case StatusEffectType.Mp:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] MP modified by {prop.Amount}");
                        target.MpModifier = Math.Max(0, target.MpModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.PAtk:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] PATK modified by {prop.Amount}");
                        target.PAtkModifier = Math.Max(0, target.PAtkModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.PDef:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] PDEF modified by {prop.Amount}");
                        target.PDefModifier = Math.Max(0, target.PDefModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.MAtk:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] MATK modified by {prop.Amount}");
                        target.MAtkModifier = Math.Max(0, target.MAtkModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.MDef:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] MDEF modified by {prop.Amount}");
                        target.MDefModifier = Math.Max(0, target.MDefModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.Speed:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] SPEED modified by {prop.Amount}");
                        target.SpeedModifier = Math.Max(0, target.SpeedModifier - (int)Math.Round(prop.Amount));
                        break;
                    case StatusEffectType.Weak:
                        Debug.Log($"{target.Name}: [ SUBTRACT EFFECT ] MULTIPLE DAMAGE (Weak) {prop.Amount} time(s)");
                        target.WeakMultipliers.Remove(prop.Amount);
                        break;
                    default:
                        Debug.Log($"{target.Name}: [ UNREGISTER EFFECT ] {prop.Type}");
                        break;
                }
            }
        }
        
        public static void Miasma(Unit target, float rate)
        {
            target.ReceiveDamage(target.MaxHp * rate, SkillAttribute.Physical);
        }

        public static bool HasEffectType(List<StatusEffectData> effAry, StatusEffectType targetType)
        {
            return effAry
                .SelectMany(ef => ef.Properties)
                .Any(prop => prop.Type == targetType);
        }
    }
}