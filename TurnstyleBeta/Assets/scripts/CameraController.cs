using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                currentLine++;
                if (currentLine == currentStation.destinations.Length)
                {
                    currentLine = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentStation.destinations[currentLine].transform.localScale = new Vector3(1, 1, 1);
                currentLine = currentLine - 1;
                if (currentLine == -1)
                {
                    currentLine = currentStation.destinations.Length - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentStation.transform.localScale = new Vector3(1, 1, 1);
                moveToStation(currentLine);
            }
            currentStation.destinations[currentLine].transform.localScale = scale;
            moveToPosition = currentStation.transform.position + new Vector3(0, 0, height);
            transform.position = Vector3.Lerp(transform.position, moveToPosition, speed);

            if (currentStation.hasCombat)
            {
                //StartCoroutine(loadScene(currentStation.combatSceneName));
                GameObject Stats = GameObject.Find("CurrentStats");
                CurrentStats currStats = Stats.GetComponent<CurrentStats>();
                currStats.CurrentEnemies = currentStation.Enemies;
                SceneManager.LoadScene("combatScene", LoadSceneMode.Additive);
                currentStation.hasCombat = false;
                Music.SetActive(false);
            }
        }
    }

    void moveToStation(int s)
    {
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
