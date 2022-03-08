using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Station currentStation;
    Vector3 moveToPosition; 
    public float speed = 2f;
    int currentLine = 0;
    public float height;
    public Vector3 scale;
    public float transitionTime = .5f;
    public Animator transitionAnimator;
    public GameObject Music;
    //determines which cutscene node is active
    private int currentCutScene =1;

    public GameObject moneyTxt;

    int money = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = currentStation.transform.position + new Vector3(0,0, height);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Actice Scene Count: " + SceneManager.sceneCount);
        if (SceneManager.sceneCount == 1) 
        {
            Music.SetActive(true);
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
                //open cutscene
                Debug.Log("Opening Cutscene #"+currentCutScene);
                currentCutScene ++;
            }

            if (currentStation.hasCombat)
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
        if(currentStation.transform.parent.gameObject != currentStation.destinations[s].transform.parent.gameObject){
            money--;
            Debug.Log("Changed lines");
            moneyTxt.GetComponent<TextMeshProUGUI>().text = "$" + money + "<size=54.4><sup><u>00";

        }
        currentStation = currentStation.destinations[s];
        currentLine = 0;
    }

    IEnumerator loadScene(string Scene)
    {
        transitionAnimator.SetTrigger("toBlack");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
    }
}
