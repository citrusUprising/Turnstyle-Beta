using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoveSelect : MonoBehaviour
{
    // public int character;
    public Sprite Blue;
    public Sprite Yellow;
    public Sprite Pink;
    public Sprite Green;
    public Sprite Red;
    private Sprite menuImg;

    // 📛 text
    private GameObject NameTxt;

    // 🏃‍♀️ move names
    private GameObject move1;
    private GameObject move2;
    private GameObject move3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ChangeColor(int character){
        menuImg = this.GetComponent<Image>().sprite;
        NameTxt = transform.GetChild(0).gameObject;
        move1 = transform.GetChild(2).gameObject;
        move2 = transform.GetChild(3).gameObject;
        move3 = transform.GetChild(4).gameObject;
        if(character == 0){
            // beverly
            menuImg = Blue;
            NameTxt.GetComponent<TextMeshProUGUI>().text = "Beverly";
            move1.GetComponent<TextMeshProUGUI>().text = "";
            move2.GetComponent<TextMeshProUGUI>().text = "";
            move3.GetComponent<TextMeshProUGUI>().text = "";
            
        }
        if(character == 1){
            // amery
            menuImg = Yellow;
            NameTxt.GetComponent<TextMeshProUGUI>().text ="Amery";
            move1.GetComponent<TextMeshProUGUI>().text = "";
            move2.GetComponent<TextMeshProUGUI>().text = "";
            move3.GetComponent<TextMeshProUGUI>().text = "";
        }
        if(character == 2){
            // koralie
            menuImg = Pink;
            NameTxt.GetComponent<TextMeshProUGUI>().text = "Koralie";
            move1.GetComponent<TextMeshProUGUI>().text = "";
            move2.GetComponent<TextMeshProUGUI>().text = "";
            move3.GetComponent<TextMeshProUGUI>().text = "";
        }
        if(character == 3){
            // jade
            menuImg = Green;
            NameTxt.GetComponent<TextMeshProUGUI>().text = "Jade";
            move1.GetComponent<TextMeshProUGUI>().text = "";
            move2.GetComponent<TextMeshProUGUI>().text = "";
            move3.GetComponent<TextMeshProUGUI>().text = "";
        }
        if(character == 4){
            // seraphim
            menuImg = Red;
            NameTxt.GetComponent<TextMeshProUGUI>().text = "Seraphim";
            move1.GetComponent<TextMeshProUGUI>().text = "";
            move2.GetComponent<TextMeshProUGUI>().text = "";
            move3.GetComponent<TextMeshProUGUI>().text = "";
        }
        //if [current player] is [x]
            // change color
            // change to correct ability list w/ descriptors
        
    }
}
