using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = System.Random;
using TMPro;
using FMODUnity;

public class displayObject{
    public string text; //the text sent to the display
    public Unit origin; //location of affected unit
    public bool isDamage;//whether or not popUp is healing or damage
    public int popUp; //damage/healing numbers displayed over unit
    public StatusName status; //status image that appears over unit
    public string sound; //sound effect to play
    public displayObject(string text, bool isDamage){
        this.text = text;
        this.origin = null;
        this.popUp = 0;
        this.isDamage = isDamage;
        this.status = StatusName.None;
        this.sound ="null";
    }

    public displayObject(string text, bool isDamage, string sound){
        this.text = text;
        this.origin = null;
        this.popUp = 0;
        this.isDamage = isDamage;
        this.status = StatusName.None;
        this.sound = sound;
    }
    public displayObject(string text, Unit origin, StatusName status, bool isDamage){
        this.text = text;
        this.origin = origin;
        this.popUp = 0;
        this.isDamage = isDamage;
        this.status = status;
        this.sound = "null";
    }
    public displayObject(string text, Unit origin, int popUp, bool isDamage){
        this.text = text;
        this.origin = origin;
        this.popUp = popUp;
        this.isDamage = isDamage;
        this.status = StatusName.None;
        this.sound = "null";
    }
     public displayObject(string text, Unit origin, StatusName status, bool isDamage, string sound){
        this.text = text;
        this.origin = origin;
        this.popUp = 0;
        this.isDamage = isDamage;
        this.status = status;
        this.sound= sound;
    }
    public displayObject(string text, Unit origin, int popUp, bool isDamage, string sound){
        this.text = text;
        this.origin = origin;
        this.popUp = popUp;
        this.isDamage = isDamage;
        this.status = StatusName.None;
        this.sound = sound;
    }
}
public class MainLoop : MonoBehaviour
{
    public string sceneName;
    public GameObject[] friendlies;
	public GameObject[] enemies;
	//player units
	Friendly[] playerUnits;
	//And enemy
	Enemy[] enemyUnits;

	//Seperate arrays for units on bench and off
	public Friendly[] activeUnits = new Friendly[3];
	public Friendly[] benchUnits = new Friendly[2];
    Random rand;
	//List of text to display at end of turn
	public List<displayObject> outputQueue;

	//And also a list of actions to take, except it's really a list of units
	private List<Unit> queuedActions;
	public int speedTotal;
    public float textSpeed;

    public GameObject uiController;
    public GameObject damagePopUp;
    public GameObject statusPopUp;
    GameObject Stats;

    public Canvas canvas;
    void Awake()
    {
        Stats = GameObject.Find("CurrentStats");
        CurrentStats currStats = Stats.GetComponent<CurrentStats>();
        rand = new Random();
        playerUnits = new Friendly[friendlies.Length];
        enemyUnits = new Enemy[enemies.Length];
        //get the scripts on units
        for (int i = 0; i < friendlies.Length; i++)
        {
            playerUnits[i] = friendlies[i].GetComponent<Friendly>();
            switch (playerUnits[i].name)
            {
                case "Beverly":
                    playerUnits[i].hp = currStats.BeverlyHealth;
                    break;
                case "Jade":
                    playerUnits[i].hp = currStats.JadeHealth;
                    break;
                case "Koralie":
                    playerUnits[i].hp = currStats.KoralieHealth;
                    break;
                case "Seraphim":
                    playerUnits[i].hp = currStats.SeraphimHealth;
                    break;
                case "Amery":
                    playerUnits[i].hp = currStats.AmeryHealth;
                    break;
            }
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyUnits[i] = enemies[i].GetComponent<Enemy>();
            enemyUnits[i].unitName = currStats.CurrentEnemies[i];
        }
        //Put the first 3 units into activeUnits, remainder in bench
        //Code is a bit awkward but could be worse

        // this needs to be put back in once the friendly objects are properly put into the nameTags
        //activeUnits[0] = playerUnits[0];
        //activeUnits[1] = playerUnits[1];
        //activeUnits[2] = playerUnits[2];    // = new Friendly[]{playerUnits[0], playerUnits[1], playerUnits[2]};

        //benchUnits[0] = playerUnits[3];
        //benchUnits[1] = playerUnits[4];     //= new Friendly[]{playerUnits[3], playerUnits[4]};

        queuedActions = new List<Unit>();
        speedTotal = 12;
        outputQueue = new List<displayObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        textSpeed = PlayerPrefs.GetFloat("combatTextSpeed", 1.125f);
    }

    // Update is called once per frame
    void Update()
    {
    	//Debug, can remove if wanted
        if (Input.GetKeyDown(KeyCode.I))
        	StartCoroutine(OutputText());
    }

    //Pretty much all code will be in functions hooked up to UI elements

    void startTurn(){
    	//Make sure that all the active units are active and all bench units are benched.
    	foreach (Friendly unit in activeUnits){
    		unit.turnStart();
    		unit.makeActive();
    	}
    	foreach (Friendly unit in benchUnits){
    		unit.stopActive();
    	}
    	speedTotal = 12;
    }

    //Code for handling the enemies
    public void queueEnemyActions(){
    	foreach(Enemy unit in enemyUnits){
    		if(unit.dead)
    			continue;
    		//Eventually the AI will be able to kill low-health targets, which requires rewriting of abilities
    		//So that will be implemented after meeting
    		// (AI needs to be able to know how much damage it does)
    		//All these lists store ints because they are storing indexes of objects
    		//Figure out which abilities are legal
    		List<int> legalAbilities = new List<int>();
    		//Also which units can be legally targeted by each
    		List<int>[] legalTargets = {new List<int>(), new List<int>(), new List<int>()};
    		//We want to use a foreach loop for simplicity but need an index, so track it manually
    		int abilityIndex = 0;
    		foreach (Ability ability in unit.abilities){
    			//Test on all legal targets
    			int unitIndex = 0;
    			bool foundTarget = false;
    			if(ability.allies == true){
					foreach(Friendly targetUnit in activeUnits){
						if(ability.requirement(targetUnit, unit)){
							legalTargets[abilityIndex].Add(unitIndex);
							foundTarget = true;
						}
						unitIndex++;
					}
    			}
    			else{
    				foreach(Enemy targetUnit in enemyUnits){
    					if(targetUnit == unit && !ability.selftarget)
    						continue;
						if(ability.requirement(targetUnit, unit)){
							legalTargets[abilityIndex].Add(unitIndex);
							foundTarget = true;
						}
						unitIndex++;
					}
    			}
    			if(foundTarget){
    				legalAbilities.Add(abilityIndex);
    			}
    			abilityIndex++;
    		}
    		//randomly decide the ability and target
    		//Random rand = new Random(); 
    		int chosenAbility = legalAbilities[rand.Next(legalAbilities.Count)];
            int randTest = rand.Next(legalTargets[chosenAbility].Count);
            Debug.Log("randTest = " + randTest);
            int chosenTarget = legalTargets[chosenAbility][randTest];
            //Debug.Log("available random targets: " + legalTargets[chosenAbility].Count);
    		Unit actTarget;
    		if(unit.abilities[chosenAbility].allies)
    			actTarget = enemyUnits[chosenTarget];
    		else
    			actTarget = activeUnits[chosenTarget];
    		queuedActions.Add(unit);
    		unit.queuedAction = new QueuedAction(actTarget, unit.abilities[chosenAbility], rand.Next(9));
    	}
    }

    //set player action.
    public void setPlayerAction(Unit unit, Unit target, Ability abil, int speed){
		int hasteMod = 0;
		if(unit.statuses[(int)StatusType.Buff].name == StatusName.Haste)
			hasteMod = unit.statuses[(int)StatusType.Buff].magnitude;
    	if(!queuedActions.Contains(unit))
    		queuedActions.Add(unit);
    	unit.queuedAction = new QueuedAction(target, abil, speed - unit.fatigue + hasteMod);
    }

    void debugQeuedActions()
    {
        foreach (Unit actor in queuedActions)
        {
            Debug.Log(actor.name + " is in queuedActions");
        }
    }
    public void debugPlayerUnits()
    {
        foreach (Unit actor in playerUnits)
        {
            Debug.Log(actor.name + " is in playerUnits");
        }
        Debug.Log("done debugging Player Units");
    }
    //Resolve all actions in order
    public IEnumerator resolveActions(){
        debugQeuedActions();
    	queuedActions.Sort(delegate(Unit a, Unit b) {return b.queuedAction.speed-a.queuedAction.speed;});
        Debug.Log("Queued Actions sorted");
        debugQeuedActions();
        Debug.Log("Done debugging queued actions after sort");
        foreach (Unit actor in queuedActions){
    		if(actor.dead)
    			continue;
            Debug.Log(actor.name + " used " + actor.queuedAction.ability.name + "!");
            //outputQueue.Add(actor.name + " used " +  actor.queuedAction.ability.name + "!");
    		actor.act();
            Debug.Log("Actor has Acted");
            yield return new WaitForSeconds(textSpeed);
    	}
        Debug.Log("QueuedActions Cleared");
    	queuedActions.Clear();
        endTurn();
    }

    //Sets which active units
    //Pass it the index of the first active unit
    public void setActiveUnits(nameTag[] nameTags){
    	for(int i = 0; i < 3; i++){
    		activeUnits[i] = nameTags[i].GetComponent<nameTag>().character.GetComponent<Friendly>();
            nameTags[i].GetComponent<nameTag>().character.GetComponent<Friendly>().makeActive();
        }
    	for (int i = 0; i < 2; i++){
    		benchUnits[i] = nameTags[i+3].GetComponent<nameTag>().character.GetComponent<Friendly>();
            nameTags[i+3].GetComponent<nameTag>().character.GetComponent<Friendly>().stopActive();
        }
    }

   	//UI controller calls this function when turn ends
   	public void endTurn(){

   		//queue all enemy actions
   		//queueEnemyActions();
   		//Do all the actions 
   		//resolveActions();
   		//Run end turn stuff
   		foreach (Unit unit in playerUnits){
   			unit.turnEnd();
   		}
   		foreach (Unit unit in enemyUnits){
   			unit.turnEnd();
   		}
   		//display output

   		//is game over?
   		bool allDead = true;
   		foreach(Unit unit in playerUnits){
   			if(!unit.dead){
   				allDead = false;
   				break;
   			}
   		}
   		//if allDead, loss
   		bool enemyDead = true;
   		foreach(Unit unit in enemyUnits){
   			if(!unit.dead){
   				enemyDead = false;
   				break;
   			}
   		}
        //if enemyDead, win
    
   		//Otherwise continue
   		if(!allDead && !enemyDead){
            //flag
   			startTurn();
   		}
   	}


   	//Coroutine for displaying output
   	public IEnumerator OutputText(){
   		//Get the text box since it gets generated
   		TextMeshProUGUI textbox = GameObject.Find("resultsBox(Clone)").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

   		for(int i = 0; i < outputQueue.Count; i++){
   			string outputBuild = "";
   			for(int index = -4; index < 1; index++){
   				if(i + index < 0)
   					continue;
   				outputBuild += outputQueue[i + index].text + "\n";
   			}
   			//Debug.Log(outputBuild);
   			textbox.text = outputBuild;

            //Generate popUps
            Vector3 tempLoc;
            if(outputQueue[i].origin != null){
                    if(outputQueue[i].origin.tag == "Enemy")
                    tempLoc = new Vector3 (outputQueue[i].origin.GetComponent<Transform>().position.x,
                        outputQueue[i].origin.GetComponent<Transform>().position.y+100,
                        outputQueue[i].origin.GetComponent<Transform>().position.z );
                    else if(outputQueue[i].origin.tag == "Ally"){
                        Image locTrans = outputQueue[i].origin.GetComponent<Friendly>().sprite;
                        tempLoc = new Vector3 (locTrans.GetComponent<Transform>().position.x,
                        locTrans.GetComponent<Transform>().position.y+500,
                        locTrans.GetComponent<Transform>().position.z );
                    }
                    else tempLoc = new Vector3 (0f,0f,0f);
                }
            else tempLoc = new Vector3 (0f,0f,0f);
            //Debug.Log("Location is "+tempLoc.x+", "+tempLoc.y+", "+tempLoc.z+" targeting "+outputQueue[i].origin.unitName);

            if(outputQueue[i].status != StatusName.None){
            GameObject temp1 = Instantiate(statusPopUp,
                tempLoc,
                Quaternion.identity,
                canvas.transform) as GameObject; 
                temp1.GetComponent<PopUpDestroyer>().timeOut = textSpeed;
            temp1.GetComponent<Image>().sprite = Resources.Load<Sprite>("StatusIcons/icon"+outputQueue[i].status.ToString());
            }

            if(outputQueue[i].popUp != 0){
            GameObject temp2 = Instantiate(damagePopUp,
                tempLoc,
                Quaternion.identity,
                canvas.transform) as GameObject;
            temp2.GetComponent<PopUpDestroyer>().timeOut = textSpeed;
            temp2.GetComponent<TextMeshProUGUI>().text = outputQueue[i].popUp.ToString();
            }

            if(outputQueue[i].sound != "null"){
                FMOD.Studio.EventInstance sfxInstance;
		        sfxInstance = RuntimeManager.CreateInstance("event:/Battle/"+outputQueue[i].sound); 
		        sfxInstance.start();
            }

   			yield return new WaitForSeconds(textSpeed);

            foreach(nameTag tag in uiController.GetComponent<combatController>().nameTagArray)
            {
                tag.updateAllStatuses();
            }
            foreach (Unit unit in enemyUnits)
            {
                unit.Kill();
            }    
        }
        outputQueue.Clear();
        bool alldead = true;
        foreach (Unit unit in enemyUnits)
        {
            if (!unit.dead)
            {
                alldead = false;
                break;
            } 
        }
        if (alldead)
        {
            Stats = GameObject.Find("CurrentStats");
            CurrentStats currStats = Stats.GetComponent<CurrentStats>();
            foreach (Friendly friend in playerUnits)
            {
                switch (friend.name)
                {
                    case "Beverly":
                        if (friend.hp == 0)
                            friend.hp = 1;
                        currStats.BeverlyHealth = friend.hp;
                        break;
                    case "Jade":
                        if (friend.hp == 0)
                            friend.hp = 1;
                        currStats.JadeHealth = friend.hp;
                        break;
                    case "Koralie":
                        if (friend.hp == 0)
                            friend.hp = 1;
                        currStats.KoralieHealth = friend.hp;
                        break;
                    case "Seraphim":
                        if (friend.hp == 0)
                            friend.hp = 1;
                        currStats.SeraphimHealth = friend.hp;
                        break;
                    case "Amery":
                        if (friend.hp == 0)
                            friend.hp = 1;
                        currStats.AmeryHealth = friend.hp;
                        break;
                }
            }
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
    }
}
