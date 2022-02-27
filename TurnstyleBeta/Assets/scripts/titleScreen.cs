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
    private GameObject credits;
    private GameObject exit;
    private GameObject pointer;
    private int selectedOption = 0;
    private GameObject[] options = new GameObject[3];

    private float rotateDirection;

    // GameObjects that hold FMOD Studio Event Emitters for playing SFX
    public GameObject menuScroll;
    public GameObject selectSound;

    public 

    // Start is called before the first frame update
    void Start()
    {
   
        start = logo.transform.GetChild(1).gameObject;
        credits = logo.transform.GetChild(2).gameObject;
        exit = logo.transform.GetChild(3).gameObject;
        pointer = logo.transform.GetChild(4).gameObject;

        options[0] = start;
        options[1] = credits;
        options[2] = exit;

        randomizeRotateDirection();

        gameObject.transform.Rotate(0, 0, Random.Range(0, 5) * 72);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, .25f*rotateDirection);

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Random.Range(0f, 1f) >= .9)
            {
                randomizeRotateDirection();
            }
        }

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
                StartCoroutine(loadScene(1));
            }

            else if (selectedOption == 2)
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(loadScene(2));
        }

        pointer.transform.localPosition = new Vector3(
            pointer.transform.localPosition[0],
            options[selectedOption].transform.localPosition[1],
            pointer.transform.localPosition[2]);
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
}
