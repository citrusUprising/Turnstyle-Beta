using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    string name;
    string text;
    bool multitarget;
    bool selftarget;
    bool allies;

    public abstract void effect();

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

    public void effect(Unit target, Unit source)
    {
        target.takeDamage(source, 3); 
    }


}