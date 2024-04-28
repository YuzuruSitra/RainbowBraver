using System;
using D_Sakurai.Resources.Skills.SkillBase;
using Vector4 = UnityEngine.Vector4;
using Random = UnityEngine.Random;
using D_Sakurai.Scripts.CombatSystem.Units;

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
            // Extract living UnitAlly
            UnitAlly[] livingAllies = Array.FindAll(allies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingAllies.Length is 0 or shorter
            if (livingAllies.Length <= 0) return (true, Affiliation.Player);

            // Extract living UnitEnemy
            UnitEnemy[] livingEnemies = Array.FindAll(enemies, elem => elem.Hp > 0);
            // Return true(annihilated) if livingEnemies.Length is 0 or shorter
            if (livingEnemies.Length <= 0) return (true, Affiliation.Enemy);

            // Return false(dont annihilated)
            return (false, Affiliation.Player);
        }

        /// <summary>
        /// 次に行動するUnitを取得する
        /// </summary>
        /// <param name="allUnits">全Unitを格納した配列</param>
        /// <returns>次に行動するUnit</returns>
        public static Unit GetNextUnit(Unit[] allUnits)
        {
            // check if all units are actioned unit
            if (Array.FindAll(allUnits, unit => unit.Actioned).Length == allUnits.Length)
            {
                foreach (var unit in allUnits)
                {
                    unit.Actioned = false;
                }
            }

            Unit result = allUnits[0];
            float currentFastest = result.Speed;

            foreach (var unit in allUnits)
            {
                // If unit is not yet actioned and faster than current fastest unit
                if (unit.Actioned && unit.Speed > currentFastest)
                {
                    result = unit;
                }
            }

            return result;
        }

        /// <summary>
        /// 味方の行動を決定する
        /// </summary>
        /// <param name="subject">行動の主体</param>
        /// <param name="allUnits">全Unitを格納した配列</param>
        /// <param name="allies">味方のUnitを格納した配列</param>
        /// <param name="enemies">敵のUnitを格納した配列</param>
        /// <param name="decisionThresh">判断に用いる閾値</param>
        public static void EvalAlly(UnitAlly subject, Unit[] allUnits, UnitAlly[] allies, UnitEnemy[] enemies, Vector4 decisionThresh)
        {
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

            if (deEffectables.Length > 0 && hasUsableDeEffect && Random.value < decisionThresh.x)
            {
                CallBraverSkill(subject,
                    enemies[0],
                    deEffectables[0],
                    enoughMpForJobSkill ? subject.JobSkill : subject.PersonalitySkill
                );
            }
            else if (healables.Length > 0 && hasUsableHeal)
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
            else if (hasUsableEffect && Random.value < decisionThresh.z)
            {
                CallBraverSkill(subject,
                    enemies[0],
                    allies[0],
                    enoughMpForJobSkill ? subject.JobSkill : subject.PersonalitySkill
                );
            }
            else if (hasUsableAttack && Random.value > decisionThresh.w)
            {
                UnitEnemy attackTarget = enemies[0];

                if (enemies.Length > 1)
                {
                    if (enemies[1].Hp < enemies[0].Hp)
                    {
                        attackTarget = enemies[1];
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
            else
            {
                UnitEnemy target = enemies[0];

                foreach (var en in enemies)
                {
                    if (en.Hp < target.Hp)
                    {
                        target = en;
                    }
                }
                
                CallGenericAttack(subject, target);
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
            // TODO: 状態効果技を連発しないようにする
            var chosenSkill = subject.Skills[
                Random.Range(0, subject.Skills.Length - 1)
            ];

            // 敵から見たロジックなので敵味方が反転することに注意
            var targetEnemy = enemies[Random.Range(0, allies.Length - 1)];
            var targetAlly = allies[Random.Range(0, allies.Length - 1)];
            
            CallEnemySkill(subject, targetEnemy, targetAlly, chosenSkill);
        }

        // Interfaceを使ってBraverSkillDataとEnemySkillDataを共通化したいが、Interfaceを継承するとSerializableでなくなってしまうっぽい
        // この辺理解が足りていないので多分もっとちゃんとした形があると思われる(今の以下のコードはあまりにもダサい)
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
                        subject.GiveDamage(targetEnemy, property.Amount);
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
                        subject.GiveDamage(targetEnemy, property.Amount);
                        break;
                }
            }
        }

        private static void CallGenericAttack(Unit subject, Unit target)
        {
            subject.GenericAttack(target);
        }
    }
}