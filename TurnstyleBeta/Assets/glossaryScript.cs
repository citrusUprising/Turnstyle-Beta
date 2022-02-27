using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class glossaryScript : MonoBehaviour
{
    public GameObject[] pages;

    public GameObject keyPrompt;

    public bool keyPromptIsShowing;

    public bool isShowing;

    public GameObject leftArrow;
    public GameObject rightArrow;

    private int currentPageIndex = 0;
    private int nextPageIndex = 0;

    private bool isAnimating = false;
    private string animateDirection = "left";

    private Vector3 offScreenLeft = new Vector3(-Screen.width, 0, 0);
    private Vector3 offScreenRight = new Vector3(Screen.width, 0, 0);
    private Vector3 center = new Vector3(0, 0, 0);
    private float t = 0f;

    private Vector3 keyPromptOnScreen = new Vector3(16, 32, 0);
    private Vector3 keyPromptOffScreen = new Vector3(-273, -160, 0);
    private float t2 = 0f;

    private bool keyPromptDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
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
        else if (isAnimating == false)
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

        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Escape)) 
        {
            keyPromptDestroyed = true;
            Destroy(keyPrompt);
        }
    }

    void nextPage(int direction)
    {
        isAnimating = true;

        t = 0.0f;

        if (direction > 0)
        {
            nextPageIndex++;

            animateDirection = "right";

            if (nextPageIndex == pages.Length)
            {
                nextPageIndex = 0;
            }
        }
        else if (direction < 0)
        {
            nextPageIndex--;

            animateDirection = "left";

            if (nextPageIndex < 0)
            {
                nextPageIndex = pages.Length - 1;
            }
        }
    }

    public void show()
    {
        isShowing = true;

        pages[currentPageIndex].transform.localPosition = new Vector3(0, 0, 0);

        changeArrowAlpha(1);
 
    }

    public void hide()
    {
        isShowing = false;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].transform.localPosition = new Vector3(0, Screen.height, 0);
        }

        changeArrowAlpha(0);
    }

    void animatePages()
    {
        if (animateDirection == "left")
        {
            // current one
            pages[currentPageIndex].transform.localPosition = Vector3.Lerp(center, offScreenRight, t);

            // next one
            pages[nextPageIndex].transform.localPosition = Vector3.Lerp(offScreenLeft, center, t);
        }
        else
        {
            // current one
            pages[currentPageIndex].transform.localPosition = Vector3.Lerp(center, offScreenLeft, t);

            // next one
            pages[nextPageIndex].transform.localPosition = Vector3.Lerp(offScreenRight, center, t);
        }
    }

    public IEnumerator animateKeyPrompt()
    {

        keyPrompt.transform.position = keyPromptOnScreen;

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);

        toggleKeyPromptAlpha();
        yield return new WaitForSeconds(.5f);
    }

    void toggleKeyPromptAlpha()
    {
        if (keyPromptDestroyed == false)
        {
            var color = keyPrompt.GetComponent<Image>().color;

            if (color.a == 1f)
            {
                color.a = .5f;
            }
            else
            {
                color.a = 1f;
            }

            keyPrompt.GetComponent<Image>().color = color;
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
