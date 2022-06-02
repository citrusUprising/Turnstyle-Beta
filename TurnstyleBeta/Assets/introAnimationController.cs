using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introAnimationController : MonoBehaviour
{

    public introTextBox introTextBox;
    public introTextBox outroTextBox;

    public introPentagon introPentagon;

    public float animationInterval = 1f;
    private float originalAnimationInterval;
    private float animationGap;

    public GameObject whiteBox;

    public combatController combatController;

    private bool waitingForInput = false;

    public GameObject transitionObject;
    public Animator transitionAnimator;

    private bool isAnimating = false;

    public float timeMultiplier = 1f;
    public float fastTimeMultiplier = 1.5f;

    private float t = 0f;

    private bool pause = false;

    private bool isZPressed = false;
    private int numOfFramesZ;

    public int howLongZPressedToSkip = 30;

    public GameObject fastForwardText;

    private bool isNotOnOutroAnimation2 = true;

    // Start is called before the first frame update
    void Start()
    {
        updateAnimationInterval(animationInterval);

        originalAnimationInterval = animationInterval;

        whiteBox.GetComponent<CanvasRenderer>().SetAlpha(.25f);

        fastForwardText.GetComponent<CanvasRenderer>().SetAlpha(0);

        StartCoroutine(introAnimation());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isZPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isZPressed = false;
        }

        if (isZPressed && numOfFramesZ < howLongZPressedToSkip)
        {
            numOfFramesZ++;
        }
        else
        {
            numOfFramesZ = 0;
        }

        if (isAnimating == true && waitingForInput == false)
        {
            if (numOfFramesZ >= howLongZPressedToSkip)
            {
                if (timeMultiplier != fastTimeMultiplier)
                {
                    timeMultiplier = fastTimeMultiplier;

                    fastForwardText.GetComponent<CanvasRenderer>().SetAlpha(1);
                }
            }
            // ok so this checks if you press z, and if the multiplier wasn't already sped up, AND if the outro animation 2 isn't playing
            // the last part is so that if you press z to advance past the second outro, the fastForwardText won't show up as a result of
            // letting go of Z during that same z press
            // i feel like that guy who plays mario 64
            else if (Input.GetKeyUp(KeyCode.Z) && timeMultiplier != fastTimeMultiplier && isNotOnOutroAnimation2)
            {
                fastForwardText.GetComponent<CanvasRenderer>().SetAlpha(1);
            }
        }

        // this happens inbetween the two outro animations. it pauses only starts the second one after you press z
        if (waitingForInput)
        {
            fastForwardText.GetComponent<CanvasRenderer>().SetAlpha(0);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(outroAnimation2());
                waitingForInput = false;
            }
        }
    }

    void updateAnimationInterval(float newInterval)
    {
        animationInterval = newInterval;
        animationGap = animationInterval / 4;
    }

    void resetAnimationInterval()
    {
        animationInterval = originalAnimationInterval;
        updateAnimationInterval(animationInterval);
        timeMultiplier = 1;
        fastForwardText.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    void editAnimatingVars(bool value)
    {
        isAnimating = value;
        combatController.isIntroAnimating = value;
    }

    public void startOutroAnimation1()
    {
        StartCoroutine(outroAnimation1());
    }

    IEnumerator introAnimation()
    {
        // for details on these functions, see introTextBox.cs, introPentagon.cs, and introTriangle.cs

        editAnimatingVars(true);

        yield return new WaitForSeconds(.5f);

        introTextBox.startAnimation(animationInterval, 0);

        introPentagon.startTriAnimation(animationInterval, true);

        while (t < animationInterval + animationGap)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        t = 0f;

        introPentagon.startRotateAnimation(animationInterval * 2, 4);

        while (t < animationInterval + animationGap)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        t = 0f;

        updateAnimationInterval(animationInterval / 2);

        introTextBox.startAnimation(animationInterval, 1);

        introPentagon.isGoingOnScreen = false;
        introPentagon.startMoveAnimation(animationInterval);

        StartCoroutine(lerpWhiteBoxAlpha(animationInterval, .25f, 0f));

        while (t < animationInterval)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        t = 0f;

        resetAnimationInterval();

        editAnimatingVars(false);
    }

    IEnumerator outroAnimation1()
    {
        // for details on these functions, see introTextBox.cs, introPentagon.cs, and introTriangle.cs

        editAnimatingVars(true);

        StartCoroutine(lerpWhiteBoxAlpha(animationInterval, 0f, .25f));

        outroTextBox.startAnimation(animationInterval, 0);

        introPentagon.startTriAnimation(0f, true);

        introPentagon.isGoingOnScreen = true;
        introPentagon.startMoveAnimation(animationInterval);

        while (t < animationInterval + animationGap)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        t = 0f;

        waitingForInput = true;
    }

    IEnumerator outroAnimation2()
    {

        isNotOnOutroAnimation2 = false;

        introPentagon.startRotateAnimation(animationInterval, 2);

        while (t < animationInterval)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        updateAnimationInterval(animationInterval / 2);

        outroTextBox.startAnimation(animationInterval, 1);

        introPentagon.startTriAnimation(animationInterval, false);

        while (t < animationInterval)
        {
            t += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        t = 0f;

        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(.5f);

        combatController.isIntroAnimating = false;
        SceneManager.UnloadSceneAsync("tutorialScene");

        yield return new WaitForEndOfFrame();

        GameObject.Find("NodeMapCamera").GetComponent<CameraController>().isTransitioningFromAnotherScene = true;
        Destroy(gameObject);

        // so these are all useless because the game object is being destroyed but it feels tidy to include them anyway
        resetAnimationInterval();

        editAnimatingVars(false);

        isNotOnOutroAnimation2 = true;
    }

    IEnumerator lerpWhiteBoxAlpha(float duration, float oldAlpha, float newAlpha)
    {
        float time = 0f;

        float alpha;

        while (time < duration)
        {

            float t = time / duration;

            alpha = Mathf.Lerp(oldAlpha, newAlpha, t);

            whiteBox.GetComponent<CanvasRenderer>().SetAlpha(alpha);

            time += Time.deltaTime * timeMultiplier;

            yield return null;
        }

        whiteBox.GetComponent<CanvasRenderer>().SetAlpha(newAlpha);
    }

    // this is not currently being used but i wanted to keep it around because i think it is interesting
    /*
    IEnumerator introAnimationOld()
    {

        // for details on these functions, see introTextBox.cs, introPentagon.cs, and introTriangle.cs

        introTextBox.startAnimation(animationInterval, 0);

        introPentagon.startRotateAnimation(animationInterval, 4);

        introPentagon.startTriAnimation(animationInterval, true);

        yield return new WaitForSeconds(animationInterval);
        yield return new WaitForSeconds(animationGap);

        updateAnimationInterval(animationInterval / 2);

        introTextBox.startAnimation(animationInterval, 1);

        introPentagon.startRotateAnimation(animationInterval, 2);

        introPentagon.startMoveAnimation(animationInterval);

        StartCoroutine(lerpWhiteBoxAlpha(animationInterval, .25f, 0f));
    }
    */
}
