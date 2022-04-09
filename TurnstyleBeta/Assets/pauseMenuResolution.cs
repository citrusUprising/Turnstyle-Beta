using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TODO
// fix logic around pop up
// fix bug with getResolution fucking it up some times
// have the pause menu realign all items when the resolution changes
// be sure to test on a build often!

public class pauseMenuResolution : MonoBehaviour
{

    public pauseMenu pauseMenu;

    private bool isShowing = false;

    public GameObject pointer;

    public GameObject labelResolution;
    public GameObject labelFullScreen;
    public GameObject labelApplyChanges;

    private GameObject selectedLabel;
    private int selectedLabelIndex;
    private GameObject[] labels = new GameObject[3];

    public GameObject resolutionText;
    public GameObject fullScreenButton;

    private Resolution res;
    private Resolution[] possibleRes;
    private int resIndex;

    private bool isFullScreen;

    public GameObject selectSound;
    public GameObject scrollSound;
    public GameObject menuForward;

    public GameObject popUp;
    private bool isPopUpShowing = false;

    private bool isGoingBack = false;

    // Start is called before the first frame update
    void Start()
    {
        possibleRes = Screen.resolutions;

        selectedLabel = labelResolution;

        labels[0] = labelResolution;
        labels[1] = labelFullScreen;
        labels[2] = labelApplyChanges;

        // set the labels to be accurate to what is currently the case
        res = Screen.currentResolution;

        updateResolutionText();

        while (possibleRes[resIndex].width != res.width && possibleRes[resIndex].refreshRate != res.refreshRate)
        {
            editResolution(1);
        }

        isFullScreen = Screen.fullScreen;
        updateFullScreenIcon();
    }

    // Update is called once per frame
    void Update()
    {
        isShowing = pauseMenu.pauseMenuItemsShowing[2];

        if (isShowing)
        {
            if (isPopUpShowing)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    hidePopUp();
                    applyChanges();
                    isGoingBack = true;
                    pauseMenu.goBack();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    hidePopUp();
                    isGoingBack = true;
                    pauseMenu.goBack();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Escape))
                {
                    if (checkForChanges())
                    {
                        showPopUp();
                    }
                    else
                    {
                        isGoingBack = true;
                        pauseMenu.goBack();
                    }
                }


                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    changeSelectedLabel(-1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    changeSelectedLabel(1);
                }

                if (selectedLabel == labelResolution)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        editResolution(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        editResolution(-1);
                    }
                }
                else if (selectedLabel == labelFullScreen)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.X))
                    {
                        toggleFullScreen();
                    }
                }
                else if (selectedLabel == labelApplyChanges)
                {
                    if (checkForChanges() && Input.GetKeyDown(KeyCode.X))
                    {
                        applyChanges();
                    }
                }
            }
            if (isGoingBack == false)
            {
                realignPointer();
            }
        }
        else
        {
            // this is so that when you first open the menu, the selected label is resolution
            // this is to fix a problem where opening the menu when the fullscreen was selected would also toggle fullscreen
            selectedLabelIndex = 0;
            selectedLabel = labels[selectedLabelIndex];
        }

        if (checkForChanges() == false)
        {
            labelApplyChanges.GetComponent<TextMeshProUGUI>().color = new Color(.3f, .3f, .3f, 1);
        }
        else
        {
            labelApplyChanges.GetComponent<TextMeshProUGUI>().color = Color.black;
        }

        isGoingBack = false;
    }

    void changeSelectedLabel(int direction)
    {

        selectedLabelIndex += direction;

        if (selectedLabelIndex <= -1)
        {
            selectedLabelIndex = labels.Length - 1;
        }
        else if (selectedLabelIndex >= labels.Length)
        {
            selectedLabelIndex = 0;
        }

        if (selectedLabelIndex == 2 && checkForChanges() == false)
        {
            if (direction == -1)
            {
                selectedLabelIndex = 1;
            }
            else if (direction == 1)
            {
                selectedLabelIndex = 0;
            }
        }

        selectedLabel = labels[selectedLabelIndex];

        scrollSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void editResolution(int direction)
    {
        resIndex += direction;
        if (resIndex >= possibleRes.Length)
        {
            resIndex = 0;
        }
        else if (resIndex <= -1)
        {
            resIndex = possibleRes.Length - 1;
        }

        res = possibleRes[resIndex];

        updateResolutionText();

        scrollSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void updateResolutionText()
    {
        resolutionText.GetComponent<TextMeshProUGUI>().text = res.width + "x" + res.height + "<br>" + res.refreshRate + "Hz";
    }

    void toggleFullScreen()
    {
        isFullScreen = !isFullScreen;

        updateFullScreenIcon();

        scrollSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void updateFullScreenIcon()
    {
        if (isFullScreen)
        {
            fullScreenButton.GetComponent<CanvasRenderer>().SetAlpha(1);
        }
        else
        {
            fullScreenButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
    }

    bool checkForChanges()
    {

        // Screen.fullScreen is the current full screen status
        // isFullScreen is whether the fullScreen bubble is filled or not
        if (Screen.fullScreen != isFullScreen)
        {
            return true;
        }

        if (res.width != Screen.width || res.height != Screen.height)
        {
            return true;
        }

        if (res.refreshRate != Screen.currentResolution.refreshRate)
        {
            return true;
        }

        return false;
    }

    void applyChanges()
    {
        if (checkForChanges() == true)
        {
            Screen.SetResolution(res.width, res.height, isFullScreen);

            PlayerPrefs.SetInt("width", res.width);
            PlayerPrefs.SetInt("height", res.height);
            PlayerPrefs.SetInt("refreshRate", res.refreshRate);

            if (isFullScreen == true)
            {
                PlayerPrefs.SetString("fullScreen", "yes");
            }
            else if (isFullScreen == false)
            {
                PlayerPrefs.SetString("fullScreen", "no");
            }

            selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
    }

    void realignPointer()
    {
        Vector3 pointerPos = selectedLabel.GetComponent<RectTransform>().position;

        var rect = gameObject.GetComponent<RectTransform>().rect;

        pointerPos[0] += (float)rect.left;

        if (isPopUpShowing)
        {
            pointerPos[0] -= Screen.width;
        }

        pointer.GetComponent<RectTransform>().position = pointerPos;
    }

    void showPopUp()
    {
        isPopUpShowing = true;
        popUp.GetComponent<CanvasRenderer>().SetAlpha(1);

        menuForward.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void hidePopUp()
    {
        isPopUpShowing = false;
        popUp.GetComponent<CanvasRenderer>().SetAlpha(0);
    }
}