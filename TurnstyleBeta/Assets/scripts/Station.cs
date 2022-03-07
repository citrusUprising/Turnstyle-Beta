using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    public Station[] destinations;
    public Color line;
    public bool hasCombat;
    public bool hasHardMode;
    public string[] Enemies;
    private GameObject imageObject;
    public Sprite standard;
    public Sprite encounter;
    public Sprite hardEncounter;
    public int cutscene = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(hasCombat){
            this.GetComponent<Image>().sprite = encounter;
        }else{
            this.GetComponent<Image>().sprite = standard;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableHardMode(){
        if(hasHardMode && !hasCombat) hasCombat = true;
        this.GetComponent<Image>().sprite = hardEncounter;
    }

    public void endCombat(){
        this.GetComponent<Image>().sprite = standard;
        this.hasCombat = false;
    }
}
