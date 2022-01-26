using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class combatController : MonoBehaviour
{
    public Object pentagonSprite;
    public Canvas canvas;
    public GameObject rotateBox;
    public GameObject moveSelectBox;
    public GameObject confirmBox;
    public string state;
    private GameObject currentDrawnBox;

    // Start is called before the first frame update
    void Start()
    {
        // the available states so far are "rotate", "moveSelect", and "confirm" (in that order)
        // "rotate" is for rotating the pentagon 
        // "moveSelect" is for selecting the move for a character
        // "confirm" is for reviewing and confirming the selected moves
        // i am gonna add a state for displaying combat results later on 
        state = "rotate";
        currentDrawnBox = Instantiate(rotateBox, canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // if the state is "rotate," than the available controls are up and down to rotate and Z to advance to move select
        if (state == "rotate")
        {
            // if you press Z, advance to the next state, destroying the rotate UI and replacing it with move select UI
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Destroy(currentDrawnBox);
                currentDrawnBox = Instantiate(moveSelectBox, canvas.transform);
                state = "moveSelect";
            }
        } else if (state == "moveSelect")
        {

        } else if (state == "confirm")
        {

        }
    }
}
