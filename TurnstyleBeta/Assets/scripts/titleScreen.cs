using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreen : MonoBehaviour
{

    public GameObject logo;
    public GameObject transitionObject;
    public Animator transitionAnimator;
    public float transitionTime = .5f;
    
    public GameObject controlsText;
    public GameObject creditsText;
    public GameObject exit;
    public GameObject settings;
    public GameObject start;

    private int selectedOption = 0;
    public GameObject[] options;

    private float rotateDirection;

    public GameObject credits;
    private GameObject creditsObject;

    public GameObject pauseMenu;
    private GameObject pauseMenuObject;

    public GameObject controls;
    private GameObject controlsObject;

    public Canvas canvas;

    // GameObjects that hold FMOD Studio Event Emitters for playing SFX
    public GameObject menuScroll;
    public GameObject selectSound;

    public GameObject pointer;

    private bool isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        randomizeRotateDirection();

        gameObject.transform.Rotate(0, 0, Random.Range(0, 5) * 72);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, .25f*rotateDirection);

        if (creditsObject == null && controlsObject == null && pauseMenuObject == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Play menu scroll sfx
                menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                selectedOption--;
                if (selectedOption == -1)
                {
                    selectedOption = options.Length - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Play menu scroll sfx
                menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                selectedOption++;
                if (selectedOption == options.Length)
                {
                    selectedOption = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // Play select sound sfx
                selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                if (options[selectedOption] == start)
                {
                    StartCoroutine(loadScene(2));
                }

                else if (options[selectedOption] == controlsText)
                {
                    controlsObject = Instantiate(controls, canvas.transform);
                    StartCoroutine(lerpObjectOnScreen(controlsObject));
                }

                else if (options[selectedOption] == settings)
                {
                    pauseMenuObject = Instantiate(pauseMenu, canvas.transform);
                }

                else if (options[selectedOption] == creditsText)
                {
                    creditsObject = Instantiate(credits, canvas.transform);
                    StartCoroutine(lerpObjectOnScreen(creditsObject));
                }

                else if (options[selectedOption] == exit)
                {
                    Application.Quit();
                }
            }

            pointer.transform.localPosition = new Vector3(
                pointer.transform.localPosition[0],
                options[selectedOption].transform.localPosition[1] - 140,
                pointer.transform.localPosition[2]);
        }

        else
        {
            if (creditsObject != null && (Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Z)) || (Input.GetKeyDown(KeyCode.X)))
            {
                if (isLerping == false)
                {
                    StartCoroutine(lerpObjectOffScreen(creditsObject));
                }
            }

            if (controlsObject != null && (Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Z)) || (Input.GetKeyDown(KeyCode.X)))
            {
                if (isLerping == false)
                {
                    StartCoroutine(lerpObjectOffScreen(controlsObject));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
        if (Random.Range(0f, 1f) >= .9)
            {
                randomizeRotateDirection();
            }
        }

        
    }

    void randomizeRotateDirection()
    {
        rotateDirection = Random.Range(0f, 1f);
        
        if (rotateDirection > .5)
        {
            rotateDirection = 1;
        }
        else if (rotateDirection < .5)
        {
            rotateDirection = -1;
        }
    }

    IEnumerator loadScene(int index)
    {
        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
    }

    IEnumerator lerpObjectOnScreen(GameObject obj)
    {
        isLerping = true;

        if (obj != null)
        {
            float time = 0f;
            float duration = 1f;

            Vector3 previousLocation = new Vector3(1024, 0, 0);
            Vector3 nextLocation = new Vector3(0, 0, 0);

            while (time < duration)
            {

                float t = time / duration;

                t = t * t * (3f - 2f * t);

                if (obj != null)
                {
                    obj.transform.localPosition = Vector3.Lerp(previousLocation, nextLocation, t);
                }
                time += Time.deltaTime;

                yield return null;
            }

            if (obj != null)
            {
                obj.transform.localPosition = nextLocation;
            }
        }

        isLerping = false;
    }

    IEnumerator lerpObjectOffScreen(GameObject obj)
    {
        isLerping = true;

        if (obj != null)
        {

            float time = 0f;
            float duration = 1f;

            Vector3 previousLocation = new Vector3(0, 0, 0);
            Vector3 nextLocation = new Vector3(-1024, 0, 0);

            while (time < duration)
            {

                float t = time / duration;

                t = t * t * (3f - 2f * t);

                if (obj != null)
                {
                    obj.transform.localPosition = Vector3.Lerp(previousLocation, nextLocation, t);
                }

                time += Time.deltaTime;

                yield return null;
            }

            if (obj != null)
            {
                obj.transform.localPosition = nextLocation;
                Destroy(obj);
            }
        }

        isLerping = false;
    }
}
