using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//QueuedAction Struct
//Holds the target of the ability, the ability itself and the speed of the action
public struct QueuedAction
{
    Unit target;
    Ability ability;
    int speed;

    public QueuedAction(Unit target, Ability ability, int speed)
    {
        this.target = target;
        this.ability = ability;
        this.speed = speed;
    }
}
//Status Struct
//Holds a string that describes the status, an int for duration, and an int for magnitude
public struct Status
{
    StatusType type;
    StatusName name; 
    int duration;
    int magnitude;

    public Status(StatusType type, StatusName name, int duration, int magnitude)
    {
        this.type = type;
        this.name = name;
        this.duration = duration;
        this.magnitude = magnitude;
    }
}

public class Unit : MonoBehaviour
{
    string unitName;
    Ability[] abilities;
    Status[] statuses;
    Unit[] allies;
    Unit[] enemies;
    StatusName[] immunity; 
    int fatigue;
    int hp;
    int maxHP;
    Unit priorTarget;
    bool dead;
    bool isActive;

    public Unit(string name, StatusName[] immunity, Ability[] abilities, int hp)
    {
        this.unitName = name;
        this.immunity = immunity;
        this.abilities = abilities;
        this.hp = hp;
        this.maxHP = hp;
        this.dead = false;
        this.priorTarget = null;
        this.fatigue = 0;
        this.isActive = true;
        Status health = new Status(StatusType.Health, StatusName.None, 0, 0);
        Status buff = new Status(StatusType.Buff, StatusName.None, 0, 0);
        Status debuff = new Status(StatusType.Debuff, StatusName.None, 0, 0);
        this.statuses = new Status[] {health,  buff, debuff};
    }
    void getTeams()
    {

    }
    void act()
    {

    }

    void turnEnd()
    {

    }

    void makeActive()
    {

    }

    void stopActive()
    {

    }

    void healSelf()
    {

    }

    public void takeDamage(Unit source, int damage)
    {

    }

    void applyStatus()
    {

    }

    void turnStart()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

