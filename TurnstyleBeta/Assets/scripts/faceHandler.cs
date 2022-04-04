using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class faceHandler : MonoBehaviour
{
    public string characterName = "null";
    public string currentFace = "null";
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
    public void changeFace(string name, string exp){
        this.currentFace = exp;
        this.characterName = name;
        Color trans = this.GetComponent<Image>().color;
        Sprite talkFace = this.GetComponent<Image>().sprite;
        if(this.characterName == ""){
            trans.a = 0.0f;
        }else{
            Sprite temp = Resources.Load<Sprite>("TalkSprites/"+characterName+"_"+currentFace); //change file format
            //talkFace = temp;  //commented out until images are implemented
            trans.a = 1.0f;
        }
        this.GetComponent<Image>().sprite = talkFace;
        this.GetComponent<Image>().color = trans;
    }
}
