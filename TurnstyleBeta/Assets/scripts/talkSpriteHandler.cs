using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talkSpriteHandler : MonoBehaviour
{
    public string currentName = "null";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeIdle(){
        Color temp = this.GetComponent<Image>().color;
		temp = new Color (0.5f,0.5f,0.5f,temp.a);
		this.GetComponent<Image>().color =temp;
    }

    public void makeActive(){
        Color temp = this.GetComponent<Image>().color;
		temp = new Color (1f,1f,1f,temp.a);
		this.GetComponent<Image>().color =temp;
    }
    public void changeCharacter(string name){
        this.currentName = name;
        Color trans = this.GetComponent<Image>().color;
        Sprite talker = this.GetComponent<Image>().sprite;
        if(this.currentName == ""){
            trans.a = 0.0f;
        }else{
            Sprite temp = (Sprite) Resources.Load ("Assets/sprites/TalkSprites/Battle_"+name+" PNG.png");
            talker = temp;
            trans.a = 1.0f;
        }
        this.GetComponent<Image>().sprite = talker;
        this.GetComponent<Image>().color = trans;
    }
}
