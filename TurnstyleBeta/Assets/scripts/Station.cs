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

    public Sprite encounter;
    public Sprite hardEncounter;

    // Start is called before the first frame update
    void Start()
    {
        imageObject = this.transform.GetChild(0).gameObject;
        if(hasCombat){
            imageObject.GetComponent<Image>().sprite = encounter;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableHardMode(){
        if(hasHardMode && !hasCombat) hasCombat = true;
        imageObject.GetComponent<Image>().sprite = hardEncounter;
    }

    public void endCombat(){
        imageObject.GetComponent<Image>().color = new Color (1,1,1,0);
        this.hasCombat = false;
    }
}
