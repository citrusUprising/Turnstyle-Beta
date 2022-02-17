using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using TMPro;
public class MainLoop : MonoBehaviour
{
	public GameObject[] friendlies;
	public GameObject[] enemies;
	//player units
	public Friendly[] playerUnits;
	//And enemy
	public Enemy[] enemyUnits;

	//Seperate arrays for units on bench and off
	public Friendly[] activeUnits = new Friendly[3];
	public Friendly[] benchUnits = new Friendly[2];

	//List of text to display at end of turn
	public List<string> outputQueue;

	//And also a list of actions to take, except it's really a list of units
	private List<Unit> queuedActions;
	public int speedTotal;
    // Start is called before the first frame update
    void Start()
    {	
        //get the scripts on units
        for (int i = 0; i < friendlies.Length; i++){
        	playerUnits[i] = friendlies[i].GetComponent<Friendly>();
        }
        for (int i = 0; i < enemies.Length; i++){
        	enemyUnits[i] = enemies[i].GetComponent<Enemy>();
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
    		Random rand = new Random(); 
    		int chosenAbility = legalAbilities[rand.Next(legalAbilities.Count)];
    		int chosenTarget = legalTargets[chosenAbility][rand.Next(legalTargets[chosenAbility].Count)];
    		Unit actTarget;
    		if(unit.abilities[chosenAbility].allies)
    			actTarget = playerUnits[chosenTarget];
    		else
    			actTarget = enemyUnits[chosenTarget];
    		queuedActions.Add(unit);
    		unit.queuedAction = new QueuedAction(actTarget, unit.abilities[chosenAbility], rand.Next(9));
    	}
    }

    //set player action.
    public void setPlayerAction(Unit unit, Unit target, Ability abil, int speed){
    	if(!queuedActions.Contains(unit))
    		queuedActions.Add(unit);
    	unit.queuedAction = new QueuedAction(target, abil, speed + unit.fatigue);
    }

    //Resolve all actions in order
    void resolveActions(){
    	queuedActions.Sort(delegate(Unit a, Unit b) {return b.queuedAction.speed-a.queuedAction.speed;});
    	foreach(Unit actor in queuedActions){
    		if(actor.dead)
    			continue;
    		outputQueue.Add(actor.name + " used " +  actor.queuedAction.ability.name + "!");
    		actor.act();
    	}
    	queuedActions.Clear();
    }

    //Sets which active units
    //Pass it the index of the first active unit
    public void setActiveUnits(int start){
    	for(int i = 0; i < 3; i++){
    		activeUnits[i] = playerUnits[(i + start)% 5];
    	}
    	for (int i = 0; i < 2; i++){
    		benchUnits[i] = playerUnits[(i + start + 3)% 5];
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
   	IEnumerator OutputText(){
   		//Get the text box since it gets generated
   		TextMeshProUGUI textbox = GameObject.Find("resultsBox(Clone)").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
   		GameObject textObject = GameObject.Find("resultsBox(Clone)").transform.GetChild(0).gameObject;
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
   			yield return new WaitForSeconds(.5f);
   		}
   	}
}
