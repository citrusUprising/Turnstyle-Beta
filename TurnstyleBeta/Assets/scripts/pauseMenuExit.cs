using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuExit : MonoBehaviour
{

    public pauseMenu pauseMenu;

    private bool isShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.goBack();
            }
        }
        isShowing = pauseMenu.pauseMenuItemsShowing[0];
    }
}
