using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject confirmBox;
    public GameObject playResultsBox;
    public GameObject pauseMenu;
    public GameObject gameLoop;
    private string state = "rotate";

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
    private int selectedMove = 0;

    // hard-coded coordinates for the selector sprite
    private int[] pointerCoords = new int[3] { -57, -81, -105 };

    // the selector sprite
    private GameObject moveSelectPointer;


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
    static float t = 0.0f;

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
    private nameTag[] nameTagArray = new nameTag[5];

    // --------------------------------------------------------- //
    // used in the paused state
    // --------------------------------------------------------- //
    private GameObject pauseMenuInstance;
    private bool justUnpaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // the available states so far are "rotate", "moveSelect", "targetSelect", "confirm", "playResults", "paused" (in that order)
        // "rotate" is for rotating the pentagon 
        // "moveSelect" is for selecting the move for a character
        // "targetSelect" is for selecting the target for the move slected in moveSelect
        // "confirm" is for reviewing and confirming the selected moves
        // "playResults" is when the player has no controls and the combat animations play out
        // "paused" is when the pause menu is opened. it can be accessed by any state other than "playResults" and goes back to that state
        //
        // these go in the order of:
        // rotate -> moveSelect(1) -> targetSelect(1) -> moveSelect(2) -> targetSelect(2) -> moveSelect(3) -> targetSelect(3) -> confirm ->
        // EITHER moveSelect(1) OR playResults -> rotate REPEAT
        transitionToRotate();

        // 3 and 4 are inactive, 0, 1, and 2 are active
        nameTagArray[0] = nameTagBeverly;
        nameTagArray[1] = nameTagAmery;
        nameTagArray[2] = nameTagKoralie;
        nameTagArray[3] = nameTagJade;
        nameTagArray[4] = nameTagSeraphim;

        for (int i = 0; i < 5; i++)
        {
            nameTagCoords[i] = nameTagArray[i].transform.position;
        }

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
                changeSelectedMove(1);
            }
            // when the up arrow is pressed, move the selection up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                changeSelectedMove(-1);
            }
            // when the X key is pressed, we need to go to selecting targets
            if (Input.GetKeyDown(KeyCode.X))
            {
                transitionToTargetSelect();
            }
        } 

        else if (state == "targetSelect")
        {

        }

        else if (state == "confirm")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                transitionToPlayResults();
            }
        }

        // THIS NEEDS TO BE DELETED ONCE WE GET FURTHER ALONG
        // in the play results state, the player should have no inputs. they should only proceed once all the results are finished displaying. 
        // at that point, the turn is complete and the state should be rotate
        // cuz we don't have all that done rn, i'm just using this for the moment for bug testing and such
        else if (state == "playResults")
        {
            
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
    }

    void rotatePentagon()
    {
        // converts the Vector3 rotatoins to Quarternion
        Quaternion oldRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, pentagonRotation);
        Quaternion newRotation = Quaternion.Euler(pentagonRotateX, pentagonRotateY, nextPentagonRotation);

        // lerps the rotation of the pentagon to the next rotation
        pentagonSprite.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);


        // so this should be replaced with a lerp, something along the lines of 
        // Vector3.Lerp( SOMETHING , nameTagCoords[i] , t );
        for (int i = 0; i < 5; i++)
        {
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
        Destroy(currentDrawnBox);
        setPreviousState();
        state = "rotate";
        currentDrawnBox = Instantiate(rotateBox, canvas.transform);
    }

    void transitionToMoveSelect()
    {
        setPreviousState();
        state = "moveSelect";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(moveSelectBox, canvas.transform);
        selectedMove = 0;
        moveSelectPointer = currentDrawnBox.transform.GetChild(5).gameObject;
        moveSelectPointer.transform.localPosition = new Vector3(
            moveSelectPointer.transform.localPosition[0],
            pointerCoords[selectedMove],
            moveSelectPointer.transform.localPosition[2]);
        
        // can change to string/etc later, 
        // currently only sets player name & abilities based on num input, abilities are empty strings rnow
        // i cannot get the image to swap for the life of me
        currentDrawnBox.GetComponent<PlayerMoveSelect>().ChangeColor(2);

        // we need logic to change the sprite of the currentDrawnBox to match the color of the current character
        // who is having their moves selected for them
        // also the name of the moves and the descriptions of the moves should change to match the next character
    }

    void changeSelectedMove(int change)
    {
        selectedMove += change;
        if (selectedMove == 3)
        {
            selectedMove = 0;
        }
        if (selectedMove == -1)
        {
            selectedMove = 2;
        }

        // move the cursor up or down to the next move
        moveSelectPointer.transform.localPosition = new Vector3(
            moveSelectPointer.transform.localPosition[0], 
            pointerCoords[selectedMove], 
            moveSelectPointer.transform.localPosition[2]);

        // we also have to replace the description of the move with the description of the selected move
        
    }

    // because i have not implemented this yet, it will go automatically to the next moveSelect
    void transitionToTargetSelect()
    {
        numberOfSelectedMoves++;
        setPreviousState();
        state = "targetSelect";
        // this state does not have a box associated with it. therefore, the old box should not be destroyed

        // so this logic should come when the player presses X to select a target
        // additionally, it should also do that if there is no target to select (like in the case of a move hitting all enemies/allies) 
        if (numberOfSelectedMoves == 3)
        {
            transitionToConfirm();
        } 
        else
        {
            transitionToMoveSelect();
        }
        
    }

    void transitionToConfirm()
    {
        setPreviousState();
        state = "confirm";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(confirmBox, canvas.transform);
    }

    // playResults is also not really implemented yet
    void transitionToPlayResults()
    {
        setPreviousState();
        state = "playResults";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(playResultsBox, canvas.transform);
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
