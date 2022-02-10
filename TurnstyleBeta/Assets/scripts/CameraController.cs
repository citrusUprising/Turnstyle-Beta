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
    public float height;
    public Vector3 scale; 
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0, height);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
            currentLine++;
            if(currentLine == currentStation.destinations.Length)
            {
                currentLine = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentStation.destinations[currentLine].transform.localScale = new Vector3(1,1,1);
            currentLine = currentLine - 1;
            if (currentLine == -1)
            {
                currentLine = currentStation.destinations.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentStation.transform.localScale = new Vector3(1, 1, 1);
            moveToStation(currentLine);
        }
        currentStation.destinations[currentLine].transform.localScale = scale;
        moveToPosition = currentStation.transform.position + new Vector3(0, 0, height);
        transform.position = Vector3.Lerp(transform.position, moveToPosition, speed);
    }

    void moveToStation(int s)
    {
        currentStation = currentStation.destinations[s];
        currentLine = 0;
    }
}
