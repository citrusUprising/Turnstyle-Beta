using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialSegment{
    public string[] text;
    public int[] highlights;
    public string trigger;
    public TutorialSegment (string[] text, int[] highlights, string trigger){
        this.text = text;
        this.highlights = highlights;
        this.trigger = trigger;
    }
}

public class combatController : MonoBehaviour
{
    // --------------------------------------------------------- //
    // variables that effect/control tutorials
    // these shouldn't matter in normal combat scene, other than isTutorial
    // --------------------------------------------------------- //
    public bool isTutorial;
    public GameObject tutorialHandler;

    // --------------------------------------------------------- //
    // variables that interact with states in general
    // there is an explanation of all the states at the 
    // beginning of Start()
    // --------------------------------------------------------- //
    public GameObject pentagonSprite;
    public Canvas canvas;
    public Canvas glossaryCanvas;
    public GameObject rotateBox;
    public GameObject moveSelectBox;
    public GameObject speedSelectBox;
    public GameObject confirmBox;
    public GameObject playResultsBox;
    public GameObject pauseMenu;
    public GameObject mainLoopObject;
    public totalSpeed totalSpeedPrefab;
    public GameObject glossary;
    private string state = "rotate";
    private bool xDown; //if confirm is pressed, to prevent multi-read errors
    public bool combatDone; //used to tell if combat is done
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
    private int[] pointerCoords = new int[3] { -52, -76, -100 };

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
    // These are used for facilitating what part of the tutorial
    // is visible and when the tutorial pops up
    // --------------------------------------------------------- //
        private GameObject Stats;
        private int currentTutorial;
        public bool statused;

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

    private GameObject glossaryObject;

    // --------------------------------------------------------- //
    // GameObjects that hold FMOD Studio Event Emitters for playing SFX
    // --------------------------------------------------------- //
    public GameObject turnstyleRotate;
    public GameObject menuForward;
    public GameObject menuBack;
    public GameObject menuScroll;
    public GameObject speedScroll;
    public GameObject selectSound;

    private bool isSceneOverlayActive = false;

    // Start is called before the first frame update
    void Start()
    {

        totalSpeedIndicator1 = Instantiate(totalSpeedPrefab, canvas.transform);
        totalSpeedIndicator1.GetComponent<Transform>().position = new Vector3(224, 310, 0);

        // 3 and 4 are inactive, 0, 1, and 2 are active
        nameTagArray[0] = nameTagBeverly;
        nameTagArray[1] = nameTagAmery;
        nameTagArray[2] = nameTagKoralie;
        nameTagArray[3] = nameTagJade;
        nameTagArray[4] = nameTagSeraphim;

        // init each player's moves here ⬇ this code is ugly but it works
        nameTagArray[0].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Smolder(), new Dazzle(), new Imbibe()}; 
        nameTagArray[1].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Mitigate(), new Unionize(), new Scrum()};
        nameTagArray[2].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Repel(), new Hunker(), new Crush()};
        nameTagArray[3].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Stunnerclap(), new Rally(), new Motivate()};
        nameTagArray[4].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities = new Ability[]{new Soulrip(), new Scry(), new Slump()};

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < 5; i++)
        {
            nameTagCoords[i] = nameTagArray[i].transform.localPosition;
        }

        gameLoop = mainLoopObject.GetComponent<MainLoop>();

        Stats = GameObject.Find("CurrentStats");

        targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
        
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
         //pulls from current stats object, and sets the checker to what's in current stats
        transitionToRotate();

        glossaryObject = Instantiate(glossary, glossaryCanvas.transform);

        glossaryObject.GetComponent<glossaryScript>().nextSFX = menuForward;
        glossaryObject.GetComponent<glossaryScript>().prevSFX = menuBack;
        glossaryObject.GetComponent<glossaryScript>().errorSFX = speedScroll;


        //Hides glossary
        glossaryObject.GetComponent<glossaryScript>().hide();
        //glossaryPopUp(0);

        //Sets tutorial
        this.isTutorial = Stats.GetComponent<CurrentStats>().isTutorial;
        statused = false;

        xDown = false;
        combatDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSceneOverlayActive == false)
        {
            if (isTutorial)
                tutorialHandler.GetComponent<tutorialHandler>().open(0);

            if (Input.GetKeyDown(KeyCode.X))
            {
                xDown = true;
            }

            //Disables Tutorial Logic if there is no tutorial
            if (isTutorial)
            {
                int bookTemp = tutorialHandler.GetComponent<tutorialHandler>().bookCount;
                int pageTemp = tutorialHandler.GetComponent<tutorialHandler>().pageCount;


                if (tutorialHandler.GetComponent<tutorialHandler>().isOpen)
                {
                    //Changes accepted input to continue
                    switch (tutorialHandler.GetComponent<tutorialHandler>().allTutorials[bookTemp][pageTemp].trigger)
                    {
                        case "xDown":
                            if (xPress())
                            {
                                tutorialHandler.GetComponent<tutorialHandler>().nextPage();
                            }
                            break;

                        case "ArrowKeys":
                            xDown = false;
                            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                                Input.GetKeyDown(KeyCode.DownArrow) ||
                                Input.GetKeyDown(KeyCode.LeftArrow) ||
                                Input.GetKeyDown(KeyCode.RightArrow))
                                tutorialHandler.GetComponent<tutorialHandler>().nextPage();
                            break;

                        case "G":
                            xDown = false;
                            if (Input.GetKeyDown(KeyCode.G))
                                tutorialHandler.GetComponent<tutorialHandler>().nextPage();
                            break;

                        default:
                            break;
                    }
                }
            }

            if (glossaryObject.GetComponent<glossaryScript>().isShowing == false)
            {

                if (pauseMenuInstance == null)
                {
                    // if the state is "rotate," than the available controls are up and down to rotate and Z to advance to move select
                    if (state == "rotate")
                    {
                        // if you press X, advance to the next state, destroying the rotate UI and replacing it with move select UI
                        if (xPress() && isRotating == false)
                        {
                            menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            gameLoop.setActiveUnits(nameTagArray);
                            Debug.Log("Setting Active Units");
                            Color temp = this.pentagonSprite.GetComponent<Image>().color;
                            temp.a = 0.0f;
                            this.pentagonSprite.GetComponent<Image>().color = temp;
                            transitionToMoveSelect();
                        }
                        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            // if the pentagon is NOT rotating, then begin rotating DOWN
                            if (!isRotating)
                            {
                                turnstyleRotate.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                                beginRotatingPentagon(-1);
                            }

                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            // if the pentagon IS rotating, then begin rotating UP
                            if (!isRotating)
                            {
                                turnstyleRotate.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                                beginRotatingPentagon(1);
                            }
                        }
                        // here is all the logic 
                        // if (isRotating)
                        // {
                        //     rotatePentagon();
                        // }
                    }

                    else if (state == "moveSelect")
                    {
                        // when the down arrow is pressed, move the selection down
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            changeSelectedAbilityIndex(1);
                            // nameTagArray[numberOfSelectedMoves].GetComponent<PlayerMoveSelect>().movePointer(1);

                        }
                        // when the up arrow is pressed, move the selection up
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            changeSelectedAbilityIndex(-1);
                            // nameTagArray[numberOfSelectedMoves].GetComponent<PlayerMoveSelect>().movePointer(-1);
                        }
                        // when the X key is pressed, we need to go to selecting targets
                        if (xPress())
                        {
                            menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            selectedAbility = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>().abilities[selectedAbilityIndex];
                            actions[numberOfSelectedMoves] += selectedAbility.name + " on ";
                            transitionToTargetSelect();
                        }
                        // back function needs to be implemented
                        // should go back to rotate if numberOfSelectedMoves is 0 and back to moveSelect if it is greater than zero
                        // if it goes back to moveSelect, then it should -- numberOfSelectedMoves
                        else if (Input.GetKeyDown(KeyCode.Z))
                        {
                            //menuBack.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            if (numberOfSelectedMoves == 0)
                            {
                                transitionToRotate();
                            }
                            else
                            {
                                numberOfSelectedMoves--;
                                transitionToMoveSelect();
                            }
                        }
                    }

                    // needs to be implemented
                    else if (state == "targetSelect")
                    {
                        // when the down arrow is pressed, move the selection down
                        if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            Debug.Log("previous enemy");
                            changeSelectedTarget(-1, selectedAbility.allies);
                            // change target   
                        }
                        // when the up arrow is pressed, move the selection up
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            Debug.Log("next enemy");
                            changeSelectedTarget(1, selectedAbility.allies);
                        }
                        // when the X key is pressed, we need to go to selecting speed
                        if (xPress() && !selectedTarget.dead)
                        {
                            menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
                            transitionToSpeedSelect();
                        }
                        else if (xPress())
                        {
                            speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX //flag
                        }
                        else if (Input.GetKeyDown(KeyCode.Z))
                        {
                            // possibly hide cursor here
                            targetPointer.GetComponent<CanvasRenderer>().SetAlpha(0);
                            transitionToMoveSelect();
                        }
                    }

                    else if (state == "speedSelect")
                    {
                        // this confirms the selected speed and goes to the next state
                        // if there have been three selected moves, then it goes to the confirm state
                        // if there have not, it goes to move select
                        if (xPress())
                        {
                            menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX

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
                            selectedSpeeds[numberOfSelectedMoves] = 0;
                            transitionToTargetSelect();
                        }

                        // changes the speed of the selected move up by one
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            changeSpeed(1);
                        }

                        // changes it down by one
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            changeSpeed(-1);
                        }

                    }

                    else if (state == "confirm")
                    {
                        if (xPress())
                        {
                            if (selectedAbility.allies)
                            {
                                actions[numberOfSelectedMoves - 1] += nameTagArray[targetIndex].GetComponent<nameTag>().character.GetComponent<Friendly>().name;
                            }
                            else
                            {
                                actions[numberOfSelectedMoves - 1] += enemies[targetIndex].GetComponent<Enemy>().name;
                            }
                            menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
                            transitionToPlayResults();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z))
                        {
                            menuBack.GetComponent<FMODUnity.StudioEventEmitter>().Play(); //play SFX
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

                        if (Input.GetKeyDown(KeyCode.C) && !combatDone)//flag
                        {
                            mainLoopObject.GetComponent<MainLoop>().isSkipped = true;
                            transitionToRotate();
                        }
                        else if (Input.GetKeyDown(KeyCode.C))
                        {
                            transitionToRotate();
                        }
                        else if (xPress() && combatDone)
                        {
                            transitionToRotate();
                        }
                    }

                    if (state != "playResults")
                    {
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            pauseMenuInstance = Instantiate(pauseMenu, glossaryCanvas.transform);
                        }
                    }
                }
            }

            if (isRotating == false)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (glossaryObject.GetComponent<glossaryScript>().isShowing)
                    {
                        speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                        glossaryObject.GetComponent<glossaryScript>().hide();
                    }
                    else if (glossaryObject.GetComponent<glossaryScript>().isShowing == false)
                    {
                        speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                        glossaryObject.GetComponent<glossaryScript>().show();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape) || xPress() || Input.GetKeyDown(KeyCode.Z))
                {
                    if (glossaryObject.GetComponent<glossaryScript>().isShowing)
                    {
                        speedScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                        glossaryObject.GetComponent<glossaryScript>().hide();
                    }
                }
            }

            justUnpaused = false;
        }
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
            nameTagArray[i].previousPosition = nameTagArray[i].transform.localPosition;
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

        StartCoroutine(rotatePentagon());
    }

    IEnumerator rotatePentagon()
    {
        float time = 0f;
        float duration = .5f;

        // converts the Vector3 rotatoins to Quarternion
        Quaternion oldRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, pentagonRotation);
        Quaternion newRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, nextPentagonRotation);

        while (time < duration)
        {
            float t = time / duration;

            t = easeInOutCubic(t);

            // lerps the rotation of the pentagon to the next rotation
            pentagonSprite.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);

            for (int i = 0; i < 5; i++)
            {
                nameTagArray[i].transform.localPosition = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
            }

            time += Time.deltaTime;

            yield return null;
        }

        t = 1f;

        pentagonSprite.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);

        for (int i = 0; i < 5; i++)
        {
            nameTagArray[i].transform.localPosition = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
        }

        // the pentagon is done rotating by now
        isRotating = false;

        pentagonRotation = nextPentagonRotation;
    }

    void rotatePentagonOld()
    {
        
        // converts the Vector3 rotatoins to Quarternion
        Quaternion oldRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, pentagonRotation);
        Quaternion newRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, nextPentagonRotation);

        // lerps the rotation of the pentagon to the next rotation
        pentagonSprite.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);

        for (int i = 0; i < 5; i++)
        {
            nameTagArray[i].transform.localPosition = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
        }

        // increases time
        t += 2.5f * Time.deltaTime;

        // when this happens, this function should no longer be called
        if (t > 1.0f)
        {
            // caps time at 1.0f
            t = 1.0f;

            for (int i = 0; i < 5; i++)
            {
                nameTagArray[i].transform.localPosition = Vector3.Lerp(nameTagArray[i].previousPosition, nameTagArray[i].nextPosition, t);
            }

            // the pentagon is done rotating by now
            isRotating = false;

            // update the rotation variable to the current rotation, that way next time the pentagon rotates, it lerps from the 
            // proper value
            pentagonRotation = nextPentagonRotation;
        }
        
    }

    public void transitionToRotate()
    {

        combatDone = false;
        Color temp = this.pentagonSprite.GetComponent<Image>().color;
        temp.a = 1.0f;
        this.pentagonSprite.GetComponent<Image>().color = temp;

        numberOfSelectedMoves = 0;
        Destroy(currentDrawnBox);
        setPreviousState();
        state = "rotate";
        currentDrawnBox = Instantiate(rotateBox, canvas.transform);
        resetSpeed();

        nameTagArray[0].showPassive(); // show
        nameTagArray[1].showPassive(); // show
        nameTagArray[2].showPassive(); // show
        //glossaryPopUp(2);

        if(isTutorial&&statused)
        tutorialHandler.GetComponent<tutorialHandler>().open(2);
    }

    void transitionToMoveSelect()
    {
        setPreviousState();
        state = "moveSelect";
        Destroy(currentDrawnBox);
        Debug.Log("Dead Move Select: " + nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>().dead);
        while (nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Friendly>().dead)
        {
            numberOfSelectedMoves++;
            if (numberOfSelectedMoves == 3)
            {
                transitionToConfirm();
                return;
            }
        }
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
        actions[numberOfSelectedMoves] += selectedUnit.name+ ": ";//flag

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
        /*int speedMinus = nameTagArray[0].GetComponent<nameTag>().character.GetComponent<Unit>().fatigue;
        int speedPlus = 0;
        //checks if Unit has haste and if so adds magnitude to speed indicator
        if(nameTagArray[0].GetComponent<nameTag>()
            .character.GetComponent<Unit>().statuses[(int)StatusType.Buff].name == StatusName.Haste)
            speedPlus = nameTagArray[0].GetComponent<nameTag>().character.GetComponent<Unit>().statuses[(int)StatusType.Buff].magnitude;
        */
        setPreviousState();
        state = "speedSelect";
        Destroy(currentDrawnBox);
        speedSelectBox.GetComponent<Image>().sprite = nameTagArray[numberOfSelectedMoves].speedSelectImage;
        currentDrawnBox = Instantiate(speedSelectBox, canvas.transform);

        totalSpeedPrefab.GetComponent<Image>().sprite = nameTagArray[numberOfSelectedMoves].totalSpeedImage;
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

        int hasteSpeed = 0;
            if(nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().statuses[(int)StatusType.Buff].name == StatusName.Haste)
            hasteSpeed = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().statuses[(int)StatusType.Buff].magnitude;
        int tempSpeed = hasteSpeed - nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().fatigue;;
        speedSelectTextObject.GetComponent<TextMeshProUGUI>().text = tempSpeed.ToString();
        speedForCurrentMove = 0;

        //glossaryPopUp(1);
        if(isTutorial) tutorialHandler.GetComponent<tutorialHandler>().open(1);
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
            //grabs magnitude of current Haste
            int hasteSpeed = 0;
            if(nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().statuses[(int)StatusType.Buff].name == StatusName.Haste)
            hasteSpeed = nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().statuses[(int)StatusType.Buff].magnitude;

            //creates variable, accounting for current speed, haste boost, and fatigue penalty
            int truSpeed = speedForCurrentMove 
            + hasteSpeed
            - nameTagArray[numberOfSelectedMoves].GetComponent<nameTag>().character.GetComponent<Unit>().fatigue;
            
            speedSelectTextObject.GetComponent<TextMeshProUGUI>().text = truSpeed.ToString();

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
        
        currentDrawnBox.transform.GetChild(0).GetComponent<Image>().color = nameTagArray[0].confirmColor;
        currentDrawnBox.transform.GetChild(1).GetComponent<Image>().color = nameTagArray[1].confirmColor;
        currentDrawnBox.transform.GetChild(2).GetComponent<Image>().color = nameTagArray[2].confirmColor;

        Debug.Log(nameTagArray[0].confirmColor);
        Debug.Log(nameTagArray[1].confirmColor);
        Debug.Log(nameTagArray[2].confirmColor);

        Debug.Log(currentDrawnBox.transform.GetChild(0).GetComponent<Image>().color);
        Debug.Log(currentDrawnBox.transform.GetChild(1).GetComponent<Image>().color);
        Debug.Log(currentDrawnBox.transform.GetChild(2).GetComponent<Image>().color);

        // changing confirm moves to each action 
        for (int i = 0; i < 3; i++){
            Friendly displayedUnit = nameTagArray[i].GetComponent<nameTag>().character.GetComponent<Friendly>();
            if (displayedUnit.dead){
                currentDrawnBox.transform.GetChild(i+4).gameObject.GetComponent<TextMeshProUGUI>().text = displayedUnit.unitName + " is KO'd!";
                continue;
            }
            string actionDescription = displayedUnit.unitName + ": " + displayedUnit.queuedAction.ability.name + " on ";
            if(displayedUnit.queuedAction.ability.multitarget){
                if(displayedUnit.queuedAction.ability.allies)
                    actionDescription += "allies.";
                else
                    actionDescription += "enemies.";
            }
            else
                actionDescription += displayedUnit.queuedAction.target.unitName + ".";
            currentDrawnBox.transform.GetChild(i+4).gameObject.GetComponent<TextMeshProUGUI>().text = actionDescription;   
        }
    }

    // playResults is also not really implemented yet
    void transitionToPlayResults()
    {
        setPreviousState();
        state = "playResults";
        Destroy(currentDrawnBox);

        currentDrawnBox = Instantiate(playResultsBox, canvas.transform);
        gameLoop.queueEnemyActions();
        StartCoroutine(gameLoop.OutputText());
    }

    void transitionToPause()
    {
        setPreviousState();
        state = "paused";
        pauseMenuInstance = Instantiate(pauseMenu, glossaryCanvas.transform);
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

    private bool xPress(){
        if (xDown){
            xDown = false;
            return true;
        }else return false;
    }

    void setPreviousState()
    {
        if (state != "paused")
        {
            previousState = state;
        }
    }

    /*void glossaryPopUp (int check){
        Debug.Log("Current Tutorial is "+currentTutorial);
        if(check==currentTutorial){
            Debug.Log("opening glossary page "+check);
            glossaryObject.GetComponent<glossaryScript>().setPage(check);
            glossaryObject.GetComponent<glossaryScript>().show();
            currentTutorial++;
        }
        Stats.GetComponent<CurrentStats>().currentTutorial = currentTutorial;
    }*/

    // taken from easings.net and modified for c#
    // taken from easings.net
    float easeInOutCubic(float x)
    {

        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;

    }

    public void setPage (int page){
        glossaryObject.GetComponent<glossaryScript>().setPage(page);
    }
}
