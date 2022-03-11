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
    private GameObject start;
    private GameObject creditsText;
    private GameObject settings;
    private GameObject exit;
    private GameObject pointer;
    private int selectedOption = 0;
    private GameObject[] options = new GameObject[4];

    private float rotateDirection;

    public GameObject credits;
    private GameObject creditsObject;

    public GameObject pauseMenu;
    private GameObject pauseMenuObject;

    public Canvas canvas;

    // GameObjects that hold FMOD Studio Event Emitters for playing SFX
    public GameObject menuScroll;
    public GameObject selectSound;

    public 

    // Start is called before the first frame update
    void Start()
    {
   
        start = logo.transform.GetChild(1).gameObject;
        creditsText = logo.transform.GetChild(2).gameObject;
        settings = logo.transform.GetChild(3).gameObject;
        exit = logo.transform.GetChild(4).gameObject;
        pointer = logo.transform.GetChild(5).gameObject;

        options[0] = start;
        options[1] = creditsText;
        options[2] = settings;
        options[3] = exit;

        randomizeRotateDirection();

        gameObject.transform.Rotate(0, 0, Random.Range(0, 5) * 72);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, .25f*rotateDirection);

        if (creditsObject == null && pauseMenuObject == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Play menu scroll sfx
                menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                selectedOption--;
                if (selectedOption == -1)
                {
                    selectedOption = 2;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Play menu scroll sfx
                menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                selectedOption++;
                if (selectedOption == 3)
                {
                    selectedOption = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                // Play select sound sfx
                selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                if (selectedOption == 0)
                {
                    StartCoroutine(loadScene(2));
                }

                else if (selectedOption == 1)
                {
                    if (creditsObject == null)
                    {
                        creditsObject = Instantiate(credits, canvas.transform);
                        StartCoroutine(lerpCreditsOnScreen());
                    }
                }

                else if (selectedOption == 2)
                {
                    if (pauseMenuObject == null)
                    {
                        pauseMenuObject = Instantiate(pauseMenu, canvas.transform);
                    }
                }

                else if (selectedOption == 3)
                {
                    Application.Quit();
                }
            }

            pointer.transform.localPosition = new Vector3(
                pointer.transform.localPosition[0],
                options[selectedOption].transform.localPosition[1],
                pointer.transform.localPosition[2]);
        } 
        
        else if (creditsObject != null && (Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Z)) || (Input.GetKeyDown(KeyCode.X)))
        {
            StartCoroutine(lerpCreditsOffScreen());
        }

        else if (pauseMenuObject != null && Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(pauseMenuObject);
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

    IEnumerator lerpCreditsOnScreen()
    {
        if (creditsObject != null)
        {
            float time = 0f;
            float duration = 1f;

            Vector3 previousLocation = new Vector3(1024, 0, 0);
            Vector3 nextLocation = new Vector3(0, 0, 0);

            while (time < duration)
            {

                float t = time / duration;

                t = t * t * (3f - 2f * t);

                if (creditsObject != null)
                {
                    creditsObject.transform.localPosition = Vector3.Lerp(previousLocation, nextLocation, t);
                }
                time += Time.deltaTime;

                yield return null;
            }

            if (creditsObject != null)
            {
                creditsObject.transform.localPosition = nextLocation;
            }
        }
    }

    IEnumerator lerpCreditsOffScreen()
    {
        if (creditsObject != null)
        {
            float time = 0f;
            float duration = 1f;

            Vector3 previousLocation = new Vector3(0, 0, 0);
            Vector3 nextLocation = new Vector3(-1024, 0, 0);

            while (time < duration)
            {

                float t = time / duration;

                t = t * t * (3f - 2f * t);

                if (creditsObject != null)
                {
                    creditsObject.transform.localPosition = Vector3.Lerp(previousLocation, nextLocation, t);
                }

                time += Time.deltaTime;

                yield return null;
            }

            if (creditsObject != null)
            {
                creditsObject.transform.localPosition = nextLocation;
                Destroy(creditsObject);
            }
        }
    }
}
