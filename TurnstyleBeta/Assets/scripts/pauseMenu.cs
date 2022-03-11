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


    // Start is called before the first frame update
    void Start()
    {

        canvas = GameObject.Find("glossaryCanvas");

        currentX = selectedX;

        items[currentSelectedItem].GetComponent<RectTransform>().position = new Vector3(unselectedX, onScreen, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isItemSelected)
        {
            currentX = selectedX;
        }
        else
        {
            currentX = unselectedX;
        }

        if (currentSelectedItemObject == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rotate(1);
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                rotate(-1);
            }

            if (Input.GetKeyDown(KeyCode.X) && isItemSelectionAnimating == false)
            {
                pauseMenuItemsShowing[currentSelectedItem] = true;
                currentSelectedItemObject = items[currentSelectedItem];
                StartCoroutine(lerpCurrentSelectedItemLeft());
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Z))
            {
                Destroy(gameObject);
            }

        }
        else if (Input.GetKeyDown(KeyCode.Z) && isItemSelectionAnimating == false)
        {
            pauseMenuItemsShowing[currentSelectedItem] = false;
            StartCoroutine(lerpCurrentSelectedItemRight());
        }
        
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

        Vector3 itemGoingOnScreenOld = new Vector3(itemGoingOnScreenX, itemGoingOnScreenY, 0);
        Vector3 itemGoingOnScreenNew = new Vector3(itemGoingOnScreenX, onScreen, 0);

        Vector3 itemGoingOffScreenOld = new Vector3(itemGoingOffScreenX, onScreen, 0);
        Vector3 itemGoingOffScreenNew = new Vector3(itemGoingOffScreenX, itemGoingOffScreenY, 0);

        while (time < duration)
        {

            float t = time / duration;

            t = t * t * (3f - 2f * t);

            itemGoingOnScreen.GetComponent<RectTransform>().position = Vector3.Lerp(itemGoingOnScreenOld, itemGoingOnScreenNew, t);
            itemGoingOffScreen.GetComponent<RectTransform>().position = Vector3.Lerp(itemGoingOffScreenOld, itemGoingOffScreenNew, t);

            time += Time.deltaTime;

            yield return null;
        }

        itemGoingOnScreen.GetComponent<RectTransform>().position = itemGoingOnScreenNew;
        itemGoingOffScreen.GetComponent<RectTransform>().position = itemGoingOffScreenNew;
    }

    IEnumerator lerpCurrentSelectedItemLeft()
    {
        float time = 0f;
        float duration = .25f;

        Vector3 oldVector3 = currentSelectedItemObject.GetComponent<RectTransform>().position;
        Vector3 newVector3 = new Vector3(selectedX, onScreen, 0);

        currentSelectedItemObject.GetComponent<Canvas>().sortingOrder = 11;
        pentagon.GetComponent<Canvas>().sortingOrder = 9;
        whiteRectangle.GetComponent<Canvas>().sortingOrder = 10;

        isItemSelectionAnimating = true;

        while (time < duration)
        {
            currentSelectedItemObject.GetComponent<RectTransform>().position = Vector3.Lerp(oldVector3, newVector3, time / duration);

            pentagon.GetComponent<RectTransform>().localPosition = Vector3.Lerp(pentagonSelected, pentagonUnselected, time / duration);

            time += Time.deltaTime;

            yield return null;
        }

        currentSelectedItemObject.GetComponent<RectTransform>().position = newVector3;

        isItemSelectionAnimating = false;
    }

    IEnumerator lerpCurrentSelectedItemRight()
    {
        float time = 0f;
        float duration = .25f;

        Vector3 oldVector3 = currentSelectedItemObject.GetComponent<RectTransform>().position;
        Vector3 newVector3 = new Vector3(unselectedX, onScreen, 0);

        currentSelectedItemObject.GetComponent<Canvas>().sortingOrder = 9;
        pentagon.GetComponent<Canvas>().sortingOrder = 11;
        whiteRectangle.GetComponent<Canvas>().sortingOrder = 10;

        isItemSelectionAnimating = true;

        while (time < duration)
        {
            currentSelectedItemObject.GetComponent<RectTransform>().position = Vector3.Lerp(oldVector3, newVector3, time / duration);

            pentagon.GetComponent<RectTransform>().localPosition = Vector3.Lerp(pentagonUnselected, pentagonSelected, time / duration);

            time += Time.deltaTime;

            yield return null;
        }

        currentSelectedItemObject.GetComponent<RectTransform>().position = newVector3;

        isItemSelectionAnimating = false;

        currentSelectedItemObject = null;
    }

    void closeMenu()
    {
        Destroy(this.gameObject);
    }
}
