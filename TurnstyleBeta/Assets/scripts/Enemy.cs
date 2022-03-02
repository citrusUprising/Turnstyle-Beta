using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
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

            case "Basic": case "Basic A": case "Basic B": case "Basic C":
            this.hp = 15;
            this.maxHP = 15;
            this.abilities = new Ability[] { new BasicAttack(), new Temp14(), new Crush() };
            a = new Color(0.95f,1f,0.5f,1);
            break;

            case "Fragile": case "Fragile A": case "Fragile B": case "Fragile C":
            this.hp = 7;
            this.maxHP = 7;
            this.abilities = new Ability[] { new Temp15(), new Temp16(), new Temp17() };
            a = new Color(0.5f,0.8f,1,1);
            break;

            case "Beefy": case "Beefy A": case "Beefy B": case "Beefy C":
            this.hp = 20;
            this.maxHP = 20;
            this.abilities = new Ability[] { new Hunker(), new Temp14(), new Slump() };
            a = new Color(1,0.5f,0.4f,1);
            break;

            case "Support": case "Support A": case "Support B": case "Support C":
            this.hp = 10;
            this.maxHP = 10;
            this.abilities = new Ability[] { new Temp18(), new Temp19(), new Temp20() };
            a = new Color(1,0.6f,0.8f,1);
            break;

            case "Trickster": case "Trickster A": case "Trickster B": case "Trickster C":
            this.hp = 12;
            this.maxHP = 12;
            this.abilities = new Ability[] { new Temp21(), new Temp22(), new Temp10() };
            a = new Color(0.4f,0.9f,0.5f,1);
            break;

            case "Yellow": case "Yellow A": case "Yellow B": case "Yellow C":
            this.hp = 15;
            this.maxHP = 15;
            this.abilities = new Ability[] { new Temp01(), new Temp02(), new Repel() };
            a = new Color(0.7f,0.8f,0.1f,1);
            break;

            case "Red": case "Red A": case "Red B": case "Red C":
            this.hp = 20;
            this.maxHP = 20;
            this.immunity = new StatusName[] {StatusName.Vulnerable};
            this.abilities = new Ability[] { new Temp06(), new Temp07(), new Temp08() };
            a = new Color(0.5f,0.15f,0.15f,1);
            break;

            case "Blue": case "Blue A": case "Blue B": case "Blue C":
            this.hp = 25;
            this.maxHP = 25;
            this.immunity = new StatusName[] {StatusName.Flinch};
            this.abilities = new Ability[] { new Temp03(), new Temp04(), new Temp05() };
            a = new Color(0,0.3f,0.4f,1);
            break;

            case "Green": case "Green A": case "Green B": case "Green C":
            this.hp = 16;
            this.maxHP = 16;
            this.immunity = new StatusName[] {StatusName.Weakened};
            this.abilities = new Ability[] { new Repel(), new Temp09(), new Temp10() };
            a = new Color(0.15f,0.35f,0.15f,1);
            break;

            case "Pink": case "Pink A": case "Pink B": case "Pink C":
            this.hp = 19;
            this.maxHP = 19;
            this.abilities = new Ability[] { new Temp11(), new Temp12(), new Temp13() };
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

    }


    void getTeams()
    {
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
