using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class glossaryScript : MonoBehaviour
{
    public GameObject[] pagesRef;
    public List<GameObject> pagesReal;
    private GameObject Stats;

    public GameObject keyPrompt;

    public bool keyPromptIsShowing;

    public bool isShowing;

    public GameObject leftArrow;
    public GameObject rightArrow;

    private int currentPageIndex = 0;
    private int nextPageIndex = 0;

    private bool isAnimating = false;
    private string animateDirection = "left";

    private Vector3 offScreenLeft = new Vector3(-Screen.width*2, 0, 0);
    private Vector3 offScreenRight = new Vector3(Screen.width*2, 0, 0);
    private Vector3 center = new Vector3(0, 0, 0);
    private float t = 0f;

    private Vector3 keyPromptOnScreen = new Vector3(Screen.width - 176 - 65, 32, 0);
    private Vector3 keyPromptOffScreen = new Vector3(Screen.width - 176 - 65, -160, 0);

    private float keyPromptFlickerTime = .25f;

    public GameObject nextSFX;
    public GameObject prevSFX;
    public GameObject errorSFX;

    public GameObject whiteRectangle;

    // Start is called before the first frame update
    void Start()
    {
        Stats = GameObject.Find("CurrentStats");
        
        if(Stats.GetComponent<CurrentStats>().currentTutorial > 2){
            for(int i =0; i<pagesRef.Length; i++)
            pagesReal.Add(pagesRef[i]);
        }
        

        if (isShowing)
        {
            show();
            
            StartCoroutine(animateKeyPrompt());
        }
        else
        {
            hide();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isShowing)
        {
            offScreenLeft[1] = 0;
            offScreenRight[1] = 0;
            center[1] = 0;
        }
        else if (isShowing == false)
        {
            offScreenLeft[1] = Screen.height;
            offScreenRight[1] = Screen.height;
            center[1] = Screen.height;
        }

        if (isAnimating)
        {

            t += 2.5f * Time.deltaTime;

            animatePages();

            if (t > 1.0f)
            {
                t = 1.0f;

                animatePages();

                isAnimating = false;

                currentPageIndex = nextPageIndex;

            }
        }
        else if (isAnimating == false && isShowing == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nextPage(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextPage(1);
            }
        }
    }

    void nextPage(int direction)
    {
        if(pagesReal.Count > 1){
            isAnimating = true;

            t = 0.0f;

            if (direction > 0)
            {
                if (isShowing)
                {
                    nextSFX.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }

                nextPageIndex++;

                animateDirection = "right";

                nextPageIndex = nextPageIndex%pagesReal.Count;
            }
            else if (direction < 0)
            {
                if (isShowing)
                {
                    prevSFX.GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }

                nextPageIndex--;

                animateDirection = "left";

                nextPageIndex = (nextPageIndex+pagesReal.Count)%pagesReal.Count;
            }
        }else errorSFX.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    public void show()
    {
        Debug.Log ("Opening Glossary");
        isShowing = true;

        pagesReal[currentPageIndex].transform.localPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < pagesReal.Count; i++)
        {
            pagesReal[i].GetComponent<CanvasRenderer>().SetAlpha(1f);
        }

        changeArrowAlpha(1);

        whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(.5f);
 
    }

    public void hide()
    {   
        Debug.Log ("Closing Glossary");
        isShowing = false;

        for (int i = 0; i < pagesReal.Count; i++)
        {
            pagesReal[i].GetComponent<CanvasRenderer>().SetAlpha(0f);
        }

        changeArrowAlpha(0);

        whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(0f);
    }

    void animatePages()
    {
        if (animateDirection == "left")
        {
            // current one
            pagesReal[currentPageIndex].transform.localPosition = Vector3.Lerp(center, offScreenRight, t);

            // next one
            pagesReal[nextPageIndex].transform.localPosition = Vector3.Lerp(offScreenLeft, center, t);
        }
        else
        {
            // current one
            pagesReal[currentPageIndex].transform.localPosition = Vector3.Lerp(center, offScreenLeft, t);

            // next one
            pagesReal[nextPageIndex].transform.localPosition = Vector3.Lerp(offScreenRight, center, t);
        }
    }

    public void setPage(int page){
        page=page%pagesRef.Length;
        if(pagesReal.Count <= page){
            pagesReal.Add(pagesRef[page]);
            Debug.Log("Adding Page "+page);
    }

        if(page==(currentPageIndex-1+pagesReal.Count)%pagesReal.Count)
        pagesReal[currentPageIndex].transform.localPosition = offScreenRight;

        else if (page==(currentPageIndex+1)%pagesReal.Count)
        pagesReal[currentPageIndex].transform.localPosition = offScreenLeft;

        currentPageIndex = page;
        nextPageIndex = page;
    }

    public IEnumerator animateKeyPrompt()
    {

        StartCoroutine(lerpKeyPromptPosition());
        yield return new WaitForSeconds(keyPromptFlickerTime * 2);

        StartCoroutine(lerpKeyPromptAlpha(0f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(1f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(0f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(1f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(0f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(1f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(0f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(1f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(0f));
        yield return new WaitForSeconds(keyPromptFlickerTime);

        StartCoroutine(lerpKeyPromptAlpha(1f));
        yield return new WaitForSeconds(keyPromptFlickerTime);
    }

    IEnumerator lerpKeyPromptAlpha(float targetAlpha)
    {
        if (keyPrompt != null)
        {

            float time = 0f;
            float duration = keyPromptFlickerTime * 2;
            var color = keyPrompt.GetComponent<Image>().color;
            float oldAlpha = color.a;
            var currentAlpha = oldAlpha;

            while (time < duration)
            {
                currentAlpha = Mathf.Lerp(oldAlpha, targetAlpha, time / duration);

                time += Time.deltaTime;

                color.a = currentAlpha;

                if (keyPrompt != null)
                {
                    keyPrompt.GetComponent<Image>().color = color;
                }
                    

                yield return null;
            }

            color.a = targetAlpha;

            if (keyPrompt != null)
            {
                keyPrompt.GetComponent<Image>().color = color;
            } 
        }
        
    }

    IEnumerator lerpKeyPromptPosition()
    {

        if (keyPrompt != null)
        {

            float time = 0f;
            float duration = .75f;

            while (time < duration)
            {

                if (keyPrompt != null)
                {
                    keyPrompt.transform.localPosition = Vector3.Lerp(keyPromptOffScreen, keyPromptOnScreen, time/duration);
                }

                time += Time.deltaTime;

                yield return null;
            }

            if (keyPrompt != null)
            {
                keyPrompt.transform.position = keyPromptOnScreen;
            }
        }
    }

    void changeArrowAlpha(float alpha)
    {
        var color = rightArrow.GetComponent<Image>().color;

        color.a = alpha;

        rightArrow.GetComponent<Image>().color = color;
        leftArrow.GetComponent<Image>().color = color;
    }
}
