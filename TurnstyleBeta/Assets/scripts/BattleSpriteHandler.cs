using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpriteHandler : MonoBehaviour
{
    public GameObject parent;
    public GameObject profile;
    private Color shade;
    public GameObject controller;

    // Start is called before the first frame update
    void Start()
    {
        shade = this.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        //changes color based on dead
        if (profile.GetComponent<nameTag>().character.GetComponent<Unit>().dead){
            shade = new Color (0.5f,0.5f,0.5f);
        }

        //reverses rotation from pentagon and skew
        var trans = this.GetComponent<Transform>();
        Vector3 rot = trans.rotation.eulerAngles;
        rot = new Vector3 (0.0f,0.0f,0.0f);
        trans.rotation = Quaternion.Euler(rot);

        //changes layer and color based on location on pentagon 
        nameTag[] players = controller.GetComponent<combatController>().nameTagArray;
        if(players[2].name == profile.name){
            this.GetComponent<Canvas>().sortingOrder = 3;
            shade.a = 1;
        }else if(players[3].name == profile.name){
            this.GetComponent<Canvas>().sortingOrder = 2;
            shade.a = 0.5f;
        }else if(players[1].name == profile.name){
            this.GetComponent<Canvas>().sortingOrder = 2;
            shade.a = 1.0f;
        }else if(players[0].name == profile.name){
            this.GetComponent<Canvas>().sortingOrder = 1;
            shade.a = 1.0f;
        }else{
            this.GetComponent<Canvas>().sortingOrder = 1;
            shade.a = 0.5f;
        }

        this.GetComponent<Image>().color = shade;
    }
}
