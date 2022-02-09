using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Station currentStation;
    Vector3 moveToPosition; 
    public float speed = 2f;
    int currentLine = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0,-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentLine++;
            if(currentLine == currentStation.destinations.Length)
            {
                currentLine = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentLine = currentLine - 1;
            if (currentLine == -1)
            {
                currentLine = currentStation.destinations.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveToStation(currentLine);
        }
        moveToPosition = currentStation.transform.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, moveToPosition, speed);
    }

    void moveToStation(int s)
    {
        currentStation = currentStation.destinations[s];
        currentLine = 0;
    }
}
