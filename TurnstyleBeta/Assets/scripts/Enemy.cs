using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
