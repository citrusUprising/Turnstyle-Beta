using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueEntry{
	public string line;
	public bool speaker; // = false if left speaker, true if right

	public dialogueEntry(string line, bool speaker){
		this.line = line;
		this.speaker = speaker;
	}
}

public class overallDialogue{
	public string speakerA;
	public string speakerB;
	public dialogueEntry[] lines;

	public overallDialogue(string speakerA, string speakerB, dialogueEntry[] lines){
		this.speakerA = speakerA;
		this.speakerB = speakerB;
		this.lines = lines;
	}
}

public class dialogueEngine : MonoBehaviour
{
	int currentLine;
	overallDialogue chosenDialogue;
	public GameObject mainBox;
	public GameObject rightSprite;
	public GameObject leftSprite;
	TextMeshProUGUI mainBoxText;
	overallDialogue[] dialogueVarieties;
	bool writing;
    // Start is called before the first frame update
    void Start()
    {
    	dialogueVarieties = new overallDialogue[1];
    	dialogueVarieties[0] = new overallDialogue("test", "test", new dialogueEntry[]{new dialogueEntry("this is the first line", false),
    		 new dialogueEntry("this is the second line", true), new dialogueEntry("this is the third line", false)});
        mainBoxText = mainBox.GetComponent<TextMeshProUGUI>();
        //Need to figure out how to take input
        int dialogueChoice = 0;
        chosenDialogue = dialogueVarieties[dialogueChoice];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)){
        	
        	if(writing){
        		//if they hit the button again while text is writing, write all of it
        		StopCoroutine("WriteLine");
        		mainBoxText.text = chosenDialogue.lines[currentLine].line;
        		currentLine += 1;
        		writing = false;
        	}
        	else if(currentLine == chosenDialogue.lines.Length && !writing){
        		//return to proper scene
        	}
        	else{
        		if(chosenDialogue.lines[currentLine].speaker){
        			//left sprite on right sprite off
        		}
        		else{
        			//right sprite off left sprite on
        		}
        		StartCoroutine("WriteLine");
        	}
        }
    }

    public IEnumerator WriteLine(){
    	writing = true;
    	dialogueEntry currentEntry = chosenDialogue.lines[currentLine];
    	for(int i = 1; i < currentEntry.line.Length; i++){
    		mainBoxText.text = currentEntry.line.Remove(i);
    		yield return new WaitForSeconds(.1f);
    	}
    	mainBoxText.text = currentEntry.line;
    	currentLine += 1;
    	writing = false;
    }
}
