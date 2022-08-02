using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTemplate : MonoBehaviour
{

    public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runCode(){

    }

    public overallDialogue[] returnScript(){
        Debug.Log("returning script");
        return this.script;
    }
}