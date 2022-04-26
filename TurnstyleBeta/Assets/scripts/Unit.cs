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
    static Dictionary<StatusName, string> statusNames;
    

    static Unit(){
        statusNames = new Dictionary<StatusName, string>();
        statusNames.Add(StatusName.Haste, "hasted");
    }

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
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " flinched",
            this,
            StatusName.Flinch,
            false)
            );
            this.statuses[(int) StatusType.Debuff].name = StatusName.None;
            this.statuses[(int) StatusType.Debuff].duration = 0;
            return;
        }
        //we don't always want to print this so it has been moved
        //gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!"), true);
        this.getTeams();
        if(this.queuedAction.ability.multitarget == true)
        {
            bool used = false;
            if(this.queuedAction.ability.allies == true)
            {
                if(this.tag == "Ally"){
                    foreach(GameObject o in this.allies){
                        if(!o.GetComponent<Unit>().dead && o.GetComponent<Unit>().isActive){
                            if(!used){
                                gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                                this,
                                StatusName.None,
                                false)
                                );
                                used = true;
                            }
                            this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                        }
                    }
                }else{
                    foreach(GameObject o in this.enemies){ 
                        if(!o.GetComponent<Unit>().dead){
                            if(!used){
                                gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                                this,
                                StatusName.None,
                                false)
                                );
                                used = true;
                            }
                            this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                        }
                    }
                }
            } 
            else
            {
                if(this.tag == "Ally"){
                    foreach(GameObject o in this.enemies) { 
                        if(!o.GetComponent<Unit>().dead){
                            if(!used){
                                gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                                this,
                                StatusName.None,
                                false)
                                );
                                used = true;
                            }
                            this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                        }
                    }
                }else{
                    foreach(GameObject o in this.allies){
                        if(!o.GetComponent<Unit>().dead && o.GetComponent<Unit>().isActive){
                            if(!used){
                                gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                                this,
                                StatusName.None,
                                false)
                                );
                                used = true;
                            }
                            this.queuedAction.ability.effect(o.GetComponent<Unit>(),this, gameLoop);
                        }
                    }
                }
            }
            if(!used)
                gameLoop.outputQueue.Add(new displayObject(this.unitName + "'s ability had no targets!",
                false)
                );
        } else if (this.queuedAction.ability.selftarget == true)
        {
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                this,
                StatusName.None,
                false)
                );
            this.queuedAction.ability.effect(this, this, gameLoop);
        }
        else
        {
            if(!this.queuedAction.target.dead || this.queuedAction.ability.name == "Rally" || this.queuedAction.ability.name == "Mitigate"){
                gameLoop.outputQueue.Add(new displayObject(this.unitName + " used " + this.queuedAction.ability.name + "!",
                    this,
                    StatusName.None,
                    false)
                    );
                this.queuedAction.ability.effect(this.queuedAction.target, this, gameLoop);
            }
            else
                gameLoop.outputQueue.Add(new displayObject(this.unitName + "'s ability had no target!",
                false)
                );
        }
        if(this.queuedAction.speed < 0){
            this.hp = Math.Max(this.hp - (this.maxHP/5), 0);
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " took "+this.maxHP/5+" damage from exhaustion",
            this,
            this.maxHP/5,
            true,
            "damage")
            );
        }
        this.fatigue += 1;
        if(this.statuses[(int)StatusType.Debuff].name == StatusName.FatigueUP) this.fatigue += 1;
        
    }

    public void turnEnd()
    {
        if(!this.isActive)this.fatigue = 0;
        //gameLoop.outputQueue.Add(new displayObject(this.unitName + " has ended their turn");
        Debug.Log(this.unitName + " has ended their turn");
        //Debug.Log("queuedAction.ability.name: " + this.queuedAction.ability.name);
        if (this.queuedAction != null)
        {
            this.priorTarget = this.queuedAction.target;
            this.queuedAction.target = null;
            this.queuedAction.ability = null;
            this.queuedAction.speed = 0;
        }

        if(this.statuses[(int) StatusType.Health].name == StatusName.Regeneration && (!this.dead||this.tag == "Ally"))
        {
            this.hp = Math.Min(this.hp + this.statuses[(int) StatusType.Health].magnitude, this.maxHP);
            gameLoop.outputQueue.Add(new displayObject(this.unitName+" regenerated "+this.statuses[(int) StatusType.Health].magnitude+" health",
            this,
            StatusName.Regeneration,
            true,
            "buff")
            );
        }
        if(this.statuses[(int) StatusType.Health].name == StatusName.Burn&&!this.dead)
        {
            this.hp = Math.Max(0, this.hp - this.statuses[(int) StatusType.Health].magnitude);
            gameLoop.outputQueue.Add(new displayObject(this.unitName+" took "+this.statuses[(int) StatusType.Health].magnitude+" damage from their "+StatusName.Burn,
            this,
            StatusName.Burn,
            true,
            "damage")
            );
        }
        if(this.tag == "Enemy"){
            foreach(Status s in this.statuses)
            {
                if(s.duration > 0)
                {
                    s.duration -= 1;
                    Debug.Log(s.name+" has "+s.duration+" turns left");
                    if(s.duration == 0 && !this.dead)
                    {
                        String test = s.name.ToString();
                        if(test.Contains("ed")||test == "Vulnerable")gameLoop.outputQueue.Add(new displayObject(this.unitName + " is no longer " + s.name,
                            this,
                            s.name,
                            false)
                            );
                        else gameLoop.outputQueue.Add(new displayObject(this.unitName + "'s " + s.name + " wore off",
                            this,
                            s.name,
                            false)
                            );
                        s.name = StatusName.None;
                        s.magnitude = 0;
                    }
                }
            }
        }
        if(this.hp <= 0 && !this.dead)
        { //I have replaced the queue with just printing to the debug log for the moment
            //I have also not set anything to change tint
            this.dead = true;
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " died!",
            false,
            "damage")
            );            
        }
        if(this.unitName.Equals("Jade")&&this.fatigue < 2&&this.isActive&& !this.dead){
            foreach(GameObject o in this.allies){
                Unit temp = o.GetComponent<Unit>();
                if(temp.statuses[(int)StatusType.Debuff].name != StatusName.None &&
                temp.statuses[(int)StatusType.Debuff].name != StatusName.Null){
                    gameLoop.outputQueue.Add(new displayObject(this.unitName+" cured "+temp.unitName+" of any conditions",
                    temp,
                    temp.statuses[(int)StatusType.Debuff].name,
                    false,
                    "buff")
                    );
                    temp.statuses[(int)StatusType.Debuff].name = StatusName.None;
                    temp.statuses[(int)StatusType.Debuff].duration = 0;
                    temp.statuses[(int)StatusType.Debuff].magnitude = 0;
                }
            }
        }
        if(this.unitName.Equals("Amery")&&this.isActive&& !this.dead){
            bool fullHealth = true;

            foreach(GameObject o in this.allies){
                Unit u = o.GetComponent<Unit>();
                if (u.hp < u.maxHP&&u.isActive){
                    fullHealth = false;
                    break;
                    }
                }
            if (!fullHealth){
                Unit highest = new Unit("null",new StatusName[0],new Ability[0], 0);
                int highestHP = 0;
                Unit lowest = new Unit("null",new StatusName[0],new Ability[0], 0);
                int lowestHP = 21;
                foreach(GameObject o in this.allies){
                    Unit u = o.GetComponent<Unit>();
                    if(u.hp > highestHP&&u.isActive){
                        highest = u;
                        highestHP = u.hp;
                        }
                    if(u.hp < lowestHP&&u.isActive){
                        lowest = u;
                        lowestHP = u.hp;
                        }
                    }
                        int transfer = Math.Min(4, lowest.maxHP-lowest.hp);
                        if(transfer >0){
                            gameLoop.outputQueue.Add(new displayObject("Amery delegated health",
                            false)
                            );
                            highest.hp = Math.Max(highest.hp-transfer, 0);
                            gameLoop.outputQueue.Add(new displayObject(highest.unitName+" gave "+transfer+" health",
                            highest,
                            transfer,
                            true,
                            "damage")
                            );
                            lowest.healSelf(transfer);
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
                this.statuses[(int)StatusType.Health].magnitude = 0;
            }
            this.isActive = true;
            Debug.Log(this.unitName + " has become active");
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
                s.magnitude = 0;
            }
            if(this.unitName == "Koralie")
            {
                this.statuses[(int) StatusType.Health].name = StatusName.Regeneration;
                this.statuses[(int)StatusType.Health].magnitude = 3;
            }
            this.isActive = false;
            Debug.Log(this.unitName + " has become inactive");
        }
    }

    public void healSelf(int amount)
    {
        this.hp = Math.Min(this.hp + amount, this.maxHP);
        gameLoop.outputQueue.Add(new displayObject(this.unitName+" regained "+amount+" health",
        this,
        amount,
        true)
        );
    }

    public void takeDamage(Unit source, int amount)
    {
        if(this.statuses[(int) StatusType.Buff].name == StatusName.Shielded)
        {
            amount = (int)Math.Ceiling((double)amount / 2);
        }
        if(source.statuses[(int) StatusType.Buff].name == StatusName.Strengthened)
        {
            amount = amount * 2;
        }
        if(this.statuses[(int) StatusType.Debuff].name == StatusName.Vulnerable)
        {
            amount = amount * 2;
        }
        if(source.statuses[(int) StatusType.Debuff].name == StatusName.Weakened)
        {
            amount = (int) Math.Ceiling((double) amount / 2);
        }
        if(this.unitName.Equals("Seraphim") && this.queuedAction.speed >= 5)
        { //again no output queue just printing to debug log
            float check = UnityEngine.Random.Range(0.0f,1.0f);
            Debug.Log("Seraphim's dodge check is" + check);
            if(check > 0.5){
                gameLoop.outputQueue.Add(new displayObject(this.unitName + " dodged the attack",
                this,
                StatusName.None,
                false)
                );
                return;
            }
        }
        this.hp = Math.Max(this.hp - amount, 0);
        gameLoop.outputQueue.Add(new displayObject(
            this.unitName + " took " + amount + " damage.",
            this,
            amount,
            true,
            "damage")
            );
        if (this.hp == 0 && !this.dead){ //No output queue just printing to debug log
                                           //No tint changing either
            this.dead = true;
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " died!",
            this,
            StatusName.None,
            false,
            "damage")
            );
            if(source.unitName.Equals("Beverly"))
            {
                source.fatigue = 0;
            gameLoop.outputQueue.Add(new displayObject(source.unitName+" gets a second wind",
            source,
            StatusName.None,
            false,
            "buff")
            );
            }
        }
    }

   public void applyStatus(StatusType type, StatusName newStatus, int duration, int magnitude)
    {
        foreach(StatusName s in this.immunity)
        {
            if(s == newStatus)
            {
                String s2 = s.ToString();
                 if(s2.Contains("ed")||s2 == "Vulnerable")
                gameLoop.outputQueue.Add(new displayObject(
                    this.unitName + " is immune to being "+s2,
                    this,
                    newStatus,
                    false,
                    "buff")
                    );
                else gameLoop.outputQueue.Add(new displayObject(
                    this.unitName + " is immune to "+s2,
                    this,
                    newStatus,
                    false,
                    "buff")
                    );
                return;
            }
        }
        if(this.statuses[(int) type].name != StatusName.None)
        {
            String cur =this.statuses[(int) type].name.ToString();
            if(cur.Contains("ed")||cur == "Vulnerable")
            gameLoop.outputQueue.Add(new displayObject(this.unitName + " is already "+cur,
            this,
            this.statuses[(int) type].name,
            false)
            );
            else gameLoop.outputQueue.Add(new displayObject(this.unitName + " already has "+cur,
            this,
            this.statuses[(int) type].name, 
            false)
            );
            return;
        }
        else
        { //again no outputQueue
            this.statuses[(int) type].name = newStatus;
            this.statuses[(int) type].duration = duration;
            this.statuses[(int) type].magnitude = magnitude;
            String test = newStatus.ToString();
            String sound = "null";
            if(type == StatusType.Buff) sound = "buff";

            if(test.Contains("ed")||test == "Vulnerable")gameLoop.outputQueue.Add(new displayObject(
                this.unitName + " is " + newStatus,
                this,
                newStatus,
                false,
                sound)
                );
            else gameLoop.outputQueue.Add(new displayObject(
                this.unitName + " has " + newStatus,
                this,
                newStatus,
                false,
                sound)
                );
            
            
        }
    }

   public void turnStart()
    {
        foreach(Status s in this.statuses)
        {
            if(s.duration > 0)
            {
                s.duration -= 1;
                Debug.Log(s.name+" has "+s.duration+" turns left");
                if(s.duration == 0)
                {
                    String test = s.name.ToString();
                    if(test.Contains("ed")||test == "Vulnerable")gameLoop.outputQueue.Add(new displayObject(this.unitName + " is no longer " + s.name,
                    this,
                    s.name,
                    false)
                    );
                    else gameLoop.outputQueue.Add(new displayObject(this.unitName + "'s " + s.name + " wore off",
                    this,
                    s.name,
                    false)
                    );
                    s.name = StatusName.None;
                }
            }
        }
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

