using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friendly : Unit
{
    public Friendly(string name, StatusName[] immunity, Ability[] abilities, int hp) : base(name, immunity, abilities, hp)
    {


    }

    public Image sprite;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Ally";
        this.allies = GameObject.FindGameObjectsWithTag("Ally");
        this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        this.allies = GameObject.FindGameObjectsWithTag("Ally");
        // this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }


}
