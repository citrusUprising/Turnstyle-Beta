using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverScene : MonoBehaviour
{

    public GameObject pointer;

    public GameObject whiteRectangle;

    public GameObject restartLabel;
    public GameObject titleScreenLabel;

    public GameObject labelContainer;

    public GameObject gameOverText;

    private GameObject seletedLabel;

    private bool isLerping = false;

    private GameObject currentSelectedLabel;

    public Animator transitionAnimator;
    public GameObject transitionObject;

    public GameObject selectSound;
    public GameObject menuScroll;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lerpStuffOnScreen());

        currentSelectedLabel = restartLabel;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLerping == false)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)
                        || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                toggleSelectedLabel();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {

                selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                isLerping = true;

                if (currentSelectedLabel == restartLabel)
                {
                    StartCoroutine(restartFromCheckpoint());
                }
                else
                {
                    StartCoroutine(goToTitleScreen());
                }
            }
        }

        pointer.transform.position = currentSelectedLabel.transform.position;
    }

    IEnumerator goToTitleScreen()
    {
        transitionAnimator.SetTrigger("toBlack");
        
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator restartFromCheckpoint()
    {
        transitionAnimator.SetTrigger("toBlack");
        
        yield return new WaitForSeconds(.5f);
        PlayerPrefs.SetInt("Load", 1);
        SceneManager.LoadScene(1);
    }

    void toggleSelectedLabel()
    {
        if (currentSelectedLabel == restartLabel)
        {
            currentSelectedLabel = titleScreenLabel;
        }
        else
        {
            currentSelectedLabel = restartLabel;
        }
    }

    IEnumerator lerpStuffOnScreen()
    {
        float time = 0f;
        float duration = 1f;
        float t;

        isLerping = true;

        Vector3 oldGameOverPos = gameOverText.transform.position;
        Vector3 newGameOverPos = new Vector3(Screen.width / 2, (int)Screen.height * .75f, 0);

        Vector3 oldLabelContainerPos = labelContainer.transform.position;
        Vector3 newLabelContainerPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        int oldAlpha = 0;
        float newAlpha = .5f;

        while (time < duration)
        {
            t = time / duration;

            t = t * t * (3f - 2f * t);

            gameOverText.transform.position = Vector3.Lerp(oldGameOverPos, newGameOverPos, t);
            labelContainer.gameObject.transform.position = Vector3.Lerp(oldLabelContainerPos, newLabelContainerPos, t);
            whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(oldAlpha, newAlpha, t));

            Debug.Log(whiteRectangle.GetComponent<CanvasRenderer>().GetAlpha());

            time += Time.deltaTime;

            yield return null;
        }

        t = 1;

        gameOverText.transform.position = Vector3.Lerp(oldGameOverPos, newGameOverPos, t);
        labelContainer.transform.position = Vector3.Lerp(oldLabelContainerPos, newLabelContainerPos, t);
        whiteRectangle.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(oldAlpha, newAlpha, t));

        isLerping = false;
    }
}
