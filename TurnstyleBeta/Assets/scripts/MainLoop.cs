using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MainLoop : MonoBehaviour
{
	public GameObject[] friendlies;
	public GameObject[] enemies;
	//player units
	public Friendly[] playerUnits;
	//And enemy
	public Enemy[] enemyUnits;

	//Seperate arrays for units on bench and off
	public Friendly[] activeUnits;
	public Friendly[] benchUnits;
	//And also a list of actions to take, except it's really a list of units
	private List<Unit> queuedActions;
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
        activeUnits = new Friendly[]{playerUnits[0], playerUnits[1], playerUnits[2]};
        benchUnits = new Friendly[]{playerUnits[3], playerUnits[4]};

        queuedActions = new List<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    void setPlayerAction(Unit unit, Unit target, Ability abil, int speed){
    	unit.queuedAction = new QueuedAction(target, abil, speed + unit.fatigue);
    	queuedActions.Add(unit);
    }

    //Resolve all actions in order
    void resolveActions(){
    	queuedActions.Sort(delegate(Unit a, Unit b) {return b.queuedAction.speed - b.fatigue-a.queuedAction.speed + a.fatigue;});
    	foreach(Unit actor in queuedActions){
    		if(actor.dead)
    			continue;
    		actor.act();
    	}
    	queuedActions.Clear();
    }

    //Sets which active units
    //Pass it the index of the first active unit
    void setActiveUnits(int start){
    	for(int i = 0; i < 3; i++){
    		activeUnits[i] = playerUnits[(i + start)% 5];
    	}
    	for (int i = 0; i < 2; i++){
    		benchUnits[i] = playerUnits[(i + start + 3)% 5];
    	}
    }
}
