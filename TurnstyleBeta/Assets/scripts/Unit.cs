using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//QueuedAction Class
//Holds the target of the ability, the ability itself and the speed of the action
public class QueuedAction
{
    public Unit target;
    public Ability ability;
    public int speed;

    public QueuedAction(Unit target, Ability ability, int speed)
    {
        this.target = target;
        this.ability = ability;
        this.speed = speed;
    }
}
//Status Class
//Holds a string that describes the status, an int for duration, and an int for magnitude
public class Status
{
    public StatusType type;
    public StatusName name; 
    public int duration;
    public int magnitude;

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
    public GameObject[] allies;
    public GameObject[] enemies;
    StatusName[] immunity; 
    int fatigue;
    public int hp;
    public int maxHP;
    Unit priorTarget;
    bool dead;
    bool isActive;
    QueuedAction queuedAction;

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
        queuedAction = null;
    }
    void getTeams()
    {
        this.allies = GameObject.FindGameObjectsWithTag("Ally");
        this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void act()
    {
        if(this.statuses[(int) StatusType.Debuff].name == StatusName.Flinch)
        {
            Debug.Log(this.unitName + " flinched");
            this.statuses[(int) StatusType.Debuff].name = StatusName.None;
            this.statuses[(int) StatusType.Debuff].duration = 0;
            return;
        }
        this.getTeams();
        if(this.queuedAction.ability.multitarget == true)
        {
            if(this.queuedAction.ability.allies == true)
            {
                foreach(GameObject o in this.allies)
                {
                    this.queuedAction.ability.effect(o.GetComponent<Unit>(),this);
                }
            } 
            else
            {
                foreach(GameObject o in this.enemies) { 
                    this.queuedAction.ability.effect(o.GetComponent<Unit>(), this);
                }
            }
        } else if (this.queuedAction.ability.selftarget == true)
        {
            this.queuedAction.ability.effect(this, this);
        }
        else
        {
            this.queuedAction.ability.effect(this.queuedAction.target, this);
        }
        this.queuedAction.target = null;
        this.queuedAction.ability = null;
        this.queuedAction.speed = 0;
    }

    void turnEnd()
    {
        Debug.Log(this.unitName + " has ended their turn");
        this.priorTarget = this.queuedAction.target;
        this.queuedAction.target = null;
        this.queuedAction.ability = null;
        this.queuedAction.speed = 0;
        if(this.statuses[(int) StatusType.Health].name == StatusName.Regeneration)
        {
            this.hp = Math.Min(this.hp + 3, this.maxHP);
        }
        if(this.statuses[(int) StatusType.Health].name == StatusName.Burn)
        {
            this.hp = Math.Max(0, this.hp - 3);
        }
        if(this.hp == 0 && !this.dead)
        { //I have replaced the queue with just printing to the debug log for the moment
            //I have also not set anything to change tint
            this.dead = true;
            Debug.Log(this.unitName + " died!");
        }
        foreach(Status s in this.statuses)
        {
            if(s.duration > 0)
            {
                s.duration -= 1;
                if(s.duration == 0)
                {
                    Debug.Log(this.unitName + "'s " + s.name + " wore off");
                    s.name = StatusName.None;
                }
            }
        }
    }

    void makeActive()
    {
        if (!this.isActive)
        {
            if (this.unitName.Equals("Telepath"))
            {
                this.statuses[(int) StatusType.Health].name = StatusName.Regeneration;
                this.statuses[(int) StatusType.Health].duration = 0;
            }
            if(this.unitName.Equals("Juggernaut"))
            {
                this.statuses[(int) StatusType.Health].name = StatusName.None;
            }
            this.isActive = true;
        }
    }

    void stopActive()
    {
        if (this.isActive)
        {
            foreach(Status s in this.statuses)
            {
                s.duration = 0;
                s.name = StatusName.None;
            }
            if(this.unitName == "Juggernaut")
            {
                this.statuses[(int) StatusType.Health].name = StatusName.Regeneration;
            }
            this.isActive = false;
        }
        this.fatigue = 0;
    }

    void healSelf(int amount)
    {
        this.hp = Math.Min(this.hp + amount, this.maxHP);
    }

    public void takeDamage(Unit source, int amount)
    {
        if(this.statuses[(int) StatusType.Buff].name == StatusName.Aegis)
        {
            amount = (int)Math.Ceiling((double)amount / 2);
        }
        if(source.statuses[(int) StatusType.Buff].name == StatusName.Enrage)
        {
            amount = amount * 2;
        }
        if(this.statuses[(int) StatusType.Debuff].name == StatusName.Distracted)
        {
            amount = amount * 2;
        }
        if(source.statuses[(int) StatusType.Debuff].name == StatusName.StrungOut)
        {
            amount = (int) Math.Ceiling((double) amount / 2);
        }
        if(this.unitName.Equals("Medic") && this.queuedAction.speed >= 5 && UnityEngine.Random.Range(0,1) > 0.5)
        { //again no output queue just printing to debug log
            Debug.Log(this.unitName + " dodged the attack");
            return;
        }
        if(source.unitName.Equals("Bounty Hunter") && source.queuedAction.target == source.priorTarget)
        {
            amount = (int) Math.Ceiling(amount * 1.5);
        }
        this.hp = Math.Max(this.hp - amount, 0);
        Debug.Log(this.unitName + " took " + amount + " damage.");
        if (this.hp == 0 && !this.dead){ //No output queue just printing to debug log
                                           //No tint changing either
            this.dead = true;
            Debug.Log(this.unitName + " died!");
        }
    }

    void applyStatus(StatusType type, StatusName newStatus, int duration, int magnitude)
    {
        foreach(StatusName s in this.immunity)
        {
            if(s == newStatus)
            {
                return;
            }
        }
        if(this.statuses[(int) type].name != StatusName.None)
        {
            return;
        }
        else
        { //again no outputQueue
            this.statuses[(int) type].name = newStatus;
            this.statuses[(int) type].duration = duration;
            this.statuses[(int) type].magnitude = magnitude;
            Debug.Log(this.unitName + " has " + newStatus);
        }
    }

    void turnStart()
    {
        if (this.unitName.Equals("Sniper") && this.fatigue == 0)
        {
            this.statuses[(int) StatusType.Buff].name = StatusName.Aegis;
            this.statuses[(int) StatusType.Buff].duration = 1;
        }
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

