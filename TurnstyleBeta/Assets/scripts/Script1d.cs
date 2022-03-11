using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Script1d : MonoBehaviour
{

     public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=(new overallDialogue("","","", 
        new dialogueEntry[]{
            new dialogueEntry("",false,"null"),
            new dialogueEntry("",false,"null"),
            new dialogueEntry("",false,"null")
            }

        ));
        dialogueVarietyCount++;

        script= new overallDialogue[dialogueVarietyCount];
        for(int i =0; i<dialogueVarietyCount;i++){
            script[i] = temp[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
