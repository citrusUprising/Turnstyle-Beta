using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class combatController : MonoBehaviour
{
    // --------------------------------------------------------- //
    // variables that interact with states in general
    // there is an explanation of all the states at the 
    // beginning of Start()
    // --------------------------------------------------------- //
    public GameObject pentagonSprite;
    public Canvas canvas;
    public GameObject rotateBox;
    public GameObject moveSelectBox;
    public GameObject speedSelectBox;
    public GameObject confirmBox;
    public GameObject playResultsBox;
    public GameObject pauseMenu;
    public GameObject mainLoopObject;
    public totalSpeed totalSpeedPrefab;
    private string state = "rotate";
    MainLoop gameLoop;
    // this is currently only used in the pause state
    private string previousState = "rotate";

    // this is the current box that is being drawn in the top right corner
    // every state transition destroys this and most of them create a new one
    private GameObject currentDrawnBox;


    // --------------------------------------------------------- //
    // variables that interact with the selectMove state
    // --------------------------------------------------------- //
    // the amount of moves that have been selected, one per character
    // this goes up by one every time the player selects and chooses targets for a move
    // this is used for telling if we should go to the next character's move selection or move onto the confirm state
    private int numberOfSelectedMoves = 0;

    // the current move that the selector is on, 0-2
    private int selectedAbilityIndex = 0;

    // hard-coded coordinates for the selector sprite
    private int[] pointerCoords = new int[3] { -57, -81, -105 };

    // the selector sprite
    private GameObject moveSelectPointer;

    // --------------------------------------------------------- //
    // 🎯 target select variables
    // --------------------------------------------------------- //
    public GameObject targetPointer;
    private int targetIndex = 0;

    private Vector3[] playerTargets = new Vector3[5] {new Vector3(-65, 8, 0), 
    new Vector3(43,-110,0), new Vector3(16,-216,0), new Vector3(-260, -201, 0),
    new Vector3(-260, -31, 0)};

    // --------------------------------------------------------- //
    // variables that interact with the rotate state
    // --------------------------------------------------------- //

    // if the pentagon is rotating or not
    // the rotate function does nothing if it is already rotating
    // this makes everything a lot simpler
    private bool isRotating = false;

    // this is the rotation of the pentagon sprite
    private float pentagonRotation = 0.0f;

    // this is the value that we lerp the pentagon rotation to
    // it will increase or decrease by 72 (one fifth of 360) whenever the rotate function is called
    private float nextPentagonRotation = 0.0f;

    // this goes between 0 and 4
    // this will be important for implementing game logic
    private int rotationState = 0;

    public float pentagonRotateX;
    public float pentagonRotateY;

    // this stores the coordinates of the name tags when the scene starts
    // the logic behind this is a little flimsy. right now, all the name tags have to be in the correct location in the scene,
    // or else they wont be put in the right locaion later.
    // the idea behind this is that if a name tag is [i] in the name tag array, then it should go to the coordinates at [i] 
    // in this array. the problem i am having with this currently is that i can't figure out a good way to store the name tags' 
    // locations before they get moved along the array, so i can't lerp them properly to the new array.
    // this wouldn't be a problem in phaser because we could tween them from their current x and y values and forget aobut it,
    // but there are no tweensin unity. at least not that i know.
    private Vector3[] nameTagCoords = new Vector3[5];

    // these are taken from the name tag objects and set in the begin rotation function
    // they are used for the lerp
    private Vector3 oldNameTagCoordsBeverly;
    private Vector3 oldNameTagCoordsAmery;
    private Vector3 oldNameTagCoordsKoralie;
    private Vector3 oldNameTagCoordsJade;
    private Vector3 oldNameTagCoordsSeraphim;

    // this is time 
    private float t = 0.0f;

    // --------------------------------------------------------- //
    // these are used in the rotate state, but will also be used
    // later on for game logic stuff
    // --------------------------------------------------------- //
    public nameTag nameTagBeverly;
    public nameTag nameTagAmery;
    public nameTag nameTagKoralie;
    public nameTag nameTagJade;
    public nameTag nameTagSeraphim;
    // this is an array of the above objects
    public nameTag[] nameTagArray;

    // --------------------------------------------------------- //
    // these are used in the speedSelect state
    // --------------------------------------------------------- //
    public int totalSpeedEachTurn = 12;
    public int totalSpeedThisTurn = 12;
    private int speedForCurrentMove = 0;
    private int totalSpeedAllocatedThisTurn = 0;
    // private string currentMoveName = "CURRENT MOVE";
    private GameObject topSquare;
    private GameObject bottomSquare;
    private GameObject speedSelectTextObject;
    private totalSpeed totalSpeedIndicator1;
    private totalSpeed totalSpeedIndicator2;
    private int[] selectedSpeeds = new int[3];

    // --------------------------------------------------------- //
    // variables for setPlayerAction()
    // --------------------------------------------------------- //
    private Unit selectedUnit;
    private Unit selectedTarget;
    private Ability selectedAbility;
    private int selectedSpeed;

    // --------------------------------------------------------- //
    // used in the paused state
    // --------------------------------------------------------- //
    private GameObject pauseMenuInstance;
    private bool justUnpaused = false;

    // --------------------------------------------------------- //
    // 👾 enemy variables
    // --------------------------------------------------------- //
    public GameObject[] enemies;

    // --------------------------------------------------------- //
    // results display
    // --------------------------------------------------------- //
    private string[] actions = new string[3];
    // Start is called before the first frame update
    void Start()
    {

        totalSpeedIndicator1 = Instantiate(totalSpeedPrefab, canvas.transform);

        // 3 and 4 are inactive, 0, 1, and 2 are active
        nameTagArray[0] = nameTagBeverly;
        nameTagArray[1] = nameTagAmery;
        nameTagArray[2] = nameTagKoralie;
        nameTagArray[3] = nameTagJade;
        nameTagArray[4] = nameTagSeraphim;

        // init each player's moves here ⬇ this code is ugly but it works
        nameTagArray[0].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Smolder(), new Dazzle(), new Imbibe()}; 
        nameTagArray[1].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Mitigate(), new Fallguy(), new Scrum()};
        nameTagArray[2].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Repel(), new Hunker(), new Crush()};
        nameTagArray[3].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Stunnerclap(), new Rally(), new Motivate()};
        nameTagArray[4].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Soulrip(), new Scry(), new Slump()};

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < 5; i++)
        {
            nameTagCoords[i] = nameTagArray[i].transform.position;
        }

        gameLoop = mainLoopObject.GetComponent<MainLoop>();
        // the available states so far are "rotate", "moveSelect", "targetSelect", "confirm", "playResults", "paused" (in that order)
        // "rotate" is for rotating the pentagon 
        // "moveSelect" is for selecting the move for a character
        // "targetSelect" is for selecting the target for the move slected in moveSelect
        // "speedSelect" is for selecting the speed for the move afte the target has been selected
        // "confirm" is for reviewing and confirming the selected moves
        // "playResults" is when the player has no controls and the combat animations play out
        // "paused" is when the pause menu is opened. it can be accessed by any state other than "playResults" and goes back to that state
        //
        // these go in the order of:
        // rotate -> moveSelect(1) -> targetSelect(1) -> moveSelect(2) -> targetSelect(2) -> moveSelect(3) -> targetSelect(3) -> confirm ->
        // EITHER moveSelect(1) OR playResults -> rotate REPEAT
        targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
        transitionToRotate();
    }

    // Update is called once per frame
    void Update()
    {
        // if the state is "rotate," than the available controls are up and down to rotate and Z to advance to move select
        if (state == "rotate")
        {
            // if you press X, advance to the next state, destroying the rotate UI and replacing it with move select UI
            if (Input.GetKeyDown(KeyCode.X))
            {
                gameLoop.setActiveUnits(nameTagArray); 
                transitionToMoveSelect();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // if the pentagon is NOT rotating, then begin rotating DOWN
                if (!isRotating)
                {
                    beginRotatingPentagon(-1);
                }
                    
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // if the pentagon IS rotating, then begin rotating UP
                if (!isRotating)
                {
                    beginRotatingPentagon(1);
                }
            }
            // here is all the logic 
            if (isRotating)
            {
                rotatePentagon();
            }
        } 

        else if (state == "moveSelect")
        {
            // when the down arrow is pressed, move the selection down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                changeSelectedAbilityIndex(1);
                // nameTagArray[numberOfSelectedMoves].GetComponent<PlayerMoveSelect>().movePointer(1);
                
            }
            // when the up arrow is pressed, move the selection up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                changeSelectedAbilityIndex(-1);
                // nameTagArray[numberOfSelectedMoves].GetComponent<PlayerMoveSelect>().movePointer(-1);
            }
            // when the X key is pressed, we need to go to selecting targets
            if (Input.GetKeyDown(KeyCode.X))
            {
                selectedAbility = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities[selectedAbilityIndex];
                actions[numberOfSelectedMoves] += selectedAbility.name + " on ";
                transitionToTargetSelect();
            }
            // back function needs to be implemented
            // should go back to rotate if numberOfSelectedMoves is 0 and back to moveSelect if it is greater than zero
            // if it goes back to moveSelect, then it should -- numberOfSelectedMoves
            else if (Input.GetKeyDown(KeyCode.Z))
            {

            }
        } 

        // needs to be implemented
        else if (state == "targetSelect")
        {
            // when the down arrow is pressed, move the selection down
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("previous enemy");
                changeSelectedTarget(-1, selectedAbility.allies);
                // change target   
            }
            // when the up arrow is pressed, move the selection up
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("next enemy");
                changeSelectedTarget(1, selectedAbility.allies);
            }
            // when the X key is pressed, we need to go to selecting speed
            if (Input.GetKeyDown(KeyCode.X))
            {
                targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
                transitionToSpeedSelect();
            }
        }

        else if (state == "speedSelect")
        {
            // this confirms the selected speed and goes to the next state
            // if there have been three selected moves, then it goes to the confirm state
            // if there have not, it goes to move select
            if (Input.GetKeyDown(KeyCode.X))
            {
                // destroy the old speedIndicator2 (the one on top of the speed select sprite)
                // i had to make a custom function for some reason idk
                totalSpeedIndicator2.destroySelf();

                selectedSpeed = selectedSpeeds[numberOfSelectedMoves];
                Debug.Log("Selected Unit: " + selectedUnit.name);
                Debug.Log("Selected Target: " + selectedTarget.name);
                Debug.Log("Selected Ability: " + selectedAbility.name);
                Debug.Log("Selected Speed: " + selectedSpeed);
                //gameLoop.debugPlayerUnits();
                gameLoop.setPlayerAction(selectedUnit, selectedTarget, selectedAbility, selectedSpeed);
              
                    // this is used for a few different things, including handling which unit is acting
                numberOfSelectedMoves++;

                if (numberOfSelectedMoves == 3)
                {
                    transitionToConfirm();
                }
                else
                {
                    transitionToMoveSelect();
                }

            }

            // back function: needs to be implemented
            // should go back to the target select and reset the speed that was set for that move
            else if (Input.GetKeyDown(KeyCode.Z))
            {

            }

            // changes the speed of the selected move up by one
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                changeSpeed(1);
            }

            // changes it down by one
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                changeSpeed(-1);
            }

        }

        else if (state == "confirm")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if(selectedAbility.allies){
                    actions[numberOfSelectedMoves-1] += nameTagArray[targetIndex].GetComponent<nameTag>().character.GetComponent<Friendly>().name;
                }
                else{
                    actions[numberOfSelectedMoves-1] += enemies[targetIndex].GetComponent<Enemy>().name;
                }
                transitionToPlayResults();
            } 
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                numberOfSelectedMoves = 0;
                transitionToRotate();
            }
        }

        // THIS NEEDS TO BE DELETED ONCE WE GET FURTHER ALONG
        // in the play results state, the player should have no inputs. they should only proceed once all the results are finished displaying. 
        // at that point, the turn is complete and the state should be rotate
        // cuz we don't have all that done rn, i'm just using this for the moment for bug testing and such
        else if (state == "playResults")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                transitionToRotate();
            }
        }

        else if (state == "paused")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                transitionFromPause();
            }
        }

        // the pause menu should be able to be opened as long as the state is not playResults
        if (state != "playResults" && state != "pause")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && justUnpaused == false)
            {
                transitionToPause();
            }
        }
        justUnpaused = false;
    }

    // gets called once at the beginning of the pentagon rotation when the player presses up or down
    void beginRotatingPentagon(int direction)
    {
        // figure out the next pentagon rotation by adding or subtracting 72
        nextPentagonRotation = pentagonRotation + (72 * direction);

        // reset time to 0
        t = 0.0f;

        // set isRotating to true
        // this makes sure the function does not get called again until the rotation is done
        isRotating = true;

        // changes the rotation state
        // makes sure it's between and including 0 and 4
        // this variable actaully does nothing. if you can't think of a good reason for it to stick around, please delete it
        rotationState += direction;
        if (rotationState == 5)
        {
            rotationState = 0;
        }
        if (rotationState == -1)
        {
            rotationState = 4;
        }

        // this is a local variable that i use to move the name tags around
        // in the alpha we did all of this code in 2 lines. thank you javascript <3 ily bb
        // you never know what you have until you lose it T_T
        nameTag[] changeNameTagArray = new nameTag[5];

        // so basically everything gets pushed down one
        // and the thing in end gets put in the front
        if (direction == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    changeNameTagArray[i] = nameTagArray[0];
                }
                else
                {
                    changeNameTagArray[i] = nameTagArray[i + 1];
                }
            }
        }
        // and over here everything gets pushed up one
        // and the thing in the front gets pushed to the end
        else if (direction == -1)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    changeNameTagArray[i] = nameTagArray[4];
                }
                else
                {
                    changeNameTagArray[i] = nameTagArray[i - 1];
                }
            }
        }

        //NOW we update the actual nameTagArray with the local variable
        nameTagArray = changeNameTagArray;

        for (int i = 0; i < 5; i++)
        {
            nameTagArray[i].previousPosition = nameTagArray[i].transform.position;
            nameTagArray[i].nextPosition = nameTagCoords[i];
        }

        if (direction == -1)
        {
            nameTagArray[3].hidePassive(); // hide?
            nameTagArray[0].showPassive(); // show?
        }
        else if (direction == 1)
        {
            nameTagArray[4].hidePassive(); // hide?
            nameTagArray[2].showPassive(); // show?
        }
    }

    void rotatePentagon()
    {
        // converts the Vector3 rotatoins to Quarternion
        Quaternion oldRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, pentagonRotation);
        Quaternion newRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, nextPentagonRotation);

        // lerps the rotation of the pentagon to the next rotation
        pentagonSprite.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            nameTagArray[i].transform.position = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
        }

        // increases time
        // honestly i have no clue how this works. i have never understood what "deltaTime" means in any programming language
        // but hey it works
        t += 2.5f * Time.deltaTime;

        // when this happens, this function should no longer be called
        if (t > 1.0f)
        {
            // caps time at 1.0f
            t = 1.0f;

            for (int i = 0; i < 5; i++)
            {
                nameTagArray[i].transform.position = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
            }

            // the pentagon is done rotating by now
            isRotating = false;

            // update the rotation variable to the current rotation, that way next time the pentagon rotates, it lerps from the 
            // proper value
            pentagonRotation = nextPentagonRotation;
        }
    }

    void transitionToRotate()
    {
        numberOfSelectedMoves = 0;
        Destroy(currentDrawnBox);
        setPreviousState();
        state = "rotate";
        currentDrawnBox = Instantiate(rotateBox, canvas.transform);
        resetSpeed();

        nameTagArray[0].showPassive(); // show
        nameTagArray[1].showPassive(); // show
        nameTagArray[2].showPassive(); // show
    }

    void transitionToMoveSelect()
    {
        setPreviousState();
        state = "moveSelect";
        Destroy(currentDrawnBox);
        
        // 🎨 setting draw box color & move names 
        nameTagArray[numberOfSelectedMoves].GetComponent<PlayerMoveSelect>().ChangeColor();   

        currentDrawnBox = Instantiate(moveSelectBox, canvas.transform);
        selectedAbilityIndex = 0;
        moveSelectPointer = currentDrawnBox.transform.GetChild(5).gameObject;
        moveSelectPointer.transform.localPosition = new Vector3(
            moveSelectPointer.transform.localPosition[0],
            pointerCoords[selectedAbilityIndex],
            moveSelectPointer.transform.localPosition[2]);

        if (previousState == "rotate")
        {
            nameTagArray[0].hidePassive(); // hide
            nameTagArray[1].hidePassive(); // hide
            nameTagArray[2].hidePassive(); // hide
        }

        selectedUnit = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>();
        actions[numberOfSelectedMoves] += selectedUnit.name+ ": ";

        // this needs to be put back in once the friendly objects are properly put into the nameTags
        //gameLoop.setActiveUnits(rotationState);
    }

    void changeSelectedAbilityIndex(int change)
    {
        selectedAbilityIndex += change;
        if (selectedAbilityIndex == 3)
        {
            selectedAbilityIndex = 0;
        }
        if (selectedAbilityIndex == -1)
        {
            selectedAbilityIndex = 2;
        }
        // move the cursor up or down to the next move
        moveSelectPointer.transform.localPosition = new Vector3(
            moveSelectPointer.transform.localPosition[0], 
            pointerCoords[selectedAbilityIndex], 
            moveSelectPointer.transform.localPosition[2]);

        // 👉 changing move description to reflect pointer movement
        currentDrawnBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 
        nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities[selectedAbilityIndex].text;
    }

    void changeSelectedTarget(int change, bool allied){
        targetIndex += change;
        // this is kind of messy but it gets the allied targets in color order of 💙💛💗💚💖 
        if(allied){
            if(targetIndex == 3)
                targetIndex = 0;
            if(targetIndex == -1)
                targetIndex = 2;
            targetPointer.transform.localPosition = playerTargets[targetIndex];
            selectedTarget = nameTagArray[targetIndex].GetComponent<nameTag>().character.GetComponent<Friendly>();
        }
        else{
            if(targetIndex == enemies.Length)
                targetIndex = 0;
            if(targetIndex == -1)
                targetIndex = enemies.Length-1;
            targetPointer.transform.localPosition = new Vector3(
                    enemies[targetIndex].transform.localPosition[0] - 100,
                    enemies[targetIndex].transform.localPosition[1],
                    enemies[targetIndex].transform.localPosition[2]
                );

            selectedTarget = enemies[targetIndex].GetComponent<Enemy>();
            Debug.Log("Target = " + selectedTarget.name);
        }
    }

    // because we have not implemented this yet, it will go automatically to the speedSelect
    void transitionToTargetSelect()
    {
        setPreviousState();
        state = "targetSelect";
        // this state does not have a box associated with it. therefore, the old box should not be destroyed
        Debug.Log(selectedAbility);
        if (selectedAbility.selftarget) {
            selectedTarget = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>();
            targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
            transitionToSpeedSelect();
        } else if (selectedAbility.multitarget)
        {
            targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
            if (selectedAbility.allies) 
            {
                selectedTarget = nameTagArray[0].GetComponent<nameTag>().character.GetComponent<Friendly>();
            }
            else
            {
                selectedTarget = enemies[0].GetComponent<Enemy>();
            }
            transitionToSpeedSelect();
        }
        else{
            changeSelectedTarget(0, selectedAbility.allies);
            if(selectedAbility.allies){
                targetPointer.transform.localPosition = playerTargets[0];
                targetPointer.transform.eulerAngles = new Vector3(0,180,0);
            }
            else
                targetPointer.transform.eulerAngles = new Vector3(0,0,0);
            targetPointer.GetComponent<CanvasRenderer>().SetAlpha(1);
        }
    }

    // a lot of things have to happen here
    // TODO: 1. change color of the totalSpeedIndicator and speedSelectBox based on who is active
    //       2. add blinking animation 
    //       3. change text of the speedSelectBox to match the current move 
    void transitionToSpeedSelect()
    {
        setPreviousState();
        state = "speedSelect";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(speedSelectBox, canvas.transform);

        // this is the second speed indicator that appears in the top left 
        totalSpeedIndicator2 = Instantiate(totalSpeedPrefab, canvas.transform);
        
        // puts it right into place. please excuse my magic numbers
        totalSpeedIndicator2.transform.localPosition = new Vector3(
            currentDrawnBox.transform.localPosition[0] - 133,
            currentDrawnBox.transform.localPosition[1] - 104,
            0);

        // updates the text on the indicators 
        totalSpeedIndicator1.currentDisplayedSpeed = totalSpeedThisTurn;
        totalSpeedIndicator2.currentDisplayedSpeed = totalSpeedThisTurn;

        // the topSquare and bottomSqaure are white squares that go above the arrows drawn on the speedSelect
        // right now they are invisible 100% of the time lol
        topSquare = currentDrawnBox.transform.GetChild(2).gameObject;
        bottomSquare = currentDrawnBox.transform.GetChild(3).gameObject;
        speedSelectTextObject = currentDrawnBox.transform.GetChild(0).gameObject;

        speedForCurrentMove = 0;
    }

    void changeSpeed(int change)
    {
        // two things can cancel this function:
        // 1. if the speed is going DOWN
        //      the speed for the currently selected move cannot go below 0
        // 2. if the speed is going UP
        //      the total speed that you have allocated total (across all moves) cannot go above the maximum speed each turn
        // this prevents the speed from going out of bounds (like negative numbers or > 12)
        if ((change == -1 && speedForCurrentMove + change > -1) || (change == 1 && totalSpeedAllocatedThisTurn < totalSpeedEachTurn))
        {
            // change the speed for the current move
            speedForCurrentMove += change;

            // update that in the array of all the speed values for each move
            selectedSpeeds[numberOfSelectedMoves] = speedForCurrentMove;

            // calculate the total speed allocated based on the speed values for each move
            totalSpeedAllocatedThisTurn = selectedSpeeds[0] + selectedSpeeds[1] + selectedSpeeds[2];

            // see how much speed you have left to allocate based on the difference between the total speed and the speed you have allocated
            totalSpeedThisTurn = totalSpeedEachTurn - totalSpeedAllocatedThisTurn;


            // update the speed shown on the speedSelectBox to show the speed that is being put into the currently selected move
            speedSelectTextObject.GetComponent<TextMeshProUGUI>().text = speedForCurrentMove.ToString();

            // changes the total speed display
            totalSpeedIndicator1.currentDisplayedSpeed = totalSpeedThisTurn;
            totalSpeedIndicator2.currentDisplayedSpeed = totalSpeedThisTurn;

            selectedSpeeds[numberOfSelectedMoves] = speedForCurrentMove;
        }
    }

    // resets everything to do with speed
    // right now, this is called on transitionToRotate(), so that when a new turn starts, everything works again
    // in the future we can make this more sophisticated so that we can use it to partially reset the speed when we go back to a previous move
    // select and need to only reset some of the speed values
    void resetSpeed()
    {
        selectedSpeeds[0] = 0;
        selectedSpeeds[1] = 0;
        selectedSpeeds[2] = 0;
        speedForCurrentMove = 0;
        totalSpeedAllocatedThisTurn = 0;
        totalSpeedThisTurn = totalSpeedEachTurn;
        totalSpeedIndicator1.currentDisplayedSpeed = totalSpeedThisTurn;
    }

    void transitionToConfirm()
    {
        setPreviousState();
        state = "confirm";
        Destroy(currentDrawnBox);

        currentDrawnBox = Instantiate(confirmBox, canvas.transform);
        
        // changing confirm moves to each action 
            for(int i = 0; i < 3; i++){
            currentDrawnBox.transform.GetChild(i+2).gameObject.GetComponent<TextMeshProUGUI>().text = actions[i];   
        }
    }

    // playResults is also not really implemented yet
    void transitionToPlayResults()
    {
        setPreviousState();
        state = "playResults";
        Destroy(currentDrawnBox);

        currentDrawnBox = Instantiate(playResultsBox, canvas.transform);
        gameLoop.endTurn();
        StartCoroutine(gameLoop.OutputText());
    }

    void transitionToPause()
    {
        setPreviousState();
        state = "paused";
        Debug.Log(previousState);
        pauseMenuInstance = Instantiate(pauseMenu, canvas.transform);
    }

    void transitionFromPause()
    {
        Debug.Log("transitioned from pause");

        switch (previousState)
        {

            case "rotate":
                transitionToRotate();
                break;

            case "moveSelect":
                transitionToMoveSelect();
                break;

            case "targetSelect":
                transitionToTargetSelect();
                break;

            case "confirm":
                transitionToConfirm();
                break;

            // this should never happen
            case "playResults":
                transitionToPlayResults();
                break;

            // this should never happen
            default:
                break;

        }
        Destroy(pauseMenuInstance);
        justUnpaused = true;
    }

    void setPreviousState()
    {
        if (state != "paused")
        {
            previousState = state;
        }
    }
}
