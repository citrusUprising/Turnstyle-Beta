using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1b : MonoBehaviour
{

    public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        script[dialogueVarietyCount]=(new overallDialogue("","","", 
        new dialogueEntry[]{
            new dialogueEntry("",false,"null"),
            new dialogueEntry("",false,"null"),
            new dialogueEntry("",false,"null")
            }

        ));
        dialogueVarietyCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
