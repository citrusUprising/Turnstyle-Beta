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
    public string unitName;
    public Ability[] abilities;
    public Status[] statuses;
    public GameObject[] allies;
    public GameObject[] enemies;
    public StatusName[] immunity; 
    public int fatigue;
    public int hp;
    public int maxHP;
    public Unit priorTarget;
    public bool dead;
    public bool isActive;
    public QueuedAction queuedAction;
    public MainLoop gameLoop;

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

    public void act()
    {
        if(this.statuses[(int) StatusType.Debuff].name == StatusName.Flinch)
        {
            gameLoop.outputQueue.Add(this.unitName + " flinched");
            this.statuses[(int) StatusType.Debuff].name = StatusName.None;
            this.statuses[(int) StatusType.Debuff].duration = 0;
            return;
        }
        //we don't always want to print this so it has been moved
        //gameLoop.outputQueue.Add(this.unitName + " used " + this.queuedAction.ability.name + "!");
        this.getTeams();
        if(this.queuedAction.ability.multitarget == true)
        {
            bool used = false;
            if(this.queuedAction.ability.allies == true)
            {
                foreach(GameObject o in this.allies)
                {
                    if(!o.GetComponent<Unit>().dead){
                        if(!used){
                            gameLoop.outputQueue.Add(this.unitName + " used " + this.queuedAction.ability.name + "!");
                            used = true;
                        }
                        this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                    }
                }
            } 
            else
            {
                foreach(GameObject o in this.enemies) { 
                    if(!o.GetComponent<Unit>().dead){
                        if(!used){
                            gameLoop.outputQueue.Add(this.unitName + " used " + this.queuedAction.ability.name + "!");
                            used = true;
                        }
                        this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                    }
                }
            }
            if(!used)
                gameLoop.outputQueue.Add(this.unitName + "'s ability had no targets!");
        } else if (this.queuedAction.ability.selftarget == true)
        {
            gameLoop.outputQueue.Add(this.unitName + " used " + this.queuedAction.ability.name + "!");
            this.queuedAction.ability.effect(this, this, gameLoop);
        }
        else
        {
            if(!this.queuedAction.target.dead){
                gameLoop.outputQueue.Add(this.unitName + " used " + this.queuedAction.ability.name + "!");
                this.queuedAction.ability.effect(this.queuedAction.target, this, gameLoop);
            }
            else
                gameLoop.outputQueue.Add(this.unitName + "'s ability had no target!");
        }
        this.fatigue += 1;
    }

    public void turnEnd()
    {
        gameLoop.outputQueue.Add(this.unitName + " has ended their turn");
        Debug.Log(this.unitName + " has ended their turn");
        //Debug.Log("queuedAction.ability.name: " + this.queuedAction.ability.name);
        if (this.queuedAction != null)
        {
            this.priorTarget = this.queuedAction.target;
            this.queuedAction.target = null;
            this.queuedAction.ability = null;
            this.queuedAction.speed = 0;
        }

        if(this.statuses[(int) StatusType.Health].name == StatusName.Regeneration)
        {
            this.hp = Math.Min(this.hp + this.statuses[(int) StatusType.Health].magnitude, this.maxHP);
        }
        if(this.statuses[(int) StatusType.Health].name == StatusName.Burn)
        {
            this.hp = Math.Max(0, this.hp - this.statuses[(int) StatusType.Health].magnitude);
        }
        if(this.hp <= 0 && !this.dead)
        { //I have replaced the queue with just printing to the debug log for the moment
            //I have also not set anything to change tint
            this.dead = true;
            gameLoop.outputQueue.Add(this.unitName + " died!");
        }
        if(this.unitName.Equals("Jade")&&this.fatigue < 2){
            gameLoop.outputQueue.Add("This ability is under construction"); //flag
            /*foreach(GameObject o in this.allies){
                if(o.statuses[(int)StatusType.Debuff].name != StatusName.None){
                    gameLoop.outputQueue.Add(this.unitName+" cured "+o.unitName+" of any conditions");
                    o.statuses[(int)StatusType.Debuff].name = StatusName.None;
                    o.statuses[(int)StatusType.Debuff].duration = 0;
                    o.statuses[(int)StatusType.Debuff].magnitude = 0;
                }
            */}
        if(this.unitName.Equals("Amery")){
            gameLoop.outputQueue.Add("This ability is under construction"); //flag
            /*bool fullHealth = true;
            Unit highest;
            int highestHP = 0;
            Unit lowest;
            int lowestHP = 21;

            foreach(GameObject o in this.allies){
                if (o.hp < o.maxHP){
                    fullHealth = false;
                    break;
                    }
                }
            if (!fullHealth){
                foreach(GameObject o in this.allies){
                    if(o.hp > highestHP){
                        highest = o;
                        highestHP = o.hp;
                        }
                    if(o.hp < lowestHP){
                        lowest = o;
                        lowestHP = o.hp;
                        }
                    }
                lowest.healSelf(4);
                highest.hp = Math.Min(highest.hp-4, highest.maxHP);
                }*/
            }
        foreach(Status s in this.statuses)
        {
            if(s.duration > 0)
            {
                s.duration -= 1;
                if(s.duration == 0)
                {
                    gameLoop.outputQueue.Add(this.unitName + "'s " + s.name + " wore off");
                    s.name = StatusName.None;
                }
            }
        }
    }

    public void makeActive()
    {
        if (!this.isActive)
        {
            if(this.unitName.Equals("Koralie"))
            {
                this.statuses[(int) StatusType.Health].name = StatusName.None;
            }
            this.isActive = true;
        }
    }

    public void stopActive()
    {
        if (this.isActive)
        {
            foreach(Status s in this.statuses)
            {
                s.duration = 0;
                s.name = StatusName.None;
            }
            if(this.unitName == "Koralie")
            {
                this.statuses[(int) StatusType.Health].name = StatusName.Regeneration;
            }
            this.isActive = false;
        }
        this.fatigue = 0;
    }

    public void healSelf(int amount)
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
        if(this.unitName.Equals("Seraphim") && this.queuedAction.speed >= 5 && UnityEngine.Random.Range(0,1) > 0.5)
        { //again no output queue just printing to debug log
            gameLoop.outputQueue.Add(this.unitName + " dodged the attack");
            return;
        }
        if(source.unitName.Equals("Beverly") && source.queuedAction.target == source.priorTarget)
        {
            amount = (int) Math.Ceiling(amount * 1.5);
        }
        this.hp = Math.Max(this.hp - amount, 0);
        gameLoop.outputQueue.Add(this.unitName + " took " + amount + " damage.");
        if (this.hp == 0 && !this.dead){ //No output queue just printing to debug log
                                           //No tint changing either
            this.dead = true;
            gameLoop.outputQueue.Add(this.unitName + " died!");
        }
    }

   public void applyStatus(StatusType type, StatusName newStatus, int duration, int magnitude)
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
            gameLoop.outputQueue.Add(this.unitName + " has " + newStatus);
        }
    }

   public void turnStart()
    {
    }

    public void Kill()
    {
        if (this.dead)
        {
            gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
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

