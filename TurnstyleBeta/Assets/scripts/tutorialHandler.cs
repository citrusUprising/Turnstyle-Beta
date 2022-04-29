using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialHandler : MonoBehaviour
{
    
    public TutorialSegment[][] allTutorials;
    [SerializeField] private GameObject combatCon;
    [SerializeField] private GameObject sceneCanvas;
    [SerializeField] private string scene;
    private Image[] sceneObjects;
    public bool isOpen;
    public int pageCount;
    public int bookCount;
    private bool startup;

    void Awake(){
        isOpen = false;
        bookCount =0;
        pageCount = 0;
        startup = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        newCanvas();
        if(scene == "nodeMap")
        allTutorials = this.GetComponent<comTutorialScript>().allTutorials; //flag
        else if (scene == "combat")
        allTutorials = this.GetComponent<comTutorialScript>().allTutorials; 
    }

    // Update is called once per frame
    void Update()
    {
    }

    void newCanvas(){
        sceneObjects = sceneCanvas.GetComponentsInChildren<Image>();
    }

    public void nextPage(){

        pageCount++;
        if(pageCount > 0){
            //close current prefabs
        }
        if(pageCount >= allTutorials[bookCount].Length){
            pageCount = 0;
            this.close();
        }else{
            //open new prefabs
            if (allTutorials[bookCount][pageCount].trigger == "G"){
            combatCon.GetComponent<combatController>().setPage(2);
        }
        }
        //sets glossary to open up statuses when prompted, untested
    }

    public void open(int book){
        if(!isOpen&&bookCount == book){
        Transform transform = this.GetComponent<Transform>();
        Vector3 phonePos = transform.localPosition;
        phonePos = new Vector3 (phonePos.x, phonePos.y+490, phonePos.z);
        transform.localPosition = phonePos;
        isOpen = true;
        for(int i = 0; i < sceneObjects.Length; i++){
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
            sceneObjects[i].color *= new Color(0.5f,0.5f,0.5f);
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
        }
        this.GetComponent<Image>().color = new Color (1.0f,1.0f,1.0f);
        //Instnatiate first page
        }
    }

    public void close(){
        if(isOpen){
        sceneObjects = sceneCanvas.GetComponentsInChildren<Image>();
        Transform transform = this.GetComponent<Transform>();
        Vector3 phonePos = transform.localPosition;
        phonePos = new Vector3 (phonePos.x, phonePos.y-490, phonePos.z);
        transform.localPosition = phonePos;
        isOpen = false;
        for(int i = 0; i < sceneObjects.Length; i++){
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
            sceneObjects[i].color *= new Color(2.0f,2.0f,2.0f);
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
        }
        bookCount++;
        }
    }

    private void HighlightObject(){

    }
}
