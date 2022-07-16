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
	public string sfx;

	public dialogueEntry(string line, bool speaker, string music, string sfx){
		this.line = line;
		this.speaker = speaker;
		this.music = music;
		this.sfx = sfx;
		this.faceL = "happy";
		this.faceR = "happy";
	}

	public dialogueEntry(string line, bool speaker, string faceL, string faceR, string music, string sfx){
		this.line = line;
		this.speaker = speaker;
		this.music = music;
		this.sfx = sfx;
		this.faceL = faceL;
		this.faceR = faceR;
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
	public GameObject leftFace;
	public GameObject rightFace;
	
	public GameObject leftNameSprite;
	public GameObject rightNameSprite;

	public GameObject leftNameParent;
	public GameObject rightNameParent;

	public GameObject leftPentagon;
	public GameObject rightPentagon;

	private float yUp;
	private float yDown = -1600;
	private bool sceneStart = true;
	public GameObject backGround;
	TextMeshProUGUI mainBoxText;
	overallDialogue[] dialogueVarieties;
	public GameObject PlayScripts;
	bool writing;
	private int dialogueChoice = 0;
	private bool endOfDay = false;

	private float textSpeed;

	private FMOD.Studio.EventInstance musicInstance;
	private FMOD.Studio.EventInstance sfxInstance;

	public GameObject pauseMenu;
	private GameObject pauseMenuObject;

	public Canvas canvas;

	public GameObject keyPrompt;

	public Color[] beverlyColors;
	public Color[] ameryColors;
	public Color[] koralieColors;
	public Color[] jadeColors;
	public Color[] seraphimColors;

	public GameObject loadingAnimation;
	public GameObject realDialogueBox;

	public GameObject transitionObject;
	public Animator transitionAnimator;
	public float transitionTime = .5f;

	// Start is called before the first frame update
	void Start()
    {

		yUp = leftNameParent.transform.localPosition[1];

		textSpeed = PlayerPrefs.GetFloat("dialogueTextSpeed", .055f);

		/*dialogueVarieties = new overallDialogue[1];
    	dialogueVarieties[0] = new overallDialogue("test", "seraphim", new dialogueEntry[]{new dialogueEntry("this is the first line", false),
    		 new dialogueEntry("this is the second line", true), new dialogueEntry("this is the third line", false)});
        //Need to figure out how to take input
        */
		mainBoxText = mainBox.GetComponent<TextMeshProUGUI>();
		//handles which cutscenes to open
		switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentDay){
			
			case 0: default: //day 1
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
				endOfDay = true;
				break; 
			}
			break;

			case 1:
				switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene){
					case 0: default:
					dialogueVarieties = PlayScripts.GetComponent<Script2a>().script;
					endOfDay = true;
					break;
				}
			break;

			case 2:
				switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene){
					default:
					dialogueVarieties = PlayScripts.GetComponent<Script3a>().script;
					endOfDay = true;
					break;
				}
			break;

			case 3:
				switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene){
					default:
					dialogueVarieties = PlayScripts.GetComponent<Script4a>().script;
					endOfDay = true;
					break;					
				}
			break;

			case 4:
				switch(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene){
					default:
					dialogueVarieties = PlayScripts.GetComponent<Script5a>().script;
					endOfDay = true;
					break;					
				}
			break;

		}
        chosenDialogue = dialogueVarieties[dialogueChoice];//flag

		this.leftSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerA);
		this.rightSprite.GetComponent<talkSpriteHandler>().changeCharacter(chosenDialogue.speakerB);
		this.leftFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerA),"");
		this.rightFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerB),"");
		this.backGround.GetComponent<backGroundHandler>().changeBackground(chosenDialogue.background);
        leftNameSprite.GetComponentInChildren<TextMeshProUGUI>().text = Truncate(chosenDialogue.speakerA);
        rightNameSprite.GetComponentInChildren<TextMeshProUGUI>().text = Truncate(chosenDialogue.speakerB);
    }

    // Update is called once per frame
    void Update()
    {
		if (pauseMenuObject == null)
        {
			//skip dialogue (Debugging)
			if (Input.GetKeyDown(KeyCode.S))
				endCutscene();
			
			if (Input.GetKeyDown(KeyCode.Z)||sceneStart)
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
						endCutscene();
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
						leftNameSprite.GetComponentInChildren<TextMeshProUGUI>().text = Truncate(chosenDialogue.speakerA);
						rightNameSprite.GetComponentInChildren<TextMeshProUGUI>().text = Truncate(chosenDialogue.speakerB);


						Debug.Log("dialogueChoice at end is " + dialogueChoice);
						Debug.Log("dialogueVarieties.Length at end is " + dialogueVarieties.Length);
						StartCoroutine("WriteLine");
						showCorrectNameTag();
					}
				}
				else
				{
					this.swapAndSound();
					showCorrectNameTag();
					StartCoroutine("WriteLine");
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape))
            {
				pauseMenuObject = Instantiate(pauseMenu, canvas.transform);
            }
			keyPrompt.SetActive(true);
		}
        else
        {
			keyPrompt.SetActive(false);
		}
		sceneStart = false;
    }

	void OnDestroy(){
		musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // fade music out on cutscene end
		//GameObject.Find("Music").SetActive(true);
	}

	void endCutscene (){
		GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene++;
		GameObject.Find("NodeMapCamera").GetComponent<CameraController>().changeObjective(
			GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentDay,
			GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene);
		dialogueChoice = 0;
		PlayerPrefs.SetInt("Load", 0);
		GameObject.Find("CurrentStats").GetComponent<savingEngine>().reset();
		GameObject.Find("CurrentStats").GetComponent<savingEngine>().checkpoint();
		if(endOfDay){
			endOfDay = false;
			GameObject.Find("NodeMapCamera").GetComponent<CameraController>().nextDay();
		}

		StartCoroutine(transitionToNodeMap());
	}

    public IEnumerator WriteLine(){
		textSpeed = PlayerPrefs.GetFloat("dialogueTextSpeed", .055f);
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
		musicInstance = RuntimeManager.CreateInstance("event:/"+path); 
		musicInstance.start();
	}

	private void playSfx(string path){
		sfxInstance = RuntimeManager.CreateInstance("event:/"+path); 
		sfxInstance.start();
	}

	private void showCorrectNameTag(){
		if(chosenDialogue.lines[currentLine].speaker&&chosenDialogue.speakerB != ""){
			leftNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				leftNameParent.GetComponent<Transform>().localPosition.x,
				yDown,
				leftNameParent.GetComponent<Transform>().localPosition.z
			);

			rightNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				rightNameParent.GetComponent<Transform>().localPosition.x,
				yUp,
				rightNameParent.GetComponent<Transform>().localPosition.z
			);

			setColors(Truncate(chosenDialogue.speakerB));

		}else if (!chosenDialogue.lines[currentLine].speaker&&chosenDialogue.speakerA != ""){
			leftNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				leftNameParent.GetComponent<Transform>().localPosition.x,
				yUp,
				leftNameParent.GetComponent<Transform>().localPosition.z
			);

			rightNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				rightNameParent.GetComponent<Transform>().localPosition.x,
				yDown,
				rightNameParent.GetComponent<Transform>().localPosition.z
			);

			setColors(Truncate(chosenDialogue.speakerA));

		}
		else{
			leftNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				leftNameParent.GetComponent<Transform>().localPosition.x,
				yDown,
				leftNameParent.GetComponent<Transform>().localPosition.z
			);
			rightNameParent.GetComponent<Transform>().localPosition = new Vector3 (
				rightNameParent.GetComponent<Transform>().localPosition.x,
				yDown,
				rightNameParent.GetComponent<Transform>().localPosition.z
			);

			setColors("");
		}
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
					leftFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerA),chosenDialogue.lines[currentLine].faceL);
					rightSprite.GetComponent<talkSpriteHandler>().makeActive();
					rightFace.GetComponent<faceHandler>().makeActive();
					rightFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerB),chosenDialogue.lines[currentLine].faceR);
        		}
        		else{
					rightSprite.GetComponent<talkSpriteHandler>().makeIdle();
					rightFace.GetComponent<faceHandler>().makeIdle();
					rightFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerB),chosenDialogue.lines[currentLine].faceR);
					leftSprite.GetComponent<talkSpriteHandler>().makeActive();
					leftFace.GetComponent<faceHandler>().makeActive();				
					leftFace.GetComponent<faceHandler>().changeFace(Truncate(chosenDialogue.speakerA),chosenDialogue.lines[currentLine].faceL);	
        		}
				
				// music
				if (chosenDialogue.lines[currentLine].music == "stop")
					musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				else if(chosenDialogue.lines[currentLine].music != "null")
				{
					musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.playMusic(chosenDialogue.lines[currentLine].music);
				}
				// sfx
				if (chosenDialogue.lines[currentLine].sfx == "stop")
					sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				else if(chosenDialogue.lines[currentLine].sfx != "null")
				{
					sfxInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.playSfx(chosenDialogue.lines[currentLine].sfx);
				}
	}

	private string Truncate(string name){
		//Handles names for alternate character images
		//format for alternates is "SeraphimAlt0"
		if (name.Contains("Alt")){
			int temp = name.Length - 4;
			return name.Substring(0,temp);
		} else
			return name;
	}

	private void setColors(string character)
    {
		Color[] currentColors = new Color[3];

		for (int i = 0; i < 3; i++)
        {
			currentColors[i] = new Color(1, 1, 1, 1);
        }

		switch(character){
			case "Beverly":
				currentColors = beverlyColors;
				break;
			
			case "Amery":
				currentColors = ameryColors;
				break;

			case "Jade":
				currentColors = jadeColors;
				break;

			case "Koralie":
				currentColors = koralieColors;
				break;

			case "Seraphim":
				currentColors = seraphimColors;
				break;

			default:
				break;
		}

		leftPentagon.GetComponent<Image>().color = currentColors[0];
		rightPentagon.GetComponent<Image>().color = currentColors[0];

		leftNameSprite.GetComponent<Image>().color = currentColors[1];
		rightNameSprite.GetComponent<Image>().color = currentColors[1];

		realDialogueBox.GetComponent<Image>().color = currentColors[2];
	}

	void showSavingAnimation()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Node Map"));
		Instantiate(loadingAnimation);
	}

	IEnumerator transitionToNodeMap()
    {
		transitionAnimator.SetTrigger("toBlack");

		yield return new WaitForSeconds(transitionTime);

		showSavingAnimation();
		GameObject.Find("NodeMapCamera").GetComponent<CameraController>().isTransitioningFromAnotherScene = true;
		SceneManager.UnloadSceneAsync(sceneName);
	}
}
