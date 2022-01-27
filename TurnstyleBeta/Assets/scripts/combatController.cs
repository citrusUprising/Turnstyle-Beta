using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    public Object pentagonSprite;
    public Canvas canvas;
    public GameObject rotateBox;
    public GameObject moveSelectBox;
    public GameObject confirmBox;
    public GameObject playResultsBox;
    private string state;
    private GameObject currentDrawnBox;
    private int numberOfSelectedMoves = 0;
    private int selectedMove = 0;
    private int[] pointerCoords = new int[3] { -57, -81, -105};
    private GameObject moveSelectPointer; 

    // Start is called before the first frame update
    void Start()
    {
        // the available states so far are "rotate", "moveSelect", "targetSelect", "confirm", "playResults" (in that order)
        // "rotate" is for rotating the pentagon 
        // "moveSelect" is for selecting the move for a character
        // "targetSelect" is for selecting the target for the move slected in moveSelect
        // "confirm" is for reviewing and confirming the selected moves
        // "playResults" is when the player has no controls and the combat animations play out
        //
        // these go in the order of:
        // rotate -> moveSelect(1) -> targetSelect(1) -> moveSelect(2) -> targetSelect(2) -> moveSelect(3) -> targetSelect(3) -> confirm ->
        // EITHER moveSelect(1) OR playResults -> rotate REPEAT
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
                transitionToMoveSelect();
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

        // THIS NEEDS TO BE DELTED ONCE WE GET FURTHER ALONG
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
    }

    void transitionToRotate()
    {
        Destroy(currentDrawnBox);
        state = "rotate";
        currentDrawnBox = Instantiate(rotateBox, canvas.transform);
    }

    void transitionToMoveSelect()
    {
        state = "moveSelect";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(moveSelectBox, canvas.transform);
        selectedMove = 0;
        moveSelectPointer = currentDrawnBox.transform.GetChild(5).gameObject;
        moveSelectPointer.transform.localPosition = new Vector3(
            moveSelectPointer.transform.localPosition[0],
            pointerCoords[selectedMove],
            moveSelectPointer.transform.localPosition[2]);
        
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
        state = "targetSelect";
        // this state does not have a box associated with it. therefore, the old box should not be destroyed

        // so this logic should come when the player presses X to select a target
        // additionally, it should also do that if there is no target to select (like in the case of a move hitting all enemies/allies) 
        if (numberOfSelectedMoves == 3)
        {
            transitionToConfirm();
        } else
        {
            transitionToMoveSelect();
        }
        
    }

    void transitionToConfirm()
    {
        state = "confirm";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(confirmBox, canvas.transform);
    }

    // playResults is also not really implemented yet
    void transitionToPlayResults()
    {
        state = "playResults";
        Destroy(currentDrawnBox);
        currentDrawnBox = Instantiate(playResultsBox, canvas.transform);
    }
}
