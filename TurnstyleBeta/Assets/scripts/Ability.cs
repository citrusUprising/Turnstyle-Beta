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
        this.text = "Deal 3 damage to a target";
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
        this.text = "Heal 3 damage from ally.";
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
        this.text = "Heal 1 damage from all allies.";
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
        this.text = "Deal 7 Damage to a target";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
        this.damage = 7;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 7);
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
        this.text = "Heal 8 damage from user";
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
        this.text = "Give ally Regen (2) for 4 turns";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Health, StatusName.Regeneration, 4, 2);
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
        this.text = "Cure an ally of debuffs, then inflict them with Null for 3 turns";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
  if(target.statuses[(int) StatusType.Debuff].name != StatusName.None){
    L.outputQueue.Add(new displayObject(target.unitName+" was cured of "+target.statuses[(int) StatusType.Debuff].name,
    target,
    target.statuses[(int) StatusType.Debuff].name,
    false,
    "buff")
    );
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
        this.text = "Hit enemy with fire, damaging them based on user's speed";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
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
        this.text = "Give self Haste (3) and Weakened for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        //L.outputQueue.Add(new displayObject(source.unitName +" imbibed coffee",false));
        target.applyStatus(StatusType.Buff,StatusName.Haste, 2, 3);
        target.applyStatus(StatusType.Debuff,StatusName.Weakened, 2, 0);
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
        this.text = "Hit all enemies for 2 damage";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
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
        this.text = "Give self Vulnerable and all allies Shielded for 1 turn.";//flag
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
    if(target.unitName != source.unitName)
        target.applyStatus(StatusType.Buff,StatusName.Shielded, 1, 0);
    else
    source.applyStatus(StatusType.Debuff,StatusName.Vulnerable, 1, 0);
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
        this.text = "Deal 8 damage to a target and 6 damage to self";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 8);
        source.takeDamage(source, 6);
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
        this.text = "Heal an ally for 6 damage and deal 3 damage to user";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        source.hp = Math.Min(source.hp-3, source.maxHP);
        L.outputQueue.Add(new displayObject(source.unitName+" sacrificed 3 health",
        source,
        3,
        true)
        );
        target.healSelf(6);
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
        this.text = "Deal 2 damage to targeted enemy and inflict Weakened for 1 turn";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source,2);
        target.applyStatus(StatusType.Debuff,StatusName.Weakened, 1, 0);
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
        this.text = "Deal 10 damage. -33% Accuracy for each level of fatigue";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {   double test = 1-0.33*source.fatigue;
        float check = UnityEngine.Random.Range(0.0f,1.0f);
        Debug.Log("Seraphim's accuracy is "+test);
        Debug.Log("Seraphim rolled "+ check);
        if(source.fatigue <= 0 || check <= test) target.takeDamage(source, 10);
        else L.outputQueue.Add(new displayObject(source.unitName+" couldn't manifest",
        false)
        );
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Soulrip2 : Ability
{
    public Soulrip2()
    {
        this.name = "Soul Rip";
        this.text = "Deal 10 damage, but has 50% accuracy if at 1 or more fatigue and gives user Vulnerable for 1 turn";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {   double test = 1;
        if(source.fatigue > 0)test -= 0.5d;
        float check = UnityEngine.Random.Range(0.0f,1.0f);
        Debug.Log("Seraphim's accuracy is "+test);
        Debug.Log("Seraphim rolled "+ check);
        if(source.fatigue <= 0 || check <= test) target.takeDamage(source, 10);
        else L.outputQueue.Add(new displayObject(source.unitName+" couldn't manifest",
        false));
        source.applyStatus(StatusType.Debuff, StatusName.Vulnerable, 1, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Soulrip3 : Ability
{
    public Soulrip3()
    {
        this.name = "Soul Rip";
        this.text = "Deal 8 damage, but gives target Regen(6) for 1 turn";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {   
        target.takeDamage(source, 8);
        target.applyStatus(StatusType.Health, StatusName.Regeneration, 1, 6);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Scream : Ability
{
    public Scream()
    {
        this.name = "Scream";
        this.text = "Deal 10 damage to all enemies with 33% accuracy";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {   
        double test = 0.33;
        float check = UnityEngine.Random.Range(0.0f,1.0f);
        Debug.Log("Seraphim's accuracy is "+test);
        Debug.Log("Seraphim rolled "+ check);
        if(check <= test) target.takeDamage(source, 10);
        else L.outputQueue.Add(new displayObject(target.unitName+" is unconcerned",
        false));
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Scream2 : Ability
{
    public Scream2()
    {
        this.name = "Scream";
        this.text = "Deal 2x user's fatigue in damage to all enemies with 50% accuracy";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {   
        double test = 0.5;
        float check = UnityEngine.Random.Range(0.0f,1.0f);
        Debug.Log("Seraphim's accuracy is "+test);
        Debug.Log("Seraphim rolled "+ check);
        if(check <= test) target.takeDamage(source, source.fatigue*2);
        else L.outputQueue.Add(new displayObject(target.unitName+" is unconcerned",
        false));
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
        float check1 = UnityEngine.Random.Range(0.0f,1.0f);
        float check2 = UnityEngine.Random.Range(0.0f,1.0f);
        if(check1 <= 0.35){
            target.applyStatus(StatusType.Debuff,StatusName.Flinch, 1, 0);
        }else if(check2 <= 0.35){
            target.applyStatus(StatusType.Health, StatusName.Burn, 2, 4);
        }else L.outputQueue.Add(new displayObject(target.unitName+" avoided the "+this.name,
        false)
        );
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
        this.text = "Afflicts one targeted enemy with Vulnerable for 2 turns.";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Debuff,StatusName.Vulnerable, 2, 0);
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
        this.text = "Gives an ally Strengthened for 1 turn, Shielded for 1 turn, or Haste (3) for 2 turns.";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        float rng = UnityEngine.Random.Range(0.0f,1.0f);
        if(rng <= 0.33){
            target.applyStatus(StatusType.Buff,StatusName.Shielded, 1,0);
        }
        else if(rng <= 0.66){
            target.applyStatus(StatusType.Buff, StatusName.Strengthened, 1,0);
        }
        else {
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
        this.text = "Grants user Regen (6) for 1 turn";
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
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
        this.text = "Grants self Shielded for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Buff,StatusName.Shielded, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class FlamingRing : Ability
{
    public FlamingRing()
    {
        this.name = "Flaming Ring";
        this.text = "Inflict Target with either Burn (8) for 1 turn or Burn (2) for 5 turns";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        float rng = UnityEngine.Random.Range(0.0f,1.0f);
        if(rng < 0.5){
            target.applyStatus(StatusType.Health,StatusName.Burn,1,8);
        }else{
            target.applyStatus(StatusType.Health,StatusName.Burn,5,2);
        }
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Tackle : Ability
{
    public Tackle()
    {
        this.name = "Tackle";
        this.text = "Deal 6 damage to a target";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 6);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class PollenCloud : Ability
{
    public PollenCloud()
    {
        this.name = "Pollen Cloud";
        this.text = "Deal 1 damage to all enemies. 50% to inflict Fatigue Up";//flag
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 1);
        float rng = UnityEngine.Random.Range(0.0f,1.0f);
        if(rng > 0.5)target.applyStatus(StatusType.Debuff,StatusName.FatigueUP, 100, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Reflect : Ability
{
    public Reflect()
    {
        this.name = "Reflect";
        this.text = "Deals damage to a target equal to half the user's accrued damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        int damage;
        damage = (source.maxHP-source.hp)/2;
        target.takeDamage(source, damage);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Ingrain : Ability
{
    public Ingrain()
    {
        this.name = "Persist";
        this.text = "50% chance give all allies Regen (1) for 5 turns";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        float rng = UnityEngine.Random.Range(0.0f,1.0f);
        if(rng>0.5)target.applyStatus(StatusType.Health,StatusName.Regeneration, 5, 1);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Unionize : Ability
{
    public Unionize()
    {
        this.name = "Unionize";
        this.text = "Grants party Shielded for 1 turn";//flag
        this.multitarget = true;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Buff,StatusName.Shielded,1,0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Penetrate : Ability
{
    public Penetrate()
    {
        this.name = "Penetrate";
        this.text = "Deal 6 damage to a target, ignoring Buffs and Debuffs. This cannot miss.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.hp = Math.Max(target.hp - 6, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Ridicule : Ability
{
    public Ridicule()
    {
        this.name = "Ridicule";
        this.text = "Deal 2 damage to a target and inflict Weakened for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source,2);
        target.applyStatus(StatusType.Debuff,StatusName.Weakened,2,0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Flagellate : Ability
{
    public Flagellate()
    {
        this.name = "Flagellate";
        this.text = "Deal 2 damage to a target. If the target has a buff, removes the buff. Otherwise deals 2 more damage.";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        if(target.statuses[(int) StatusType.Buff].name != StatusName.None){
            target.takeDamage(source,2);
            L.outputQueue.Add(new displayObject(target.unitName+" was relieved of "+target.statuses[(int) StatusType.Buff].name,
            target,
            target.statuses[(int) StatusType.Buff].name,
            false)
            );
        }else{
            target.takeDamage(source,4);
        }
        target.statuses[(int) StatusType.Buff].name = StatusName.None;
        target.statuses[(int) StatusType.Buff].duration = 0;
        target.statuses[(int) StatusType.Buff].magnitude = 0;
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Withhold : Ability
{
    public Withhold()
    {
        this.name = "Withhold";
        this.text = "Inflict a target with Flinch";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Debuff,StatusName.Flinch, 1, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Cleave : Ability
{
    public Cleave()
    {
        this.name = "Cleave";
        this.text = "Halve a target's health";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.hp = Math.Max(target.hp-target.hp/2, 0);
        L.outputQueue.Add(new displayObject(target.unitName+"'s health was halved",
        target,
        target.hp/2,
        true,
        "damage")
        );
    }
    
    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Startle : Ability
{
    public Startle()
    {
        this.name = "Startle";
        this.text = "50% chance to inflict all enemies with Vulnerable for 2 turns";//flag
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        float rng = UnityEngine.Random.Range(0.0f,1.0f);
        if(rng > 0.5)target.applyStatus(StatusType.Debuff,StatusName.Vulnerable, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Enrage : Ability
{
    public Enrage()
    {
        this.name = "Enrage";
        this.text = "Grant an ally Strengthened for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Buff,StatusName.Strengthened, 2, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class SweepingStrike : Ability
{
    public SweepingStrike()
    {
        this.name = "Sweeping Strike";
        this.text = "Deal 1 damage to all enemies";
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source,1);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Embolden : Ability
{
    public Embolden()
    {
        this.name = "Embolden";
        this.text = "Give self Strengthened for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = true;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Buff, StatusName.Strengthened, 2,0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class OverExtend : Ability
{
    public OverExtend()
    {
        this.name = "Over-Extend";
        this.text = "Hit all enemies for 3 damage, give self Vulnerable for 1 turn";//flag
        this.multitarget = true;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.takeDamage(source, 3);
        source.applyStatus(StatusType.Debuff,StatusName.Vulnerable,1,0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class SelfDestruct : Ability
{
    public SelfDestruct()
    {
        this.name = "Self-Destruct";
        this.text = "Give target and self Burn for 1 turn, magnitude is equal to user's current health";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Health,StatusName.Burn,1,source.hp);
        source.applyStatus(StatusType.Health,StatusName.Burn,1,source.hp);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Reinforce : Ability
{
    public Reinforce()
    {
        this.name = "Reinforce";
        this.text = "Give an ally Regen (1) for 5 turns";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Health,StatusName.Regeneration,5,1);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Accelerate : Ability
{
    public Accelerate()
    {
        this.name = "Accelerate";
        this.text = "Give an ally Haste (2) for 2 turns";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Buff,StatusName.Haste,2,2);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Habituate : Ability
{
    public Habituate()
    {
        this.name = "Habituate";
        this.text = "Cure an ally of debuffs, then inflict them with Null for 1 turn";
        this.multitarget = false;
        this.selftarget = false;
        this.allies = true;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
          if(target.statuses[(int) StatusType.Debuff].name != StatusName.None){
            L.outputQueue.Add(new displayObject(target.unitName+" was cured of "+target.statuses[(int) StatusType.Debuff].name,
            target,
            target.statuses[(int) StatusType.Debuff].name,
            false,
            "buff")
            );
        }
        target.statuses[(int) StatusType.Debuff].name = StatusName.None;
        target.statuses[(int) StatusType.Debuff].duration = 0;
        target.statuses[(int) StatusType.Debuff].magnitude = 0;
        target.applyStatus(StatusType.Debuff, StatusName.Null, 1, 0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Inhibit : Ability
{
    public Inhibit()
    {
        this.name = "Inhibit";
        this.text = "Inflicts target with Weaken for 2 turns";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Debuff,StatusName.Weakened,2,0);
    }

    public override bool requirement(Unit target, Unit source)
    {
        return true;
    }
}

public class Venom : Ability
{
    public Venom()
    {
        this.name = "Venom";
        this.text = "Inflicts target with Vulnerable for 1 turn";//flag
        this.multitarget = false;
        this.selftarget = false;
        this.allies = false;
    }

    public override void effect(Unit target, Unit source, MainLoop L)
    {
        target.applyStatus(StatusType.Debuff,StatusName.Vulnerable,1,0);
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

