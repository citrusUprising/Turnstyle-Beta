using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Enemy(string name, StatusName[] immunity, Ability[] abilities, int hp) : base(name, immunity, abilities, hp)
    {
        
        this.enemies = GameObject.FindGameObjectsWithTag("Ally");
        this.allies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy"; 
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