using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using TMPro;

public class dialogueEntry{
	public string line;
	public bool speaker; // = false if left speaker, true if right
	public string music;

	public dialogueEntry(string line, bool speaker, string music){
		this.line = line;
		this.speaker = speaker;
		this.music = music;
	}
}

public class overallDialogue{
	public string speakerA;
	public string speakerB;
	public string background;
	public dialogueEntry[] lines;

	public overallDialogue(string speakerA, string speakerB, string background, dialogueEntry[] lines){
		this.speakerA = speakerA;
		this.speakerB = speakerB;
		this.background = background;
		this.lines = lines;
	}
}

public class dialogueEngine : MonoBehaviour
{
	public string sceneName;
	int currentLine = 0;
	private overallDialogue chosenDialogue;
	public GameObject mainBox;
	public GameObject rightSprite;
	public GameObject leftSprite;
	public GameObject leftName;
	public GameObject rightName;
	public GameObject backGround;
	TextMeshProUGUI mainBoxText;
	overallDialogue[] dialogueVarieties;
	public GameObject PlayScripts;
	bool writing;
	private int dialogueChoice = 0;
    // Start is called before the first frame update
	void Start()
    {
    	/*dialogueVarieties = new overallDialogue[1];
    	dialogueVarieties[0] = new overallDialogue("test", "seraphim", new dialogueEntry[]{new dialogueEntry("this is the first line", false),
    		 new dialogueEntry("this is the second line", true), new dialogueEntry("this is the third line", false)});
        //Need to figure out how to take input
        */
		mainBoxText = mainBox.GetComponent<TextMeshProUGUI>();
		switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene){
			case 0: default:
			dialogueVarieties = PlayScripts.GetComponent<Script1a>().script;
			break;
			case 1:
			dialogueVarieties = PlayScripts.GetComponent<Script1b>().script;
			Debug.Log("Choice 1");
			break;
			case 2:
			dialogueVarieties = PlayScripts.GetComponent<Script1c>().script;
			break;
			case 3:
			dialogueVarieties = PlayScripts.GetComponent<Script1d>().script;
			break; 
		}
		dialogueVarieties = PlayScripts.GetComponent<Script1a>().script;
        chosenDialogue = dialogueVarieties[dialogueChoice];//flag
		this.leftSprite.name = 
				this.leftSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerA);
		this.rightSprite.name = 
				this.rightSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerB);
        leftName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerA;
        rightName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerB;
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
        	else if(currentLine == chosenDialogue.lines.Length && !writing && dialogueChoice != dialogueVarieties.Length){
				dialogueChoice++;
				
				//change characters
				if(chosenDialogue.speakerA!= dialogueVarieties[dialogueChoice].speakerA)
				this.leftSprite.name = //flag
				this.leftSprite.GetComponent<talkSpriteHandler>().changeCharacter(dialogueVarieties[dialogueChoice].speakerA);
				if(chosenDialogue.speakerB!= dialogueVarieties[dialogueChoice].speakerB)
				this.rightSprite.name = //flag
				this.rightSprite.GetComponent<talkSpriteHandler>().changeCharacter(dialogueVarieties[dialogueChoice].speakerA);
				
				//change background
				if(chosenDialogue.background != dialogueVarieties[dialogueChoice].background)
				this.backGround.name = //flag
				this.backGround.GetComponent<backGroundHandler>().changeBackground(dialogueVarieties[dialogueChoice].background);
				
				//plays sound effects
				if(chosenDialogue.lines[currentLine].music != "null")
				this.playMusic(chosenDialogue.lines[currentLine].music);

				chosenDialogue = dialogueVarieties[dialogueChoice];

				leftName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerA;
        		rightName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerB;

				currentLine = 0;

				StartCoroutine("WriteLine");
        	}
			else if(dialogueChoice == dialogueVarieties.Length){
				GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene++;
				SceneManager.UnloadSceneAsync(sceneName);
			}
        	else{
        		if(chosenDialogue.lines[currentLine].speaker){
					leftSprite.GetComponent<talkSpriteHandler>().makeIdle();
					rightSprite.GetComponent<talkSpriteHandler>().makeActive();
        		}
        		else{
					rightSprite.GetComponent<talkSpriteHandler>().makeIdle();
					leftSprite.GetComponent<talkSpriteHandler>().makeActive();
        		}
        		StartCoroutine("WriteLine");
				if(chosenDialogue.lines[currentLine].music != "null")
				this.playMusic(chosenDialogue.lines[currentLine].music);
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

	private void playMusic(string path){
		FMOD.Studio.EventInstance sfxInstance;

		sfxInstance = RuntimeManager.CreateInstance("event:/ui/Dialogue/"+path); 
		sfxInstance.start();
	}
}
