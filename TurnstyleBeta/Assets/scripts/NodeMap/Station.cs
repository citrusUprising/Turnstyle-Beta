using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Station : MonoBehaviour
{
    public Station[] destinations;
    public Color line;
    public bool hasCombat;
    public int isTutorial;
    public bool hasHardMode;
    public string[] Enemies;
    //All lines that connect to that station
    public bool yellow;
    public bool orange;
    public bool blue;
    public bool pink;
    public bool green;
    public bool red;
    public bool[] lines;
    private GameObject imageObject;
    public Sprite standard;
    public Sprite encounter;
    public Sprite hardEncounter;
    public Sprite destination; 
    public int cutscene = 0;

    // Start is called before the first frame update
    void Start()
    {
        initStation();
        lines = new bool[] {yellow, orange, blue, pink, green, red};
    }

    public void initStation()
    {
        if(hasCombat && cutscene < 1){
            this.GetComponent<Image>().sprite = encounter;
        }else if(cutscene>0){
            this.GetComponent<Image>().sprite = destination;
        }else{
            this.GetComponent<Image>().sprite = standard;
        }
    }

    public void EnableHardMode(){
        if(hasHardMode && !hasCombat){
            hasCombat = true;
            this.GetComponent<Image>().sprite = hardEncounter;
        }
    }

    public void DisableHardMode(){
        if(hasHardMode && hasCombat){
            hasCombat = false;
            this.GetComponent<Image>().sprite = standard;
        }
    }

    public void endCombat(){
        this.GetComponent<Image>().sprite = standard;
        this.hasCombat = false;
        this.hasHardMode = false;
    }
}
