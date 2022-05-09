using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    private GameObject canvas;

    public GameObject[] items;

    private int currentSelectedItem = 0;
    private int nextSelectedItem = 0;

    public GameObject pentagon;

    private bool isRotating = false;

    private float rotateDuration = .25f;

    private int offScreenTop = Screen.height*2;
    private int offScreenBottom = -Screen.height;
    private int onScreen = Screen.height/2;

    private int selectedX = Screen.width / 2;
    private int unselectedX = (Screen.width - 600) / 2 + 600;
    private int currentX = 0;

    private bool isItemSelected = false;

    private GameObject currentSelectedItemObject;

    public GameObject whiteRectangle;

    private Vector3 pentagonSelected = new Vector3(150, 0, 0);
    private Vector3 pentagonUnselected = new Vector3(0, 0, 0);

    private bool isItemSelectionAnimating = false;

    public bool[] pauseMenuItemsShowing = new bool[5];

    public GameObject selectSound;
    public GameObject rotateSound;
    public GameObject backSound;

    private bool isLerpingOnScreen = false;

    public GameObject pointer;

    public GameObject bioObject;

    private keyPromptManager promptManager;

    // Start is called before the first frame update
    void Start()
    {
        promptManager = GetComponent<keyPromptManager>();

        promptManager.changePrompt(0);

        canvas = GameObject.Find("glossaryCanvas");

        currentX = selectedX;

        items[currentSelectedItem].GetComponent<RectTransform>().position = new Vector3(unselectedX, onScreen, 0);

        StartCoroutine(lerpEverything("on screen"));
    }

    // Update is called once per frame
    void Update()
    {

        selectedX = Screen.width / 2;
        unselectedX = (Screen.width - 600) / 2 + 600;

        offScreenTop = Screen.height * 2;
        offScreenBottom = -Screen.height;
        onScreen = Screen.height / 2;

        if (isItemSelected)
        {
            currentX = selectedX;
        }
        else
        {
            currentX = unselectedX;
        }

        if (isRotating == false && isLerpingOnScreen == false)
        {
            if (currentSelectedItemObject == null)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rotate(-1);
                    rotateSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    rotate(1);
                    rotateSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }

                if (Input.GetKeyDown(KeyCode.X) && isItemSelectionAnimating == false)
                {
                    pauseMenuItemsShowing[currentSelectedItem] = true;
                    currentSelectedItemObject = items[currentSelectedItem];

                    StartCoroutine(lerpCurrentSelectedItemLeft());
                    
                    selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Z))
                {
                    StartCoroutine(lerpEverything("off screen"));
                }

            }
        }

        bioObject.GetComponent<pauseMenuBio>().countingPreview = currentSelectedItem == 4;
    }

    void rotate(int direction)
    {
        if (isRotating == false)
        {
            nextSelectedItem += direction;

            if (nextSelectedItem == 5)
            {
                nextSelectedItem = 0;
            }
            else if (nextSelectedItem == -1)
            {
                nextSelectedItem = 4;
            }

            Quaternion oldRotation = Quaternion.Euler(0, 0, currentSelectedItem * 72);
            Quaternion newRotation = Quaternion.Euler(0, 0, nextSelectedItem * 72);

            StartCoroutine(lerpPentagonRotation(oldRotation, newRotation));
            StartCoroutine(lerpItems(direction));
            currentSelectedItem = nextSelectedItem;
        }
    }

    public void goBack()
    {
        pauseMenuItemsShowing[currentSelectedItem] = false;

        StartCoroutine(lerpCurrentSelectedItemRight());

        backSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();

        Vector3 pointerPos = new Vector3(Screen.width * -2, Screen.height * -2, 0);
        pointer.GetComponent<RectTransform>().position = pointerPos;
    }

    public void realignEverything()
    {

        selectedX = Screen.width / 2;
        unselectedX = (Screen.width - 600) / 2 + 600;

        offScreenTop = Screen.height * 2;
        offScreenBottom = -Screen.height;
        onScreen = Screen.height / 2;

        currentSelectedItemObject.transform.position = new Vector3(currentX, onScreen, 0);
    }

    IEnumerator lerpPentagonRotation(Quaternion oldRotation, Quaternion newRotation)
    {
        isRotating = true;

        float time = 0f;
        float duration = rotateDuration;

        while (time < duration)
        {

            float t = time / duration;

            //t = t * t * (3f - 2f * t);

            pentagon.GetComponent<RectTransform>().rotation = Quaternion.Lerp(oldRotation, newRotation, t);

            time += Time.deltaTime;

            yield return null;
        }

        pentagon.GetComponent<RectTransform>().rotation = newRotation;

        isRotating = false;
    }

    IEnumerator lerpItems(int direction)
    {
        GameObject itemGoingOnScreen = items[nextSelectedItem];
        GameObject itemGoingOffScreen = items[currentSelectedItem];

        float time = 0f;
        float duration = rotateDuration;

        int itemGoingOnScreenX = currentX;
        int itemGoingOffScreenX = currentX;

        int itemGoingOffScreenY = offScreenTop;
        int itemGoingOnScreenY = offScreenBottom;

        if (direction == 1)
        {
            itemGoingOffScreenY = offScreenTop;
            itemGoingOnScreenY = offScreenBottom;
        } 
        else
        {
            itemGoingOffScreenY = offScreenBottom;
            itemGoingOnScreenY = offScreenTop;
        }

        Vector3 itemGoingOnScreenOld = new Vector3(currentX, itemGoingOnScreenY, 0);
        Vector3 itemGoingOnScreenNew = new Vector3(currentX, onScreen, 0);

        Vector3 itemGoingOffScreenOld = new Vector3(currentX, onScreen, 0);
        Vector3 itemGoingOffScreenNew = new Vector3(currentX, itemGoingOffScreenY, 0);

        while (time < duration)
        {

            float t = time / duration;

            t = t * t * (3f - 2f * t);

            itemGoingOnScreen.transform.position = Vector3.Lerp(itemGoingOnScreenOld, itemGoingOnScreenNew, t);
            itemGoingOffScreen.transform.position = Vector3.Lerp(itemGoingOffScreenOld, itemGoingOffScreenNew, t);

            bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);

            time += Time.deltaTime;

            yield return null;
        }

        itemGoingOnScreen.GetComponent<RectTransform>().position = itemGoingOnScreenNew;
        itemGoingOffScreen.GetComponent<RectTransform>().position = itemGoingOffScreenNew;

        bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);
    }

    IEnumerator lerpCurrentSelectedItemLeft()
    {
        float time = 0f;
        float duration = .25f;

        Vector3 oldVector3 = currentSelectedItemObject.GetComponent<RectTransform>().position;
        Vector3 newVector3 = new Vector3(selectedX, onScreen, 0);

        Vector3 oldPentagonPos = pentagon.transform.localPosition;
        Vector3 newPentagonPos = pentagonUnselected;

        if (currentSelectedItemObject == bioObject)
        {
            newPentagonPos = new Vector3(-725, 0, 0);
            duration = .5f;
        }

        currentSelectedItemObject.GetComponent<Canvas>().sortingOrder = 11;
        pentagon.GetComponent<Canvas>().sortingOrder = 9;
        whiteRectangle.GetComponent<Canvas>().sortingOrder = 10;

        isItemSelectionAnimating = true;

        while (time < duration)
        {
            currentSelectedItemObject.GetComponent<RectTransform>().position = Vector3.Lerp(oldVector3, newVector3, time / duration);

            pentagon.GetComponent<RectTransform>().localPosition = Vector3.Lerp(oldPentagonPos, newPentagonPos, time / duration);

            bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);

            time += Time.deltaTime;

            yield return null;
        }

        currentSelectedItemObject.GetComponent<RectTransform>().position = newVector3;

        bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);

        // this is really messy code rn but it works
        // this toggles the selection on the menu item twice IF that menu item is the text speed controls
        // the function of this is to make the animation of the preview text speed play as soon as that item is selected
        if (currentSelectedItem == 3)
        {
            currentSelectedItemObject.GetComponent<pauseMenuTextSpeed>().toggleSelectedLabel();
            currentSelectedItemObject.GetComponent<pauseMenuTextSpeed>().toggleSelectedLabel();
        }

        isItemSelectionAnimating = false;

        promptManager.changePrompt(currentSelectedItem + 1);
    }

    IEnumerator lerpCurrentSelectedItemRight()
    {
        float time = 0f;
        float duration = .25f;

        Vector3 oldVector3 = currentSelectedItemObject.GetComponent<RectTransform>().position;
        Vector3 newVector3 = new Vector3(unselectedX, onScreen, 0);

        Vector3 oldPentagonPos = pentagon.transform.localPosition;
        Vector3 newPentagonPos = pentagonSelected;

        if (currentSelectedItemObject == bioObject)
        {
            duration = .5f;
        }

        currentSelectedItemObject.GetComponent<Canvas>().sortingOrder = 9;
        pentagon.GetComponent<Canvas>().sortingOrder = 11;
        whiteRectangle.GetComponent<Canvas>().sortingOrder = 10;

        isItemSelectionAnimating = true;

        while (time < duration)
        {
            currentSelectedItemObject.GetComponent<RectTransform>().position = Vector3.Lerp(oldVector3, newVector3, time / duration);

            pentagon.GetComponent<RectTransform>().localPosition = Vector3.Lerp(oldPentagonPos, newPentagonPos, time / duration);

            bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);

            time += Time.deltaTime;

            yield return null;
        }

        currentSelectedItemObject.GetComponent<RectTransform>().position = newVector3;

        bioObject.transform.position = new Vector3(0, bioObject.transform.position[1], 0);

        isItemSelectionAnimating = false;

        currentSelectedItemObject = null;

        promptManager.changePrompt(0);
    }

    // this gets called in the start function of this object and in the context this object is created in to destroy it
    public IEnumerator lerpEverything(string direction = "on screen")
    {
        isLerpingOnScreen = true;

        float time = 0f;
        float duration = .25f;

        
        Vector3 oldPentagonPosition = pentagonSelected;
        oldPentagonPosition[0] -= Screen.width / 2;
        Vector3 newPentagonPosition = pentagonSelected;

        Vector3 oldItemPosition = items[currentSelectedItem].GetComponent<RectTransform>().position;
        oldItemPosition[0] += Screen.width / 2;
        Vector3 newItemPosition = items[currentSelectedItem].GetComponent<RectTransform>().position;

        float oldRectangleAlpha = 0;
        float newRectangleAlpha = 1;
        

        if (direction == "off screen")
        {
            newPentagonPosition = pentagonSelected;
            newPentagonPosition[0] -= Screen.width / 2;
            oldPentagonPosition = pentagonSelected;

            newItemPosition = items[currentSelectedItem].GetComponent<RectTransform>().position;
            newItemPosition[0] += Screen.width / 2;
            oldItemPosition = items[currentSelectedItem].GetComponent<RectTransform>().position;

            newRectangleAlpha = 0;
            oldRectangleAlpha = 1;
        }

        while (time < duration)
        {

            float t = time / duration;

            t = t * t * (3f - 2f * t);

            pentagon.GetComponent<RectTransform>().localPosition = Vector3.Lerp(oldPentagonPosition, newPentagonPosition, t);

            items[currentSelectedItem].GetComponent<RectTransform>().position = Vector3.Lerp(oldItemPosition, newItemPosition, t);

            whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(oldRectangleAlpha, newRectangleAlpha, t));

            time += Time.deltaTime;

            yield return null;
        }

        pentagon.GetComponent<RectTransform>().localPosition = newPentagonPosition;

        items[currentSelectedItem].GetComponent<RectTransform>().position = newItemPosition;

        whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(newRectangleAlpha);

        if (direction == "off screen")
        {
            Destroy(this.gameObject);
        }

        isLerpingOnScreen = false;
    }

    void closeMenu()
    {
        Destroy(this.gameObject);
    }
}
