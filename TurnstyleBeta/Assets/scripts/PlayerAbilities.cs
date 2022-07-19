using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{

    public CharacterProfile koralie = new CharacterProfile();
    public CharacterProfile jade = new CharacterProfile();
    public CharacterProfile seraphim = new CharacterProfile();
    public CharacterProfile beverly = new CharacterProfile();
    public CharacterProfile amery = new CharacterProfile();

    // Start is called before the first frame update
    void Awake()
    {

        koralie.abilities[0] = new Hunker();
        koralie.abilities[1] = new Crush();
        koralie.abilities[2] = new Repel();

        koralie.passive.name = "Decompress";
        koralie.passive.description = "Koralie gains Regen 3 when they start the turn inactive.";

        jade.abilities[0] = new Stunnerclap();
        jade.abilities[1] = new Rally();
        jade.abilities[2] = new Motivate();

        jade.passive.name = "Support";
        jade.passive.description = "If Jade is at less than 2 Fatigue at the end of the turn, he cures all Ally Debuffs.";

        seraphim.abilities[0] = new Scry();
        seraphim.abilities[1] = new Soulrip3();
        seraphim.abilities[2] = new Scream2();

        seraphim.passive.name = "Dodge";
        seraphim.passive.description = "Seraphim has a 10% chance to dodge incoming attacks. This number is multiplied by their current Speed.";

        beverly.abilities[0] = new Smolder();
        beverly.abilities[1] = new Dazzle();
        beverly.abilities[2] = new Imbibe();

        beverly.passive.name = "Second Wind";
        beverly.passive.description = "When Beverly defeats an Enemy, her Fatigue goes down by 2.";

        amery.abilities[0] = new Mitigate();
        amery.abilities[1] = new Unionize();
        amery.abilities[2] = new Scrum();

        amery.passive.name = "Delegate";
        amery.passive.description = "Each turn, Amery heals the lowest health Ally for 4 and deals 4 damage to the highest health Ally.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class CharacterProfile
{
    public Passive passive = new Passive();
    public Ability[] abilities = new Ability[3];
}

public class Passive
{
    public string name;
    public string description;
}