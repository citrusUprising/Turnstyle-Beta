using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject exitPopUp;
    private GameObject canvas;

    private bool isExitPopUpOpen = false;
    private GameObject exitPopUpInstance;

    // Start is called before the first frame update
    void Start()
    {
        isExitPopUpOpen = false;
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isExitPopUpOpen == false)
            {
                closeMenu();
                closeExitPopUp();
            }
            else
            {
                closeExitPopUp();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            closeExitPopUp();
            closeMenu();
        }

        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (!isExitPopUpOpen)
            {
                openExitPopUp();
            } 
            else
            {
                Application.Quit();
            }
        }
    }

    void openExitPopUp()
    {

        isExitPopUpOpen = true;
        exitPopUpInstance = Instantiate(exitPopUp, canvas.transform);
    }
    
    void closeExitPopUp()
    {
        isExitPopUpOpen = false;
        Destroy(exitPopUpInstance);
    }

    void closeMenu()
    {
        Debug.Log("here");
        Destroy(this.gameObject);
    }
}
