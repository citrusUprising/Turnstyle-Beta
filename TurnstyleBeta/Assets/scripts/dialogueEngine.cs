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
	public string faceL; 
	public string faceR; 
	public string music;

	public dialogueEntry(string line, bool speaker, string music){
		this.line = line;
		this.speaker = speaker;
		this.music = music;
		this.faceL = "happy";
		this.faceR = "happy";
	}

	public dialogueEntry(string line, bool speaker, string faceL, string faceR, string music){
		this.line = line;
		this.speaker = speaker;
		this.faceL = faceL;
		this.faceR = faceR;
		this.music = music;
	}
}

public class overallDialogue{
	public string speakerA;
	public string speakerB;
	public string faceUnactive;
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
	public GameObject leftFace;
	public GameObject rightFace;
	public GameObject leftName;
	public GameObject rightName;
	public GameObject backGround;
	TextMeshProUGUI mainBoxText;
	overallDialogue[] dialogueVarieties;
	public GameObject PlayScripts;
	bool writing;
	private int dialogueChoice = 0;

	private float textSpeed;

	public GameObject pauseMenu;
	private GameObject pauseMenuObject;

	public Canvas canvas;

    // Start is called before the first frame update
	void Start()
    {

		textSpeed = PlayerPrefs.GetFloat("dialogueTextSpeed", .055f);

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
			break;
			case 2:
			dialogueVarieties = PlayScripts.GetComponent<Script1c>().script;
			break;
			case 3:
			dialogueVarieties = PlayScripts.GetComponent<Script1d>().script;
			break; 
		}
        chosenDialogue = dialogueVarieties[dialogueChoice];//flag

		this.leftSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerA);
		this.rightSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerB);
		this.leftFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerA,"");
		this.rightFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerB,"");
		this.backGround.GetComponent<backGroundHandler>().changeBackground(chosenDialogue.background);
        leftName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerA;
        rightName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerB;
    }

    // Update is called once per frame
    void Update()
    {
		if (pauseMenuObject == null)
        {
			//skip dialogue (Debugging)
			if (Input.GetKeyDown(KeyCode.S))
			{
				GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene++;
				dialogueChoice = 0;
				SceneManager.UnloadSceneAsync(sceneName);
			}

			if (Input.GetKeyDown(KeyCode.X))
			{

				if (writing)
				{
					//if they hit the button again while text is writing, write all of it
					StopCoroutine("WriteLine");
					mainBoxText.text = chosenDialogue.lines[currentLine].line;
					currentLine += 1;
					writing = false;
				}
				else if (currentLine == chosenDialogue.lines.Length && !writing && dialogueChoice < dialogueVarieties.Length)
				{
					Debug.Log("dialogueChoice at start is " + dialogueChoice);
					Debug.Log("dialogueVarieties.Length at start is " + dialogueVarieties.Length);
					dialogueChoice++;

					//catch if at end of cutscene
					if (dialogueChoice >= dialogueVarieties.Length)
					{
						GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene++;
						dialogueChoice = 0;
						SceneManager.UnloadSceneAsync(sceneName);
					}
					else
					{

						//change characters
						if (chosenDialogue.speakerA != dialogueVarieties[dialogueChoice].speakerA)
							this.leftSprite.GetComponent<talkSpriteHandler>().changeCharacter(dialogueVarieties[dialogueChoice].speakerA);
						if (chosenDialogue.speakerB != dialogueVarieties[dialogueChoice].speakerB)
							this.rightSprite.GetComponent<talkSpriteHandler>().changeCharacter(dialogueVarieties[dialogueChoice].speakerB);

						//change background
						if (chosenDialogue.background != dialogueVarieties[dialogueChoice].background)
							this.backGround.GetComponent<backGroundHandler>().changeBackground(dialogueVarieties[dialogueChoice].background);

						chosenDialogue = dialogueVarieties[dialogueChoice];
						currentLine = 0;

						this.swapAndSound();
						leftName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerA;
						rightName.GetComponent<TextMeshProUGUI>().text = chosenDialogue.speakerB;

						Debug.Log("dialogueChoice at end is " + dialogueChoice);
						Debug.Log("dialogueVarieties.Length at end is " + dialogueVarieties.Length);
						StartCoroutine("WriteLine");
					}
				}
				else
				{
					this.swapAndSound();
					StartCoroutine("WriteLine");
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape))
            {
				pauseMenuObject = Instantiate(pauseMenu, canvas.transform);
            }
		}
    }

    public IEnumerator WriteLine(){
    	writing = true;
    	dialogueEntry currentEntry = chosenDialogue.lines[currentLine];
    	for(int i = 1; i < currentEntry.line.Length; i++){
    		mainBoxText.text = currentEntry.line.Remove(i);
    		yield return new WaitForSeconds(textSpeed);
    	}
    	mainBoxText.text = currentEntry.line;
    	currentLine += 1;
    	writing = false;
    }

	private void playMusic(string path){
		FMOD.Studio.EventInstance sfxInstance;

		sfxInstance = RuntimeManager.CreateInstance("event:/"+path); 
		sfxInstance.start();
	}

	/* All cutscene sounds
	Damage = Battle/damage
	Buff = Battle/buff
	Alarm clock = Cutscene/alarm
	Footsteps = Cutscene/footsteps
	*/

	private void swapAndSound(){
		if(chosenDialogue.lines[currentLine].speaker){
					leftSprite.GetComponent<talkSpriteHandler>().makeIdle();
					leftFace.GetComponent<faceHandler>().makeIdle();
					leftFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerA,chosenDialogue.lines[currentLine].faceL);
					rightSprite.GetComponent<talkSpriteHandler>().makeActive();
					rightFace.GetComponent<faceHandler>().makeActive();
					rightFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerB,chosenDialogue.lines[currentLine].faceR);
        		}
        		else{
					rightSprite.GetComponent<talkSpriteHandler>().makeIdle();
					rightFace.GetComponent<faceHandler>().makeIdle();
					rightFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerB,chosenDialogue.lines[currentLine].faceR);
					leftSprite.GetComponent<talkSpriteHandler>().makeActive();
					leftFace.GetComponent<faceHandler>().makeActive();				
					leftFace.GetComponent<faceHandler>().changeFace(chosenDialogue.speakerA,chosenDialogue.lines[currentLine].faceL);	
        		}
				if(chosenDialogue.lines[currentLine].music != "null")
				this.playMusic(chosenDialogue.lines[currentLine].music);
	}
}
