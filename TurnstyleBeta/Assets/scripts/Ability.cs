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
    public abstract void effect(Unit target, Unit source);

    public abstract bool requirement(Unit target, Unit source); 
}

public class BasicAttack : Ability
{
    public BasicAttack() {
        this.name = "BasicAttack";
        this.text = "Deal 3 damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
        this.damage = 3;
    }

    public override void effect(Unit target, Unit source)
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
        this.name = "BasicHeal";
        this.text = "Heal 3 damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
        this.damage = 0;
    }

    public override void effect(Unit target, Unit source)
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
        this.name = "GroupAttack";
        this.text = "Deal 1 damage to all enemies";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
        this.damage = 1;
    }

    public override void effect(Unit target, Unit source)
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
        this.name = "GroupHeal";
        this.text = "Heal 1 damage to all allies.";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source)
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
        this.name = "HeavyAttack";
        this.text = "Deal 8 Damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
        this.damage = 8;
    }

    public override void effect(Unit target, Unit source)
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
        this.name = "SelfHeal";
        this.text = "Heal 8 damage.";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source)
    {
        target.healSelf(5);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Drone : Ability
{
    public Drone()
    {
        this.name = "Drone";
        this.text = "Give ally Regeneration 3";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source)
    {
        Debug.Log(source.unitName + " used " + this.name + " on " + target.unitName);
        target.applyStatus(StatusType.Health, StatusName.Regeneration, 3, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

