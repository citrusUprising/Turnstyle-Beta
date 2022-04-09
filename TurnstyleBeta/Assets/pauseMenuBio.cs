using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenuBio : MonoBehaviour
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
        isShowing = pauseMenu.pauseMenuItemsShowing[4];
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.goBack();
            }
        }
    }
}
