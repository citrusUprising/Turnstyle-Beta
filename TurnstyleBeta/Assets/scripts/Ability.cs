using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Ability
{
    public string name;
    public string text;
    public bool multitarget;
    public bool selftarget;
    public bool allies;
    public int damage;
    public abstract void effect(Unit target, Unit source, MainLoop L);

    public abstract bool requirement(Unit target, Unit source); 
}

public class BasicAttack : Ability
{
    public BasicAttack() {
        this.name = "Basic Attack";
        this.text = "Deal 3 damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
        this.damage = 3;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 3); 
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class BasicHeal : Ability
{
    public BasicHeal()
    {
        this.name = "Basic Heal";
        this.text = "Heal 3 damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
        this.damage = 0;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.hp = Math.Min(target.hp + 3, target.maxHP);
    }
    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class GroupAttack : Ability
{
    public GroupAttack()
    {
        this.name = "Group Attack";
        this.text = "Deal 1 damage to all enemies";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
        this.damage = 1;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 1);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class GroupHeal : Ability
{
    public GroupHeal()
    {
        this.name = "Group Heal";
        this.text = "Heal 1 damage to all allies.";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.hp = Math.Min(target.hp + 1, target.maxHP);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class HeavyAttack : Ability
{
    public HeavyAttack()
    {
        this.name = "Heavy Attack";
        this.text = "Deal 8 Damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
        this.damage = 8;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 8);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class SelfHeal : Ability
{
    public SelfHeal()
    {
        this.name = "Self Heal";
        this.text = "Heal 8 damage.";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.healSelf(5);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}


public class Mitigate : Ability
{
    public Mitigate()
    {
        this.name = "Mitigate";
        this.text = "Give ally Regeneration(3,4)";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName + " used " + this.name + " on " + target.unitName);
        target.applyStatus(StatusType.Health, StatusName.Regeneration, 3, 4);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Scrum : Ability
{
    public Scrum()
    {
        this.name = "Scrum";
        this.text = "Cure an ally of debuffs, then inflict them with Null(3)";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
  if(target.statuses[(int) StatusType.Debuff].name != StatusName.None){
    L.outputQueue.Add(target.unitName+" was cured of "+target.statuses[(int) StatusType.Debuff].name+" and given Null");
  }else{
    L.outputQueue.Add(target.unitName+" was given Null");
  }
  target.statuses[(int) StatusType.Debuff].name = StatusName.None;
  target.statuses[(int) StatusType.Debuff].duration = 0;
  target.statuses[(int) StatusType.Debuff].magnitude = 0;
  target.applyStatus(StatusType.Debuff, StatusName.Null, 3, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Smolder : Ability
{
    public Smolder()
    {
        this.name = "Smolder";
        this.text = "Hit enemy with fire, damaging them based on your speed";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
        target.takeDamage(source, 3 + source.queuedAction.speed/2);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Imbibe : Ability
{
    public Imbibe()
    {
        this.name = "Imbibe";
        this.text = "Give self Haste(2,5) and Strung Out(2)";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName +" imbibed coffee");
        target.applyStatus(StatusType.Buff,StatusName.Haste, 2, 5);
        target.applyStatus(StatusType.Debuff,StatusName.StrungOut, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Repel : Ability
{
    public Repel()
    {
        this.name = "Repel";
        this.text = "Hit all enemies for 2 damage.";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName + " used " + this.name + " on enemy team");
        target.takeDamage(source, 2);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Fallguy : Ability
{
    public Fallguy()
    {
        this.name = "Fall Guy";
        this.text = "Give all allies Aegis(1), give self Distracted(1).";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
    L.outputQueue.Add(source.unitName+" used "+this.name);
    if(target.unitName != source.unitName)
    target.applyStatus(StatusType.Buff,StatusName.Aegis, 1, 0);
    else
    source.applyStatus(StatusType.Debuff,StatusName.Distracted, 1, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Crush : Ability
{
    public Crush()
    {
        this.name = "Crush";
        this.text = "Deal 8 damage to a target and 4 damage to self.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
        target.takeDamage(source, 8);
        source.hp = Math.Min(source.hp-4, source.maxHP);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Rally : Ability
{
    public Rally()
    {
        this.name = "Rally";
        this.text = "Heal ally for 8 damage, while dealing 2 damage to self";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
        target.healSelf(8);
        source.hp = Math.Min(source.hp-2, source.maxHP);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Stunnerclap : Ability
{
    public Stunnerclap()
    {
        this.name = "Stunner Clap";
        this.text = "Deal 2 damage to targeted enemy and inflict Strung Out(1)";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
        target.takeDamage(source,2);
        target.applyStatus(StatusType.Debuff,StatusName.StrungOut, 1, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Soulrip : Ability
{
    public Soulrip()
    {
        this.name = "Soul Rip";
        this.text = "Deal 10 damage. Accuracy -25% for each level of fatigue";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" possessed "+target.unitName);
        if(source.fatigue <= 0 || UnityEngine.Random.Range(0,1) <= (1-.25*source.fatigue)){
            L.outputQueue.Add(target.unitName+"'s soul was torn");
            target.takeDamage(source, 10);}
        else L.outputQueue.Add(source.unitName+" couldn't manifest");
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Dazzle : Ability
{
    public Dazzle()
    {
        this.name = "Dazzle";
        this.text = "Targets all enemies, 35% chance to Flinch, 35% chance to Burn";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        if(UnityEngine.Random.Range(0,1) <= 0.35){
            L.outputQueue.Add(source.unitName+"'s "+this.name+" flinched "+target.unitName);
            target.applyStatus(StatusType.Debuff,StatusName.Flinch, 1, 0);
        }else if(UnityEngine.Random.Range(0,1) <= 0.35){
            L.outputQueue.Add(source.unitName+"'s "+this.name+" burned "+target.unitName);
            target.applyStatus(StatusType.Health, StatusName.Burn, 2, 4);
        }else L.outputQueue.Add(target.unitName+" avoided the "+this.name);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Scry : Ability
{
    public Scry()
    {
        this.name = "Scry";
        this.text = "Afflicts one targeted enemy with Distracted(2).";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" used "+this.name+" on "+target.unitName);
        target.applyStatus(StatusType.Debuff,StatusName.Distracted, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Motivate : Ability
{
    public Motivate()
    {
        this.name = "Motivate";
        this.text = "Grants Random effect (Enrage 1, Aegis 1, Haste 2,3) to an ally";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        float rng = UnityEngine.Random.Range(0,1);
        if(rng <= 0.33){
            L.outputQueue.Add(source.unitName+"'s "+this.name+" gave "+target.unitName+" Aegis(1)");
            target.applyStatus(StatusType.Buff,StatusName.Aegis, 1,0);
        }
        else if(rng <= 0.66){
            L.outputQueue.Add(source.unitName+"'s "+this.name+" gave "+target.unitName+" Enrage(1)");
            target.applyStatus(StatusType.Buff, StatusName.Enrage, 1,0);
        }
        else {
            L.outputQueue.Add(source.unitName+"'s "+this.name+" gave "+target.unitName+" Haste(2,3)");
            target.applyStatus(StatusType.Buff, StatusName.Haste, 2,3);
        }
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Slump : Ability
{
    public Slump()
    {
        this.name = "Slump";
        this.text = "Grants self Regen (1,6)";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" "+this.name+"ed");
        target.applyStatus(StatusType.Health,StatusName.Regeneration,1,6);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Hunker : Ability
{
    public Hunker()
    {
        this.name = "Hunker";
        this.text = "Grants self Aegis (2)";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        L.outputQueue.Add(source.unitName+" "+this.name+"ed");
        target.applyStatus(StatusType.Buff,StatusName.Aegis, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

/*
public class Temp : Ability
{
    public Temp()
    {
        this.name = "Temp";
        this.text = "Placeholder";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}
*/

