using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMoveSelect : MonoBehaviour
{
    // public int character;
    // 🧍‍♀️ character objects
    // public GameObject Blue;
    // public GameObject Yellow;
    // public GameObject Pink;
    // public GameObject Green;
    // public GameObject Red;
    // public GameObject moveSelect;

    //  🖼 player menu images
    // public Sprite BlueImg;
    // public Sprite YellowImg;
    // public Sprite PinkImg;
    // public Sprite GreenImg;
    // public Sprite RedImg;
    private Image menuImg;

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
        // menuImg = this.GetComponent<Image>();
        // NameTxt = transform.GetChild(0).gameObject;
        // move1 = transform.GetChild(2).gameObject;
        // move2 = transform.GetChild(3).gameObject;
        // move3 = transform.GetChild(4).gameObject;
        // if(character == 0){
        //     // beverly
        //     menuImg.sprite = BlueImg;
        //     NameTxt.GetComponent<TextMeshProUGUI>().text = Blue.GetComponent<Friendly>().name;
        //     move1.GetComponent<TextMeshProUGUI>().text = "#Girl";
        //     move2.GetComponent<TextMeshProUGUI>().text = ":)";
        //     move3.GetComponent<TextMeshProUGUI>().text = "<3";            
        // }
        // if(character == 1){
        //     // amery
        //     menuImg.sprite = YellowImg;
        //     NameTxt.GetComponent<TextMeshProUGUI>().text = Yellow.GetComponent<Friendly>().name;
        //     move1.GetComponent<TextMeshProUGUI>().text = ":(";
        //     move2.GetComponent<TextMeshProUGUI>().text = "move 2";
        //     move3.GetComponent<TextMeshProUGUI>().text = "peggle 2!";
        // }
        // if(character == 2){
        //     // koralie
        //     menuImg.sprite = PinkImg;
        //     NameTxt.GetComponent<TextMeshProUGUI>().text = Pink.GetComponent<Friendly>().name;
        //     move1.GetComponent<TextMeshProUGUI>().text = "";
        //     move2.GetComponent<TextMeshProUGUI>().text = "";
        //     move3.GetComponent<TextMeshProUGUI>().text = "";
        // }
        // if(character == 3){
        //     // jade
        //     menuImg.sprite = GreenImg;
        //     NameTxt.GetComponent<TextMeshProUGUI>().text = Green.GetComponent<Friendly>().name;
        //     move1.GetComponent<TextMeshProUGUI>().text = "";
        //     move2.GetComponent<TextMeshProUGUI>().text = "";
        //     move3.GetComponent<TextMeshProUGUI>().text = "";
        // }
        // if(character == 4){
        //     // seraphim
        //     menuImg.sprite = RedImg;
        //     NameTxt.GetComponent<TextMeshProUGUI>().text = Red.GetComponent<Friendly>().name;
        //     move1.GetComponent<TextMeshProUGUI>().text = "";
        //     move2.GetComponent<TextMeshProUGUI>().text = "";
        //     move3.GetComponent<TextMeshProUGUI>().text = "";
        // }
        //if [current player] is [x]
            // change color
            // change to correct ability list w/ descriptors
        // NameTxt.GetComponent<TextMeshProUGUI>().text = nameTag        
    }
}
