using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class Enemy : Unit
{

    private float hpPercent;

    public GameObject healthBar;
    public GameObject healthBarContainer;

    private Color transparentColor = new Color(0, 0, 0, 0);

    public GameObject healthStatusTracker;
    public GameObject buffStatusTracker;
    public GameObject debuffStatusTracker;
   
    public int speed;
    private int baseSpeed;

    Random rand;

    public enemySpeedController speedController;

    public Enemy(string name, StatusName[] immunity, Ability[] abilities, int hp) : base(name, immunity, abilities, hp)
    {
        

    }
    // Start is called before the first frame update
    void Start()
    {
        rand = new Random();

        gameObject.tag = "Enemy";         
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
        this.abilities = new Ability[] { new BasicAttack(), new HeavyAttack(), new BasicHeal() };
        Status health = new Status(StatusType.Health, StatusName.None, 0, 0);
        Status buff = new Status(StatusType.Buff, StatusName.None, 0, 0);
        Status debuff = new Status(StatusType.Debuff, StatusName.None, 0, 0);
        this.statuses = new Status[] { health, buff, debuff };
        var rend = this.GetComponent<Image>();
        Color a = new Color(1f,1f,1f,1f);
        Sprite icon;

        setSpeed();
        
        switch (this.unitName){

            case "Lethargy": case "Lethargy A": case "Lethargy B": case "Lethargy C": //Basic
            this.hp = 18;
            this.maxHP = 18;
            this.baseSpeed = 2;
            this.abilities = new Ability[] { new BasicAttack(), new SweepingStrike(), new Crush() };
            icon = Resources.Load<Sprite>("MonsterSprites/Basic");
            rend.sprite=icon;
            break;

            case "Frailty": case "Frailty A": case "Frailty B": case "Frailty C": //Fragile
            this.hp = 9;
            this.maxHP = 9;
            this.baseSpeed = 6;
            this.abilities = new Ability[] { new Embolden(), new OverExtend(), new SelfDestruct() };
            icon = Resources.Load<Sprite>("MonsterSprites/Fragile");
            rend.sprite=icon;
            break;

            case "Refusal": case "Refusal A": case "Refusal B": case "Refusal C": //Beefy
            this.hp = 25;
            this.maxHP = 25;
            this.baseSpeed = 0;
            this.abilities = new Ability[] { new Hunker(), new SweepingStrike(), new Slump() };
            icon = Resources.Load<Sprite>("MonsterSprites/Beefy");
            rend.sprite=icon;
            break;

            case "Enabling": case "Enabling A": case "Enabling B": case "Enabling C": //Support
            this.hp = 14;
            this.maxHP = 14;
            this.baseSpeed = 3;
            this.abilities = new Ability[] { new Reinforce(), new Accelerate(), new Habituate() };
            icon = Resources.Load<Sprite>("MonsterSprites/Support");
            rend.sprite=icon;
            break;

            case "Doubt": case "Doubt A": case "Doubt B": case "Doubt C": //Trickster
            this.hp = 16;
            this.maxHP = 16;
            this.baseSpeed = 4;
            this.abilities = new Ability[] { new Inhibit(), new Venom(), new Withhold() };
            icon = Resources.Load<Sprite>("MonsterSprites/Trickster");
            rend.sprite=icon;
            break;

            case "Appeasement": case "Appeasement A": case "Appeasement B": case "Appeasement C": //Yellow
            this.hp = 20;
            this.maxHP = 20;
            this.baseSpeed = 3;
            this.abilities = new Ability[] { new FlamingRing(), new Tackle(), new Repel() };
            icon = Resources.Load<Sprite>("MonsterSprites/Yellow");
            rend.sprite=icon;
            break;

            case "Spite": case "Spite A": case "Spite B": case "Spite C": //Red
            this.hp = 25;
            this.maxHP = 25;
            this.baseSpeed = 4;
            this.immunity = new StatusName[] {StatusName.Vulnerable};
            this.abilities = new Ability[] { new Ingrain(), new Penetrate(), new Ridicule() };
            icon = Resources.Load<Sprite>("MonsterSprites/Red");
            rend.sprite=icon;
            break;

            case "Reflection": case "Reflection A": case "Reflection B": case "Reflection C": //Blue
            this.hp = 30;
            this.maxHP = 30;
            this.baseSpeed = 0;
            this.immunity = new StatusName[] {StatusName.Flinch};
            this.abilities = new Ability[] { new PollenCloud(), new Reflect(), new Unionize() };
            icon = Resources.Load<Sprite>("MonsterSprites/Blue");
            rend.sprite=icon;
            break;

            case "Sacrifice": case "Sacrifice A": case "Sacrifice B": case "Sacrifice C": //Green
            this.hp = 18;
            this.maxHP = 18;
            this.baseSpeed = 6;
            this.immunity = new StatusName[] {StatusName.Weakened};
            this.abilities = new Ability[] { new Repel(), new Flagellate(), new Withhold() };
            icon = Resources.Load<Sprite>("MonsterSprites/Green");
            rend.sprite=icon;
            break;

            case "Panic": case "Panic A": case "Panic B": case "Panic C": //Pink
            this.hp = 22;
            this.maxHP = 22;
            this.baseSpeed = 4;
            this.abilities = new Ability[] { new Cleave(), new Startle(), new Enrage() };
            icon = Resources.Load<Sprite>("MonsterSprites/Pink");
            rend.sprite=icon;
            break;

            case "Null":
            this.hp = 0;
            this.maxHP = 1;
            this.baseSpeed = 0;
            this.dead = true;
            this.abilities = new Ability[] {};
            a = new Color(0,0,0,0);
            break;

            default:
            a = new Color(1,1,1,1);
            break;

        }
        rend.color=a;
        this.updateHealthBar();
        this.Kill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealthBar(){
        hpPercent = (float)hp / (float)maxHP;

        healthBar.GetComponent<Image>().fillAmount = hpPercent;

        if (gameObject.GetComponent<CanvasRenderer>().GetAlpha() == 0)
        {
            healthBar.GetComponent<Image>().color = transparentColor;
            healthBarContainer.GetComponent<Image>().color = transparentColor;
            healthBarContainer.SetActive(false);
        }

        if (gameObject.GetComponent<Image>().color.a == 0)
        {
            healthBar.GetComponent<Image>().color = transparentColor;
            healthBarContainer.GetComponent<Image>().color = transparentColor;
            healthBarContainer.SetActive(false);
        }
    }


    void getTeams()
    {
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void updateAllStatuses()
    {
        healthStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)GetComponent<Unit>().statuses[(int)StatusType.Health].name,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Health].duration,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Health].magnitude);

        buffStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)GetComponent<Unit>().statuses[(int)StatusType.Buff].name,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Buff].duration,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Buff].magnitude);

        debuffStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)GetComponent<Unit>().statuses[(int)StatusType.Debuff].name,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Debuff].duration,
            (int)GetComponent<Unit>().statuses[(int)StatusType.Debuff].magnitude);
    }

    public int setSpeed()
    {
        speed = rand.Next(3);

        speedController.speed = baseSpeed + speed;

        speedController.updateSpeed();

        return (baseSpeed + speed);
    }
}
