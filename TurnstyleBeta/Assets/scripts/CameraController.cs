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
    Vector3 moveToPosition; 
    public float speed = 2f;
    int currentLine = 0;
    //lists possible lines the player could be on, for money purposes
    private bool[] onLine;
    public Station[] allStations;
    public float height;
    public Vector3 scale;
    public float transitionTime = .5f;
    public Animator transitionAnimator;
    public GameObject Music;
    //determines which cutscene node is active
    public int currentCutScene =0;

    public GameObject moneyTxt;

    int money = 8;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0, height);
        onLine = new bool[]{false,false,false,false,false,false};
        
        Color temp = currentStation.GetComponent<Image>().color;
        temp = new Color (0.5f,0.43f,0.56f);
        currentStation.GetComponent<Image>().color = temp;

        SceneManager.LoadScene("DialogueScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        //cancels out of game after final cutscene
        if(currentCutScene == 4){
                    Music.SetActive(false);
                    StartCoroutine(loadScene("mainMenuScene"));
                }
        
        //Debug.Log("Actice Scene Count: " + SceneManager.sceneCount);
        if (SceneManager.sceneCount == 1) 
        {
            if(money >=0)Music.SetActive(true);

            if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                currentLine++;
                if (currentLine == currentStation.destinations.Length)
                {
                    currentLine = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                currentLine = currentLine - 1;
                if (currentLine == -1)
                {
                    currentLine = currentStation.destinations.Length - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                currentStation.transform.localScale = new Vector3(1, 1, 1);
                moveToStation(currentLine);
            }
            currentStation.destinations[currentLine].transform.localScale = scale;
            moveToPosition = currentStation.transform.position + new Vector3(0, 0, height);
            transform.position = Vector3.Lerp(transform.position, moveToPosition, speed);

            if (currentStation.cutscene == currentCutScene){
                onLine = new bool[]{false,false,false,false,false,false};
                Debug.Log("Opening Cutscene #"+currentCutScene);
                //Music.SetActive(false);
                SceneManager.LoadScene("DialogueScene", LoadSceneMode.Additive);
            }

            if (currentStation.hasCombat&&currentCutScene!=currentStation.cutscene)
            {
                //StartCoroutine(loadScene(currentStation.combatSceneName));
                GameObject Stats = GameObject.Find("CurrentStats");
                CurrentStats currStats = Stats.GetComponent<CurrentStats>();
                currStats.CurrentEnemies = currentStation.Enemies;
                SceneManager.LoadScene("combatScene", LoadSceneMode.Additive);
                Music.SetActive(false);
                currentStation.endCombat();
            }
        }
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

        //if(currentStation.transform.parent.gameObject != currentStation.destinations[s].transform.parent.gameObject){//flag
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
            if (money >= 0)
            moneyTxt.GetComponent<TextMeshProUGUI>().text = "$" + money + "<size=54.4><sup><u>00";
            if(money == -1){
                Music.SetActive(false);
                foreach(Station h in this.allStations){
                    h.EnableHardMode(); //flag
                }
            }

        }
        currentStation = currentStation.destinations[s];
        currentLine = 0;

        Color temp2 = currentStation.GetComponent<Image>().color;
        temp2 = new Color (0.5f,0.43f,0.56f);
        currentStation.GetComponent<Image>().color = temp2;
    }

    IEnumerator loadScene(string Scene)
    {
        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
    }
}
