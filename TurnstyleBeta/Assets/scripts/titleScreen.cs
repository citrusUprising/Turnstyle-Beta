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
    public GameObject resume;
    public GameObject newGame;
    

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

    private bool controlsLerpingOn = false;
    private bool controlsLerpingOff = false;

    // Start is called before the first frame update
    void Start()
    {
        randomizeRotateDirection();

        gameObject.transform.Rotate(0, 0, Random.Range(0, 5) * 72);

        PlayerPrefs.SetInt("Load", 0);

        initMusicVolume();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, .25f*rotateDirection);

        if (creditsObject == null && pauseMenuObject == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                changeSelectedOption(-1);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                changeSelectedOption(1);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // Play select sound sfx
                selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();

                if (options[selectedOption] == newGame)
                {
                    PlayerPrefs.SetInt("Load", 0);
                    StartCoroutine(loadScene(1));
                }

                else if (options[selectedOption] == resume){
                    PlayerPrefs.SetInt("Load", 1);
                    StartCoroutine(loadScene(1));
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
                options[selectedOption].transform.localPosition[1] - 200,
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
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
        if (Random.Range(0f, 1f) >= .9)
            {
                randomizeRotateDirection();
            }
        }

        
    }

    void changeSelectedOption(int direction)
    {
        // Play menu scroll sfx
        menuScroll.GetComponent<FMODUnity.StudioEventEmitter>().Play();

        selectedOption += direction;
        if (selectedOption == -1)
        {
            selectedOption = options.Length - 1;
        }
        else if (selectedOption == options.Length)
        {
            selectedOption = 0;
        }

        if (options[selectedOption] == controlsText && controlsObject == null)
        {
            StartCoroutine(lerpControls("on"));
        }
        else if (options[selectedOption] != controlsText && controlsObject != null)
        {
            StartCoroutine(lerpControls("off"));
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

    IEnumerator lerpControls(string direction)
    {
        float time = 0;
        float duration = .25f;

        Vector3 prevLocation = new Vector3(0, 0, 0);
        Vector3 nextLocation = new Vector3(0, 0, 0);

        if (direction == "on")
        {
            controlsObject = Instantiate(controls, canvas.transform);
            prevLocation = new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0);
            nextLocation = new Vector3(Screen.width/2, Screen.height/2, 0);

            if (controlsLerpingOn == true)
            {
                yield break;
            }

            controlsLerpingOn = true;
        }
        else if (direction == "off")
        {
            prevLocation = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            nextLocation = new Vector3(Screen.width * 3 / 4, Screen.height / 2, 0);

            if (controlsLerpingOff == true)
            {
                yield break;
            }

            controlsLerpingOff = true;
        }

        if (direction == "off")
        {
            while (controlsLerpingOn)
            {
                yield return null;
            }
        }
        else if (direction == "on")
        {
            while (controlsLerpingOff)
            {
                yield return null;
            }
        }

        while (time < duration)
        {
            float t = time / duration;

            t = t * t * (3f - 2f * t);

            controlsObject.transform.position = Vector3.Lerp(prevLocation, nextLocation, t);

            time += Time.deltaTime;

            yield return null;
        }

        if (direction == "off")
        {
            Destroy(controlsObject);
            controlsLerpingOff = false;
        }
        else if (direction == "on")
        {
            controlsLerpingOn = false;
        }

    }

    IEnumerator lerpObjectOnScreen(GameObject obj)
    {
        isLerping = true;

        if (obj != null)
        {
            float time = 0f;
            float duration = 1f;

            Vector3 previousLocation = new Vector3(Screen.width, 0, 0);
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
            Vector3 nextLocation = new Vector3(-Screen.width, 0, 0);

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

    // set initial amplitude on game launch in case default value isn't .5
    void initMusicVolume()
    {
        FMOD.Studio.VCA musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        FMOD.Studio.VCA sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        
        float musicPercent = PlayerPrefs.GetFloat("musicPercent", 0.5f);
        float musicVolume = getMusic(musicPercent, 1, 4);
        musicVCA.setVolume(musicVolume);
        float sfxPercent = PlayerPrefs.GetFloat("sfxPercent", 0.5f);
        float sfxVolume = getSFX(sfxPercent, 1, 4);
        sfxVCA.setVolume(sfxVolume);
    }

    // Range 0 to maxAmplitude, with the halfway point being halfAmplitude
    float getMusic(float musicPercent, float halfAmplitude, float maxAmplitude)
    {
        if (musicPercent <= .5) return (musicPercent * 2) * halfAmplitude;
        else return ((musicPercent - 0.5f) * 2) * (maxAmplitude - 1) + halfAmplitude;
    }

    // Range 0 to maxAmplitude, with the halfway point being halfAmplitude
    float getSFX(float sfxPercent, float halfAmplitude, float maxAmplitude)
    {
        if (sfxPercent <= .5) return (sfxPercent * 2) * halfAmplitude;
        else return ((sfxPercent - 0.5f) * 2) * (maxAmplitude - 1) + halfAmplitude;
    }
}
