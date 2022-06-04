using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: 
// the local position of this object is derived from it's parent
// which SHOULD be somewhere near 0, uiHeight/2, 0
// so i have to take THAT into account too <3 <3 <3 <3
public class pauseMenuBio : MonoBehaviour
{

    public pauseMenu pauseMenu;

    private bool isShowing = false;

    public GameObject[] pages = new GameObject[5];

    public GameObject previewPentagon;

    public GameObject cornerPentagon;

    private GameObject currentPage;
    private GameObject prevPage;
    private int pageIndex = 0;

    // all of these Vector3s are written relative to the center of the screen
    // the y value changes when the items are being scrolled through on pauseMenu.cs

    // i am using these two because the screen width is not always the same as the UI width, but the UI width is constant
    private int uiWidth = 1600;
    private int uiHeight = 900;

    private Vector3 pageOffScreenLeft;
    private Vector3 pageOffScreenRight;
    private Vector3 pageOnScreen;

    // 725 is the width of the pentagon sprite
    // 120 is a bona fide magic number
    private Vector3 previewPentagonOffScreen;
    private Vector3 previewPentagonOnScreen;

    private Vector3 cornerPentagonOffScreen;
    private Vector3 cornerPentagonOnScreen;

    private Vector3 cornerPentagonRotation;
    private Vector3 previewPentagonRotation;

    private bool prevShowingState = false;

    private bool isLerping = false;

    private float timeSinceLastLerp = 0;
    private float timeBetweenLerps = 3f;

    public bool countingPreview = false;

    public GameObject rotateForwardSFX;
    public GameObject rotateBackSFX;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.transform.position = new Vector3(0, gameObject.transform.position[1], 0);

        pageOffScreenLeft = new Vector3(uiWidth * -1, 0, 0);
        pageOffScreenRight = new Vector3(uiWidth * 2, 0, 0);
        pageOnScreen = new Vector3(uiWidth/2, 0, 0);

        // 725 is the width of the pentagon sprite
        // 120 is a bona fide magic number
        previewPentagonOffScreen = new Vector3((float)(uiWidth + 725 / 2), 0, 0);
        previewPentagonOnScreen = new Vector3(uiWidth + 150, 0, 0);

        cornerPentagonOffScreen = new Vector3((float)(-725 / 2), (float)(uiHeight / 2 + 725 / 2), 0);
        cornerPentagonOnScreen = new Vector3((float)(0), (float)(uiHeight / 2), 0);

        currentPage = pages[pageIndex];

        previewPentagon.transform.localPosition = previewPentagonOnScreen;

        if (isShowing)
        {
            StartCoroutine(showAndHideLerp("show"));
        }
        else
        {
            StartCoroutine(showAndHideLerp("hide"));
        }

        prevShowingState = !isShowing;

        cornerPentagonRotation = cornerPentagon.transform.rotation.eulerAngles;
        previewPentagonRotation = previewPentagon.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        isShowing = pauseMenu.pauseMenuItemsShowing[4];
        if (isShowing)
        {
            if (isLerping == false)
            {
                if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseMenu.goBack();
                }

            
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    changePage(1);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    changePage(-1);
                }
            }
        }
        else
        {
            if (isLerping == false && countingPreview)
            {
                timeSinceLastLerp += Time.deltaTime;

                if (timeSinceLastLerp >= timeBetweenLerps && isLerping == false)
                {
                    changePage(1);
                    timeSinceLastLerp = 0f;
                }
            }
        }

        if (isShowing != prevShowingState)
        {
            if (isShowing == true)
            {
                StartCoroutine(showAndHideLerp("show"));
            }
            else
            {
                StartCoroutine(showAndHideLerp("hide"));
            }
        }

        prevShowingState = isShowing;
        
    }

    IEnumerator showAndHideLerp(string direction)
    {
        float time = 0;
        float duration = .5f;
        float t = time / duration;

        int count = 0;

        updatePages();

        Vector3 oldPreviewPentagonPos;
        Vector3 newPreviewPentagonPos;

        Vector3 oldCornerPentagonPos;
        Vector3 newCornerPentagonPos;

        isLerping = true;

        if (direction == "show")
        {
            oldPreviewPentagonPos = previewPentagon.transform.localPosition;
            newPreviewPentagonPos = previewPentagonOffScreen;

            oldCornerPentagonPos = cornerPentagon.transform.localPosition;
            newCornerPentagonPos = cornerPentagonOnScreen;

            cornerPentagon.GetComponent<CanvasRenderer>().SetAlpha(1);
            previewPentagon.GetComponent<CanvasRenderer>().SetAlpha(1);
        }
        else
        {
            oldPreviewPentagonPos = previewPentagon.transform.localPosition;
            newPreviewPentagonPos = previewPentagonOnScreen;

            oldCornerPentagonPos = cornerPentagon.transform.localPosition;
            newCornerPentagonPos = cornerPentagonOffScreen;

            cornerPentagon.GetComponent<CanvasRenderer>().SetAlpha(1);
            previewPentagon.GetComponent<CanvasRenderer>().SetAlpha(1);
        }

        while (time < duration)
        {
            t = time / duration;

            previewPentagon.transform.localPosition = Vector3.Lerp(oldPreviewPentagonPos, newPreviewPentagonPos, t);

            cornerPentagon.transform.localPosition = Vector3.Lerp(oldCornerPentagonPos, newCornerPentagonPos, t);

            time += Time.deltaTime;

            count++;

            yield return null;
        }

        t = 1;

        previewPentagon.transform.localPosition = Vector3.Lerp(oldPreviewPentagonPos, newPreviewPentagonPos, t);

        cornerPentagon.transform.localPosition = Vector3.Lerp(oldCornerPentagonPos, newCornerPentagonPos, t);

        if (direction == "show")
        {
            previewPentagon.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
        else
        {
            cornerPentagon.GetComponent<CanvasRenderer>().SetAlpha(0);
        }

        timeSinceLastLerp = 0f;

        isLerping = false;
    }

    IEnumerator lerpPages(int direction)
    {
        GameObject pageGoingOnScreen = currentPage;
        GameObject pageGoingOffScreen = prevPage;

        float time = 0f;
        float duration = .75f;

        isLerping = true;

        Vector3 oldPageGoingOffScreen;
        Vector3 newPageGoingOffScreen;

        Vector3 oldPageGoingOnScreen;
        Vector3 newPageGoingOnScreen;

        Quaternion oldCornerPentagonRotation = cornerPentagon.transform.rotation;
        Quaternion newCornerPentagonRotation = Quaternion.Euler(cornerPentagonRotation);

        Quaternion oldPreviewPentagonRotation = previewPentagon.transform.rotation;
        Quaternion newPreviewPentagonRotation = Quaternion.Euler(previewPentagonRotation);

        if (direction == 1)
        {
            oldPageGoingOffScreen = pageOnScreen;
            newPageGoingOffScreen = pageOffScreenRight;

            oldPageGoingOnScreen = pageOffScreenLeft;
            newPageGoingOnScreen = pageOnScreen;
        }
        else
        {
            oldPageGoingOffScreen = pageOnScreen;
            newPageGoingOffScreen = pageOffScreenLeft;

            oldPageGoingOnScreen = pageOffScreenRight;
            newPageGoingOnScreen = pageOnScreen;
        }

        while (time < duration)
        {

            float t = time / duration;

            t = easeInOutCubic(t);

            pageGoingOnScreen.transform.localPosition = Vector3.Lerp(oldPageGoingOnScreen, newPageGoingOnScreen, t);
            pageGoingOffScreen.transform.localPosition = Vector3.Lerp(oldPageGoingOffScreen, newPageGoingOffScreen, t);

            cornerPentagon.transform.rotation = Quaternion.Lerp(oldCornerPentagonRotation, newCornerPentagonRotation, t);
            previewPentagon.transform.rotation = Quaternion.Lerp(oldPreviewPentagonRotation, newPreviewPentagonRotation, t);

            time += Time.deltaTime;

            yield return null;
        }

        pageGoingOnScreen.transform.localPosition = newPageGoingOnScreen;
        pageGoingOffScreen.transform.localPosition = newPageGoingOffScreen;

        isLerping = false;

        timeSinceLastLerp = 0f;
    }

    void updatePages()
    {
        foreach (GameObject page in pages)
        {
            page.transform.localPosition = pageOffScreenLeft;
        }

        currentPage.transform.localPosition = pageOnScreen;
    }

    void changePage(int direction)
    {
        prevPage = pages[pageIndex];

        pageIndex += direction;

        if (pageIndex > pages.Length - 1)
        {
            pageIndex = 0;
        }
        else if (pageIndex < 0)
        {
            pageIndex = pages.Length - 1;
        }

        if (direction == 1)
        {
            rotateForwardSFX.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
        else
        {
            rotateBackSFX.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }

        currentPage = pages[pageIndex];

        cornerPentagonRotation = new Vector3(0, 0, cornerPentagon.transform.rotation.eulerAngles[2] + 72 * direction);
        previewPentagonRotation = new Vector3(0, 0, previewPentagon.transform.rotation.eulerAngles[2] + 72 * direction);

        StartCoroutine(lerpPages(direction));
    }

    // taken from easings.net
    float easeInOutCubic(float x) {
    
        return x< 0.5 ? 4 * x* x* x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;

    }
}
