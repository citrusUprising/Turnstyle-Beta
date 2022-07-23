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
    public Ability[] allAbilities;
    public GameObject gameStats;

    // Start is called before the first frame update
    void Awake()
    {
        populateArray();
        gameStats = GameObject.Find("CurrentStats");

        koralie.abilities[0] = allAbilities[gameStats.GetComponent<CurrentStats>().KoralieAbilities[0]];
        koralie.abilities[1] = allAbilities[gameStats.GetComponent<CurrentStats>().KoralieAbilities[1]];
        koralie.abilities[2] = allAbilities[gameStats.GetComponent<CurrentStats>().KoralieAbilities[2]];

        koralie.passive.name = "Decompress";
        koralie.passive.description = "Koralie gains Regen 3 when they start the turn inactive.";

        jade.abilities[0] = allAbilities[gameStats.GetComponent<CurrentStats>().JadeAbilities[0]];
        jade.abilities[1] = allAbilities[gameStats.GetComponent<CurrentStats>().JadeAbilities[1]];
        jade.abilities[2] = allAbilities[gameStats.GetComponent<CurrentStats>().JadeAbilities[2]];

        jade.passive.name = "Support";
        jade.passive.description = "If Jade is at less than 2 Fatigue at the end of the turn, he cures all Ally Debuffs.";

       seraphim.abilities[0] = allAbilities[gameStats.GetComponent<CurrentStats>().SeraphimAbilities[0]];
       seraphim.abilities[1] = allAbilities[gameStats.GetComponent<CurrentStats>().SeraphimAbilities[1]];
       seraphim.abilities[2] = allAbilities[gameStats.GetComponent<CurrentStats>().SeraphimAbilities[2]];

        seraphim.passive.name = "Dodge";
        seraphim.passive.description = "Seraphim has a 10% chance to dodge incoming attacks. This number is multiplied by their current Speed.";

        beverly.abilities[0] = allAbilities[gameStats.GetComponent<CurrentStats>().BeverlyAbilities[0]];
        beverly.abilities[1] = allAbilities[gameStats.GetComponent<CurrentStats>().BeverlyAbilities[1]];
        beverly.abilities[2] = allAbilities[gameStats.GetComponent<CurrentStats>().BeverlyAbilities[2]];

        beverly.passive.name = "Second Wind";
        beverly.passive.description = "When Beverly defeats an Enemy, her Fatigue goes down by 2.";

        amery.abilities[0] = allAbilities[gameStats.GetComponent<CurrentStats>().AmeryAbilities[0]];
        amery.abilities[1] = allAbilities[gameStats.GetComponent<CurrentStats>().AmeryAbilities[1]];
        amery.abilities[2] = allAbilities[gameStats.GetComponent<CurrentStats>().AmeryAbilities[2]];

        amery.passive.name = "Delegate";
        amery.passive.description = "Each turn, Amery heals the lowest health Ally for 4 and deals 4 damage to the highest health Ally.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void populateArray(){
        allAbilities = new Ability[]{
            new Hunker(), 
            new Crush(), 
            new Repel(), 
            new Stunnerclap(), 
            new Rally(),
            new Motivate(),
            new Scry(),
            new Soulrip3(),
            new Scream2(),
            new Smolder(),
            new Dazzle(),
            new Imbibe(),
            new Mitigate(),
            new Unionize(),
            new Scrum()
            };
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