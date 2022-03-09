using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = System.Random;
using TMPro;
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
	public List<string> outputQueue;

	//And also a list of actions to take, except it's really a list of units
	private List<Unit> queuedActions;
	public int speedTotal;
    public float textSpeed;

    public GameObject uiController;
    GameObject Stats;
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
        outputQueue = new List<string>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
    void queueEnemyActions(){
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
    void resolveActions(){
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
    	}
    	queuedActions.Clear();
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
   		queueEnemyActions();
   		//Do all the actions 
   		resolveActions();
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
   			startTurn();
   		}
   	}


   	//Coroutine for displaying output
   	public IEnumerator OutputText(){
   		//Get the text box since it gets generated
   		TextMeshProUGUI textbox = GameObject.Find("resultsBox(Clone)").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
   		//outputQueue.Add("a");
   		//outputQueue.Add("b");
   		//outputQueue.Add("c");
   		//outputQueue.Add("d");
   		//outputQueue.Add("e");
   		//outputQueue.Add("f");
   		//outputQueue.Add("g");
   		for(int i = 0; i < outputQueue.Count; i++){
   			string outputBuild = "";
   			for(int index = -4; index < 1; index++){
   				if(i + index < 0)
   					continue;
   				outputBuild += outputQueue[i + index] + "\n";
   			}
   			//Debug.Log(outputBuild);
   			textbox.text = outputBuild;

   			yield return new WaitForSeconds(textSpeed);

            foreach(nameTag tag in uiController.GetComponent<combatController>().nameTagArray)
            {
                tag.updateAllStatuses();
            }
            
        }
        foreach (Unit unit in enemyUnits)
        {
            unit.Kill();
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
