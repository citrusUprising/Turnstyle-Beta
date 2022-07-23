using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStats : MonoBehaviour
{
    public int BeverlyHealth;
    public int JadeHealth;
    public int KoralieHealth;
    public int SeraphimHealth;
    public int AmeryHealth;
    public string[] CurrentEnemies;
    public bool AmeryGone;
    public bool BeverlyGone;
    public bool KoralieGone;
    public bool JadeGone;
    public bool SeraphimGone;
    public int[] BeverlyAbilities = new int[3];
    public int[] JadeAbilities = new int[3];
    public int[] KoralieAbilities = new int[3];
    public int[] SeraphimAbilities = new int[3];
    public int[] AmeryAbilities = new int[3];
    public int isTutorial;
    public int currentTutorial;
    public bool hasMoney;
    public int currentDay;
    public int currentCutScene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){

    }
}
