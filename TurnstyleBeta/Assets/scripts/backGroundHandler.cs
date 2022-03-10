using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backGroundHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentBackground = "null";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string changeBackground(string name){
        this.currentBackground = name;
        Sprite temp = Resources.Load<Sprite>("Backgrounds/"+name);
        this.GetComponent<Image>().sprite = temp;
        return name;
    }
}
