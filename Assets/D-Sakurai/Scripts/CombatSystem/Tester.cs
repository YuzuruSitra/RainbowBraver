using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using D_Sakurai.Scripts.CombatSystem;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private CombatManager _manager;

    [SerializeField] private int dutyId;
    [SerializeField] TestBraver[] testBravers;

    [Serializable]
    public class TestBraver
    {
        public string name;
        public Affiliation affiliation;
        public int maxHp;
        public int maxMp;
        public float pAtk;
        public string pAtkLabel;
        public float pDef;
        public float mAtk;
        public string mAtkLabel;
        public float mDef;
        public int speed;
        public Job job;
        public Personality personality;
        public int jobSkillIndex;
        public int personalitySkillIndex;
        public float friendShipLevel;
    }

    void Start()
    {
        _manager = GetComponent<CombatManager>();

        Setup();
        Commence();
    }

    void Setup()
    {
        UnitAlly[] allies = testBravers.Select(bvr => new UnitAlly(
            bvr.name,
            bvr.affiliation,
            bvr.maxHp,
            bvr.maxMp,
            bvr.pAtk,
            bvr.pAtkLabel,
            bvr.pDef,
            bvr.mAtk,
            bvr.mAtkLabel,
            bvr.mDef,
            bvr.speed,
            bvr.job,
            bvr.personality,
            bvr.jobSkillIndex,
            bvr.personalitySkillIndex,
            bvr.friendShipLevel
            )).ToArray();

        _manager.Setup(dutyId, allies);
    }

    public static UnitAlly[] GetInstancedBravers(TestBraver[] tester)
    {
        return tester.Select(bvr => new UnitAlly(
            bvr.name,
            bvr.affiliation,
            bvr.maxHp,
            bvr.maxMp,
            bvr.pAtk,
            bvr.pAtkLabel,
            bvr.pDef,
            bvr.mAtk,
            bvr.mAtkLabel,
            bvr.mDef,
            bvr.speed,
            bvr.job,
            bvr.personality,
            bvr.jobSkillIndex,
            bvr.personalitySkillIndex,
            bvr.friendShipLevel
            )).ToArray();
    }

    public void Commence()
    {
        _manager.Commence();
    }

    // Update is called once per frame
    void Update()
    {
    }
}