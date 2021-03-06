using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Station currentStation;
    public GameObject[] lines;
    Vector3 moveToPosition; 
    public float speed = 2f;
    public float pulseSpeed;
    int currentLine = 0;
    //lists possible lines the player could be on, for money purposes
    public bool[] onLine;
    public Station[] allStations;
    public float height;
    public Vector3 scale;

    public float transitionTime = .5f;
    public GameObject transitionObject;
    public Animator transitionAnimator;

    public GameObject Music;
    //determines which cutscene node is active
    public int currentCutScene =0;
    public bool hasMoney;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
    public int currentDay = 0;

    //autozoom variables
    [SerializeField] private float xOuter;
    [SerializeField] private float xInner;
    [SerializeField] private float yOuter;
    [SerializeField] private float yInner;
    private int isZooming = 0;
    private bool destCamera = false;

    [SerializeField] private float tiltRatio;   //The ratio of current station to destination in camera pos, 
                                                //the higher it is, the closer to current station
    public GameObject moneyTxt;
    public GameObject objective;
    private bool xDown;
    private bool combatEnabled = true;

    public GameObject pauseMenu;
    private GameObject pauseMenuObject;
    public Canvas canvas;
    public GameObject tutorialPhone;
    public GameObject Pointer;
    public GameObject background;

    private bool loading = false;
    private bool pulseUp = true;
    public int money;

    public GameObject keyPrompt;

    public GameObject blackBox;

    public bool isTransitioningFromAnotherScene = false;

    // this is used seperate from the variable loading so that i can add a time in the transition before loading starts
    // if this is true then the game does not check for inputs
    private bool preloading = false;

    void Awake(){
        if(PlayerPrefs.GetInt("Load", 0) == 1){
            GameObject.Find("CurrentStats").GetComponent<savingEngine>().retry();
            PlayerPrefs.SetInt("Load", 0);
        }else{
            GameObject.Find("CurrentStats").GetComponent<savingEngine>().reset();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0, height);
        //onLine = new bool[]{false,false,false,false,false,false};
        
        Color temp = currentStation.GetComponent<Image>().color;
        temp = new Color (0.5f,0.43f,0.56f);
        currentStation.GetComponent<Image>().color = temp;

        blackBox.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        if (currentCutScene == 0)
        {
            Music.SetActive(false);
            SceneManager.LoadScene("DialogueScene", LoadSceneMode.Additive);
        }
        else{
            Pointer.GetComponent<rotatePointer>().NewDestination(currentCutScene-1);
        }
        blackBox.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        if (money>0)hasMoney = true;
        this.MoneyUpdate();
        GameObject Stats = GameObject.Find("CurrentStats");
        Stats.GetComponent<CurrentStats>().hasMoney = hasMoney;
        checkPresent(currentDay,currentCutScene);
    }

    // Update is called once per frame
    void Update()
    {  

        xDown = false;
        //DISABLE FOR FINAL GAME, skip combat for debugging
        //if (Input.GetKeyDown(KeyCode.S)) combatEnabled = !combatEnabled;
        //cancels out of game after final cutscene
        //Line Pulsing animation
        //Make into own Function - Josh
        for(int i = 0; i < 6; i++){
            LineRenderer temp = lines[i].GetComponent<LineRenderer>();
            if(onLine[i]){
                if(temp.startWidth < 1.75f && pulseUp){
                    temp.startWidth += pulseSpeed;
                    temp.endWidth += pulseSpeed;
                    if(temp.startWidth >= 1f){
                        pulseUp = false;
                    }
                } else {
                    temp.startWidth -= pulseSpeed;
                    temp.endWidth -= pulseSpeed;
                    if(temp.startWidth <= 0.3f){
                        pulseUp = true;
                    }
                }
            } else {
                temp.startWidth = 0.3f;
                temp.endWidth = 0.3f;
            }
        }
        
        //Debug.Log("Actice Scene Count: " + SceneManager.sceneCount);
        //if you're on the map and not loading 
        if (SceneManager.sceneCount == 1 && !loading) 
        {
            //Navigate tutorial
            if(tutorialPhone.GetComponent<tutorialHandler>().isOpen){
            int bookTemp = tutorialPhone.GetComponent<tutorialHandler>().bookCount;
            int pageTemp = tutorialPhone.GetComponent<tutorialHandler>().pageCount;
                //Changes accepted input to continue
                switch (tutorialPhone.GetComponent<tutorialHandler>().allTutorials[bookTemp][pageTemp].trigger){
                    case "xDown":
                    if(xPress()){
                        tutorialPhone.GetComponent<tutorialHandler>().nextPage();
                    }
                    break;

                    case "ArrowKeys":
                    xDown = false;
                    if(Input.GetKeyDown(KeyCode.UpArrow)||
                        Input.GetKeyDown(KeyCode.DownArrow)||
                        Input.GetKeyDown(KeyCode.LeftArrow)||
                        Input.GetKeyDown(KeyCode.RightArrow))
                            tutorialPhone.GetComponent<tutorialHandler>().nextPage();
                    break;

                    case "G":
                    xDown = false;
                    if(Input.GetKeyDown(KeyCode.G))
                        tutorialPhone.GetComponent<tutorialHandler>().nextPage();
                    break;

                    default:
                    break;
                }
            }

            // if(money >=0)Music.SetActive(true);

            if (pauseMenuObject == null)
            {
                if (preloading == false)
                {
                    //Changing Lines
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                        currentLine++;
                        if (currentLine == currentStation.destinations.Length)
                        {
                            currentLine = 0;
                        }
                        autoZoom();
                        destCamera = true;
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                        currentLine = currentLine - 1;
                        if (currentLine == -1)
                        {
                            currentLine = currentStation.destinations.Length - 1;
                        }
                        autoZoom();
                        destCamera = true;
                    }
                    //

                    //move to next station
                    if (xPress())
                    {
                        currentStation.transform.localScale = new Vector3(1, 1, 1);
                        moveToStation(currentLine);
                        autoZoom();
                        destCamera = false;
                    }
                    //Create Pause Menu
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        pauseMenuObject = Instantiate(pauseMenu, canvas.transform);
                    }
                }
                keyPrompt.SetActive(true);
            }
            else
            {
                keyPrompt.SetActive(false);
            }

            
            currentStation.destinations[currentLine].transform.localScale = scale;
            moveCamera();

            if (currentStation.cutscene == currentCutScene&&SceneManager.sceneCount == 1&&!preloading){
                onLine = new bool[]{false,false,false,false,false,false};
                //Music.SetActive(false);
                switch(currentCutScene){ //move to seperate routine, add final cutscene and day checks
                    case 1:
                    objective.GetComponent<TextMeshProUGUI>().text = "Everyone needs to get to class. University is to the South.";
                    break;

                    case 2:
                    objective.GetComponent<TextMeshProUGUI>().text = "It's been a long day. Head back home.";
                    break;

                    default:
                    objective.GetComponent<TextMeshProUGUI>().text = "";
                    break;
                }
                if(currentCutScene < 3)StartCoroutine(pointerDelay());
                StartCoroutine(addMoney());
                GameObject Stats = GameObject.Find("CurrentStats");
                Stats.GetComponent<CurrentStats>().BeverlyHealth = 16;
                Stats.GetComponent<CurrentStats>().JadeHealth = 15;
                Stats.GetComponent<CurrentStats>().KoralieHealth = 20;
                Stats.GetComponent<CurrentStats>().SeraphimHealth = 10;
                Stats.GetComponent<CurrentStats>().AmeryHealth = 12;

                StartCoroutine(loadScene("DialogueScene"));
                checkPresent(currentDay,currentCutScene);
                

            }

            if (currentStation.hasCombat&&currentCutScene!=currentStation.cutscene&&combatEnabled)
            {
                //StartCoroutine(loadScene(currentStation.combatSceneName));
                GameObject Stats = GameObject.Find("CurrentStats");
                CurrentStats currStats = Stats.GetComponent<CurrentStats>();
                currStats.CurrentEnemies = currentStation.Enemies;
                if(currStats.currentTutorial == currentStation.isTutorial){
                    currStats.isTutorial = currentStation.isTutorial;
                    currStats.currentTutorial++;
                } else currStats.isTutorial = 0;

                StartCoroutine(loadScene("tutorialScene"));
                Music.SetActive(false);
                currentStation.endCombat();
                StartCoroutine(firstTutorial());
                //tutorialPhone.GetComponent<tutorialHandler>().open(0);
            }


            // checks if the scene just changed from another scene, and if it did applies a transition
            // this variable is set to true in the combat scene and dialogue scene
            if (isTransitioningFromAnotherScene)
            {
                // it's set to false here so that this only happens once
                isTransitioningFromAnotherScene = false;

                Music.SetActive(true);

                transitionAnimator.SetTrigger("fromBlack");
            }
        }
        
        //Zooming
        if((Input.GetKey(KeyCode.C)||isZooming > 0) && SceneManager.sceneCount == 1 && !loading){
            //Debug.Log("Pressed C");
            zoomIn();
            autoZoom();
        } 
        if((Input.GetKey(KeyCode.D)||isZooming < 0)&& SceneManager.sceneCount == 1 && !loading ){
            //Debug.Log("Pressed D");
            zoomOut();
            autoZoom();
        } 
    }

    public void nextDay(){
        Music.SetActive(false);
        //SceneManager.LoadScene("mainMenuScene",LoadSceneMode.Single); // To Remove
        currentDay++;
        if (currentDay>=5)
            SceneManager.LoadScene("mainMenuScene",LoadSceneMode.Single); //replace with credits??
        else{  //do we want a save&Quit or Continue screen?
            this.currentCutScene = 0;
            this.currentStation = this.allStations[16];
            Music.SetActive(false);
            StartCoroutine(loadScene("DialogueScene"));
            changeMap(currentDay);
            //add money (could be in change map)
        }
    }

    void changeMap (int day){;
        for (int i =0; i < this.allStations.Length; i++){
            stationDataHolder temp = this.GetComponent<DailyMapsHolder>().masterlist[day-1][i];
            allStations[i].Enemies = temp.monsters;
            allStations[i].hasCombat = temp.combat;
            allStations[i].hasHardMode = temp.hardMode;
            allStations[i].cutscene = temp.cutscene;
            allStations[i].initStation();
        }
    }

    void checkPresent(int day = 0, int curCutscene = 0){
            GameObject Stats = GameObject.Find("CurrentStats");
        /*switch(day){
            case 0: default:*/
            defaultPresent(Stats);
        /*  break;

            case 1:
            break;
        }*/
    }

    void defaultPresent(GameObject Stats){
        Stats.GetComponent<CurrentStats>().BeverlyGone = false;
        Stats.GetComponent<CurrentStats>().JadeGone = false;
        Stats.GetComponent<CurrentStats>().KoralieGone = false;
        Stats.GetComponent<CurrentStats>().SeraphimGone = false;
        Stats.GetComponent<CurrentStats>().AmeryGone = false;
    }

    void moveCamera(){
        if(!destCamera){
            moveToPosition = currentStation.transform.position + new Vector3(0, 0, height);
        }else{
            moveToPosition = (tiltRatio*currentStation.transform.position+currentStation.destinations[currentLine].transform.position)/(tiltRatio+1)
             + new Vector3(0, 0, height);
        }
        transform.position = Vector3.Lerp(transform.position, moveToPosition, speed/1.5f);
    }

    bool xPress(){
        if(Input.GetKeyDown(KeyCode.Z)&&!xDown){
        xDown = true;
        return true;
        }
        else
        return false;
    }

    void moveToStation(int s)
    {
        //change color back to default
        Color temp1 = currentStation.GetComponent<Image>().color;
        temp1 = new Color (1f,1f,1f);
        currentStation.GetComponent<Image>().color = temp1;

        bool stayLine = false;
        for(int l =0;l<6;l++){
            if(this.onLine[l]&&this.onLine[l]==currentStation.destinations[s].lines[l]){
                stayLine = true;
                l=6;
            }
        }

        //if(currentStation.transform.parent.gameObject != currentStation.destinations[s].transform.parent.gameObject){
        if(stayLine){
            for(int l =0;l<6;l++){
                if(this.onLine[l]&&!currentStation.destinations[s].lines[l]){
                    this.onLine[l] = false;
                }
            }
        }
        else{
            for(int f =0;f<6;f++){
                if(currentStation.lines[f]&&currentStation.destinations[s].lines[f]){
                    this.onLine[f] = true;
                }else{
                    this.onLine[f] = false;
                }
            }
            money--;
            Debug.Log("Changed lines");
            this.MoneyUpdate();
            if (money > 1 && tutorialPhone.GetComponent<tutorialHandler>().bookCount >= 2)
            tutorialPhone.GetComponent<tutorialHandler>().bookCount = 1;
            else if (money > 0 && tutorialPhone.GetComponent<tutorialHandler>().bookCount >= 3)
            tutorialPhone.GetComponent<tutorialHandler>().bookCount = 2;
            if(money == 1)
            tutorialPhone.GetComponent<tutorialHandler>().open(1);
        }
        currentStation = currentStation.destinations[s];
        currentLine = 0;

        Color temp2 = currentStation.GetComponent<Image>().color;
        temp2 = new Color (0.5f,0.43f,0.56f);
        currentStation.GetComponent<Image>().color = temp2;
    }

    void zoomIn(){
        Camera cam = this.GetComponent<Camera>();
        if(cam.orthographicSize >= minZoom){
            cam.orthographicSize -= zoomSpeed;
        }
    }

    void zoomOut(){
        Camera cam = this.GetComponent<Camera>();
        if(cam.orthographicSize < maxZoom){
            cam.orthographicSize += zoomSpeed;
        }
    }

    void autoZoom(){
         float scale = this.GetComponent<Camera>().orthographicSize / 19f;
         Vector2 checkposition = new Vector2 (
            this.GetComponent<Transform>().position.x,
            this.GetComponent<Transform>().position.y);
        Transform stationCheck = currentStation.destinations[currentLine].transform;
        Vector2 destination = new Vector2 (
            stationCheck.position.x,
            stationCheck.position.y
        );
        if(destination.x-xOuter*scale > checkposition.x||
            destination.x+xOuter*scale < checkposition.x||
            destination.y-yOuter*scale > checkposition.y||
            destination.y+yOuter*scale < checkposition.y
        ){isZooming = -1;}
        else if(destination.x > checkposition.x-xInner*scale&&
            destination.x < checkposition.x+xInner*scale&&
            destination.y > checkposition.y-yInner*scale&&
            destination.y < checkposition.y+yInner*scale
        ){isZooming = 1;}
        else isZooming = 0;
    }

    void MoneyUpdate(){
        GameObject Stats = GameObject.Find("CurrentStats");
        if (money >= 0)
        moneyTxt.GetComponent<TextMeshProUGUI>().text = "$" + money + "<size=54.4><sup><u>00";
        if(money == -1 && hasMoney){
            Music.SetActive(false);
            hasMoney = false;
            Stats.GetComponent<CurrentStats>().hasMoney = false;
            tutorialPhone.GetComponent<tutorialHandler>().open(2);
            foreach(Station h in this.allStations){
                h.EnableHardMode();
            }
            Color pigment;//flag
            pigment = new Color (0.5f,0.5f,0.5f,1.0f);
            background.GetComponent<SpriteRenderer>().color *= pigment;
        }
        else if (money > 0 && !hasMoney){
            if(SceneManager.sceneCount == 1)Music.SetActive(true);
            hasMoney = true;
            Stats.GetComponent<CurrentStats>().hasMoney = true;
            foreach(Station h in this.allStations){
                h.DisableHardMode();
            }
            Color pigment;
            pigment = new Color (2.0f,2.0f,2.0f,1.0f);
            background.GetComponent<SpriteRenderer>().color *= pigment;//flag
        }
        if(money < 0) money = 0;
    }

    IEnumerator addMoney(){
        yield return new WaitForSeconds (1f+transitionTime);
        money++;
        this.MoneyUpdate();
    }
    
    IEnumerator firstTutorial(){
        yield return new WaitForSeconds(5.0f);
        tutorialPhone.GetComponent<Canvas>().sortingOrder = 0;
        tutorialPhone.GetComponent<tutorialHandler>().open(0);
    }

    IEnumerator pointerDelay(){
        yield return new WaitForSeconds (1f +transitionTime);
        Pointer.GetComponent<rotatePointer>().NewDestination(currentCutScene);
    }

    IEnumerator loadScene(string Scene)
    {
        preloading = true;
        // we add a delay so that the camera settles on the newly selected node
        yield return new WaitForSeconds(.75f);

        Music.SetActive(false);

        loading = true;
        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
        loading = false;
        preloading = false;
    }
}
