using System.Collections;
using System.Collections.Generic;
using D_Sakurai.Scripts.CombatSystem;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private CombatManager _manager;

    void Start()
    {
        _manager = gameObject.AddComponent<CombatManager>();

        _manager.Setup(
            0, new []
        {
            new UnitAlly("ally 0", Affiliation.Player, 100, 100, 10, "physical attack", 10, 10, "magical attack", 10, 3,
                Job.Gladiator, Personality.Active, 0, 0, 10),
            new UnitAlly("ally 1",Affiliation.Player, 100, 100, 10, "physical attack", 10, 10, "magical attack", 10, 4,
                Job.Gladiator, Personality.Active, 0, 0, 10)
        }
        );
        
        _manager.Commence();
    }

    // Update is called once per frame
    void Update()
    {
    }
}