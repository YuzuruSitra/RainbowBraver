using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Skillの実際の中身をActionとして分割する。SkillDataを名前とActionの配列を格納したクラスに変更する
public class SkillData{
    public bool IsBasic;
    public enum SkillType{Attack, Heal, Effect};
    public SkillType Type;
    public float Amount;

    public SkillData(bool isBasic, SkillType type, float amount){
        IsBasic = isBasic;
        Type = type;
        Amount = amount;
    }
}

public class JobSkillBase{
    public string SkillName;
    public string SkillDescription;
}

public static class JobSkills{
    public static class Swordsman{
        public static string name;
        public static SkillData[] data = {
            new SkillData(true, SkillData.SkillType.Attack, 10),
            new SkillData(true, SkillData.SkillType.Attack, 20),
            new SkillData(true, SkillData.SkillType.Heal, 30)
        };
    }
}
