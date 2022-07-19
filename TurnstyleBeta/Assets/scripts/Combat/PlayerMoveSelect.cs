using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoveSelect : MonoBehaviour
{
    // object to write on 
    public GameObject moveSelect;

    // 🧍‍♀️ character object
    public GameObject Player;

    //  🖼 player menu image
    public Sprite playerImg;
    private Image menuImg;

    // 📛 text
    private GameObject NameTxt;

    // 🏃‍♀️ move names
    private GameObject move1;
    private GameObject move2;
    private GameObject move3;
    private GameObject moveDesc;
    
    void Start(){
        // getting move select gameobjects
        menuImg = moveSelect.GetComponent<Image>();
        NameTxt = moveSelect.transform.GetChild(0).gameObject;
        move1 = moveSelect.transform.GetChild(2).gameObject;
        move2 = moveSelect.transform.GetChild(3).gameObject;
        move3 = moveSelect.transform.GetChild(4).gameObject;
        moveDesc = moveSelect.transform.GetChild(1).gameObject;
    }
    public void ChangeColor(){
        menuImg.sprite = playerImg;
        NameTxt.GetComponent<TextMeshProUGUI>().text = Player.GetComponent<Friendly>().name;
        move1.GetComponent<TextMeshProUGUI>().text = Player.GetComponent<Friendly>().abilities[0].name;
        move2.GetComponent<TextMeshProUGUI>().text = Player.GetComponent<Friendly>().abilities[1].name;
        move3.GetComponent<TextMeshProUGUI>().text = Player.GetComponent<Friendly>().abilities[2].name;    
        moveDesc.GetComponent<TextMeshProUGUI>().text = Player.GetComponent<Friendly>().abilities[0].text;
    }
}
