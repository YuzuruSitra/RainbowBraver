using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unitの所属をあらわすEnum
/// </summary>
public enum Affiliation{ Player, Enemy }
/// <summary>
/// UnitAllyの職業をあらわすEnum
/// </summary>
public enum Job{Swordsman, Gladiator, Spearer, Hunter, Oracle, Sorcerer}

/// <summary>
/// ユニットの基底クラスを初期化する際に使用する構造体
/// </summary>
public struct UnitData{
    public Affiliation Affiliation;

    public int MaxHp;
    public int MaxMp;

    public float PAtk;
    public string PAtkLabel;
    public float PDef;

    public float MAtk;
    public string MAtkLabel;
    public float MDef;

    public int Speed;
}

/// <summary>
/// ユニットの基底クラス
/// </summary>
public class Unit
{
    public Affiliation Affiliation{ get; private set; }

    public int MaxHp{ get; private set; }
    public int MaxMp{ get; private set; }
    public int Hp{ get; private set; }
    public int Mp{ get; private set; }

    public float PAtk{ get; private set; }
    public string PAtkLabel{ get; private set; }
    public float PDef{ get; private set; }

    public float MAtk{ get; private set; }
    public string MAtkLabel{ get; private set; }
    public float MDef{ get; private set; }

    public int Speed{ get; private set; }

    public Unit(UnitData data){
        Affiliation = data.Affiliation;
        MaxHp = data.MaxHp;
        MaxMp = data.MaxMp;

        Hp = MaxHp;
        Mp = MaxMp;

        PAtk = data.PAtk;
        PAtkLabel = data.PAtkLabel;
        PDef = data.PDef;

        MAtk = data.MAtk;
        MAtkLabel = data.MAtkLabel;
        MDef = data.MDef;

        Speed = data.Speed;
    }
}


/// <summary>
/// UnitAllyを初期化する際に使用する構造体
/// </summary>
public struct UnitAllyData
{
    public UnitData unitData;

    public Affiliation Affiliation;

    public Job job;
    public int JobSkillIndex;
    public int PersonalitySkillIndex;
}

/// <summary>
/// 味方のUnit
/// </summary>
public class UnitAlly : Unit
{
    public int JobSkillIndex{ get; private set; }
    public int PersonalitySkillIndex{ get; private set; }

    public UnitAlly(UnitAllyData data) : base(data.unitData)
    {
        JobSkillIndex = data.JobSkillIndex;
        PersonalitySkillIndex = data.PersonalitySkillIndex;
        
    }
}


/// <summary>
/// UnitEnemyを初期化する際に使用する構造体
/// </summary>
public struct UnitEnemyData{
    public UnitData unitData;
}
/// <summary>
/// 敵のUnit
/// </summary>
public class UnitEnemy : Unit{
    public UnitEnemy(UnitEnemyData data) : base(data.unitData){
    }
}