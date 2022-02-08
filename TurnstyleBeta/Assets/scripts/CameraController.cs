using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Station currentStation;
    Vector3 moveToPosition; 
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0,-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveToStation(0);
        }
        moveToPosition = currentStation.transform.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, moveToPosition, speed);
    }

    void moveToStation(int s)
    {
        currentStation = currentStation.destinations[s];
    }
}
