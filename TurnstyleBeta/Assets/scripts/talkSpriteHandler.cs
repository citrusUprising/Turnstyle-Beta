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
		temp = new Color (0.5f,0.5f,0.5f);
		this.GetComponent<Image>().color =temp;
    }

    public void makeActive(){
        Color temp = this.GetComponent<Image>().color;
		temp = new Color (1f,1f,1f);
		this.GetComponent<Image>().color =temp;
    }
    public string changeCharacter(string name){
        this.currentName = name;
        Sprite temp = (Sprite) Resources.Load ("sprites/TalkSprites/Battle_"+name+" PNG");
        this.GetComponent<Image>().sprite = temp;
        return name;
    }
}
