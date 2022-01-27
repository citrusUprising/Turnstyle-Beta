using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public string name;
    public string text;
    public bool multitarget;
    public bool selftarget;
    public bool allies;

    public abstract void effect(Unit target, Unit source);

    public abstract bool requirment(); 
}

public class basicAttack : Ability
{
    public basicAttack() {
        this.name = "BasicAttack";
        this.text = "Deal 3 damage.";
        this.multitarget = false;
        this.selftarget = true;
        this. allies = false;
    }

    public override void effect(Unit target, Unit source)
    {
        target.takeDamage(source, 3); 
    }

    public override bool requirment()
    {
        return true;
    }

}