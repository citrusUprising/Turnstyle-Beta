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
    private float animationGap;

    public GameObject whiteBox;

    public combatController combatController;

    private bool waitingForInput = false;

    public GameObject transitionObject;
    public Animator transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        updateAnimationInterval(animationInterval);

        whiteBox.GetComponent<CanvasRenderer>().SetAlpha(.25f);

        StartCoroutine(introAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(outroAnimation2());
            waitingForInput = false;
        }
    }

    void updateAnimationInterval(float newInterval)
    {
        animationInterval = newInterval;
        animationGap = animationInterval / 4;
    } 

    public void startOutroAnimation1()
    {
        StartCoroutine(outroAnimation1());
    }

    IEnumerator introAnimation()
    {
        // for details on these functions, see introTextBox.cs, introPentagon.cs, and introTriangle.cs

        yield return new WaitForSeconds(.5f);

        combatController.isIntroAnimating = true;

        introTextBox.startAnimation(animationInterval, 0);

        introPentagon.startTriAnimation(animationInterval, true);

        yield return new WaitForSeconds(animationInterval);
        yield return new WaitForSeconds(animationGap);

        introPentagon.startRotateAnimation(animationInterval * 2, 4);

        yield return new WaitForSeconds(animationInterval);
        yield return new WaitForSeconds(animationGap);

        updateAnimationInterval(animationInterval / 2);

        introTextBox.startAnimation(animationInterval, 1);

        introPentagon.isGoingOnScreen = false;
        introPentagon.startMoveAnimation(animationInterval);

        StartCoroutine(lerpWhiteBoxAlpha(animationInterval, .25f, 0f));

        yield return new WaitForSeconds(animationInterval);
        combatController.isIntroAnimating = false;

        updateAnimationInterval(animationInterval * 2);

        
    }

    IEnumerator outroAnimation1()
    {
        // for details on these functions, see introTextBox.cs, introPentagon.cs, and introTriangle.cs

        combatController.isIntroAnimating = true;

        StartCoroutine(lerpWhiteBoxAlpha(animationInterval, 0f, .25f));

        outroTextBox.startAnimation(animationInterval, 0);

        introPentagon.startTriAnimation(0f, true);

        introPentagon.isGoingOnScreen = true;
        introPentagon.startMoveAnimation(animationInterval);

        yield return new WaitForSeconds(animationInterval);
        yield return new WaitForSeconds(animationGap);

        waitingForInput = true;
    }

    IEnumerator outroAnimation2()
    {
        introPentagon.startRotateAnimation(animationInterval, 2);

        yield return new WaitForSeconds(animationInterval);

        updateAnimationInterval(animationInterval / 2);

        outroTextBox.startAnimation(animationInterval, 1);

        introPentagon.startTriAnimation(animationInterval, false);

        yield return new WaitForSeconds(animationInterval);

        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(.5f);

        combatController.isIntroAnimating = false;
        SceneManager.UnloadSceneAsync("tutorialScene");
        Destroy(gameObject);

        updateAnimationInterval(animationInterval * 2);
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

            time += Time.deltaTime;

            yield return null;
        }

        whiteBox.GetComponent<CanvasRenderer>().SetAlpha(newAlpha);
    }

    // this is not currently being used but i wanted to keep it around because i think it is interesting
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
}
