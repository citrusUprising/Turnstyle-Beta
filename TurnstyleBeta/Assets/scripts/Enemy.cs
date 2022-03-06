using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : Unit
{

    private float hpPercent;

    public GameObject healthBar;

    public Enemy(string name, StatusName[] immunity, Ability[] abilities, int hp) : base(name, immunity, abilities, hp)
    {
        

    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy";         
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
        this.abilities = new Ability[] { new BasicAttack(), new HeavyAttack(), new BasicHeal() };
        Status health = new Status(StatusType.Health, StatusName.None, 0, 0);
        Status buff = new Status(StatusType.Buff, StatusName.None, 0, 0);
        Status debuff = new Status(StatusType.Debuff, StatusName.None, 0, 0);
        this.statuses = new Status[] { health, buff, debuff };
        var rend = this.GetComponent<Image>();
        Color a;
        
        switch (this.unitName){

            case "Lethargy": case "Lethargy A": case "Lethargy B": case "Lethargy C": //Basic
            this.hp = 15;
            this.maxHP = 15;
            this.abilities = new Ability[] { new BasicAttack(), new SweepingStrike(), new Crush() };
            a = new Color(0.95f,1f,0.5f,1);
            break;

            case "Frailty": case "Frailty A": case "Frailty B": case "Frailty C": //Fragile
            this.hp = 7;
            this.maxHP = 7;
            this.abilities = new Ability[] { new Embolden(), new OverExtend(), new SelfDestruct() };
            a = new Color(0.5f,0.8f,1,1);
            break;

            case "Refusal": case "Refusal A": case "Refusal B": case "Refusal C": //Beefy
            this.hp = 20;
            this.maxHP = 20;
            this.abilities = new Ability[] { new Hunker(), new SweepingStrike(), new Slump() };
            a = new Color(1,0.5f,0.4f,1);
            break;

            case "Enabling": case "Enabling A": case "Enabling B": case "Enabling C": //Support
            this.hp = 10;
            this.maxHP = 10;
            this.abilities = new Ability[] { new Reinforce(), new Accelerate(), new Habituate() };
            a = new Color(1,0.6f,0.8f,1);
            break;

            case "Doubt": case "Doubt A": case "Doubt B": case "Doubt C": //Trickster
            this.hp = 12;
            this.maxHP = 12;
            this.abilities = new Ability[] { new Inhibit(), new Venom(), new Withhold() };
            a = new Color(0.4f,0.9f,0.5f,1);
            break;

            case "Appeasement": case "Appeasement A": case "Appeasement B": case "Appeasement C": //Yellow
            this.hp = 15;
            this.maxHP = 15;
            this.abilities = new Ability[] { new FlamingRing(), new Tackle(), new Repel() };
            a = new Color(0.7f,0.8f,0.1f,1);
            break;

            case "Spite": case "Spite A": case "Spite B": case "Spite C": //Red
            this.hp = 20;
            this.maxHP = 20;
            this.immunity = new StatusName[] {StatusName.Vulnerable};
            this.abilities = new Ability[] { new Unionize(), new Penetrate(), new Ridicule() };
            a = new Color(0.5f,0.15f,0.15f,1);
            break;

            case "Reflection": case "Reflection A": case "Reflection B": case "Reflection C": //Blue
            this.hp = 25;
            this.maxHP = 25;
            this.immunity = new StatusName[] {StatusName.Flinch};
            this.abilities = new Ability[] { new PollenCloud(), new Reflect(), new Ingrain() };
            a = new Color(0,0.3f,0.4f,1);
            break;

            case "Sacrifice": case "Sacrifice A": case "Sacrifice B": case "Sacrifice C": //Green
            this.hp = 16;
            this.maxHP = 16;
            this.immunity = new StatusName[] {StatusName.Weakened};
            this.abilities = new Ability[] { new Repel(), new Flagellate(), new Withhold() };
            a = new Color(0.15f,0.35f,0.15f,1);
            break;

            case "Panic": case "Panic A": case "Panic B": case "Panic C": //Pink
            this.hp = 19;
            this.maxHP = 19;
            this.abilities = new Ability[] { new Cleave(), new Startle(), new Enrage() };
            a = new Color(0.7f,0.1f,0.4f);
            break;

            case "Null":
            this.hp = 0;
            this.maxHP = 0;
            this.dead = true;
            this.abilities = new Ability[] {};
            a = new Color(0,0,0,0);
            break;

            default:
            a = new Color(1,1,1,1);
            break;

        }
        rend.color=a;
    }

    // Update is called once per frame
    void Update()
    {
        hpPercent = (float)hp / (float)maxHP;

        healthBar.GetComponent<Image>().fillAmount = hpPercent;
    }


    void getTeams()
    {
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
