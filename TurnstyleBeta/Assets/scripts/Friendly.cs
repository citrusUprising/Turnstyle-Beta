using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : Unit
{
    public Friendly(string name, StatusName[] immunity, Ability[] abilities, int hp) : base(name, immunity, abilities, hp)
    {

        // this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    // Start is called before the first frame update
    void Start()
    {
         gameObject.tag = "Ally";
         this.allies = GameObject.FindGameObjectsWithTag("Ally");
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    void getTeams()
    {
        this.allies = GameObject.FindGameObjectsWithTag("Ally");
        // this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    
}
