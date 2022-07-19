using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class stationDataHolder{
    public String[] monsters;
    public bool combat;
    public bool hardMode;
    public int cutscene;

    public stationDataHolder(){
        this.monsters = new String[0];
        this.combat = false;
        this.hardMode = false;
        this.cutscene = 0;
    }

    public stationDataHolder (int cutscene){
        this.monsters = new String[0];
        this.combat = false;
        this.hardMode = false;
        this.cutscene = cutscene;
    }

    public stationDataHolder (String[] monsters, bool combat){
        this.monsters = monsters;
        this.combat = combat;
        this.hardMode = !combat;
        this.cutscene = 0;
    }

    public stationDataHolder (String[] monsters, bool combat, int cutscene){
        this.monsters = monsters;
        this.combat = combat;
        this.hardMode = !combat;
        this.cutscene = cutscene;
    }
}