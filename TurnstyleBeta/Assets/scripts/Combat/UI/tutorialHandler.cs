using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialHandler : MonoBehaviour
{
    
    public TutorialSegment[][] allTutorials;
    private GameObject[][] highlightObjects; 
    [SerializeField] private GameObject combatCon;
    [SerializeField] private GameObject[] sceneCanvases;
    [SerializeField] private string scene;
    [SerializeField] private GameObject[] background = new GameObject[0];
    
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
        allTutorials = this.GetComponent<mapTutorialScript>().allTutorials;
        else if (scene == "combat")
        allTutorials = this.GetComponent<comTutorialScript>().allTutorials; 
    }

    // Update is called once per frame
    void Update()
    {
    }

    void newCanvas(){
        int temp = 0;
        for (int i = 0; i <sceneCanvases.Length; i++){
            temp += sceneCanvases[i].GetComponentsInChildren<Image>().Length;
        }
        sceneObjects = new Image[temp];
        int counter = 0;
        for(int j = 0; j < sceneCanvases.Length; j++){
            for(int k = 0; k < sceneCanvases[j].GetComponentsInChildren<Image>().Length; k++){
                sceneObjects[k+counter] = sceneCanvases[j].GetComponentsInChildren<Image>()[k];
            }
            counter += sceneCanvases[j].GetComponentsInChildren<Image>().Length;
        }
    }

    public void nextPage(){

        breakPage(allTutorials[bookCount][pageCount]);
        pageCount++;
        if(pageCount >= allTutorials[bookCount].Length){
            pageCount = 0;
            this.close();
        }else{
            makePage(allTutorials[bookCount][pageCount]);
            if (allTutorials[bookCount][pageCount].trigger == "G"&&scene == "combat"){
            combatCon.GetComponent<combatController>().setPage(2);
        }
        }
        //sets glossary to open up statuses when prompted, untested
    }

    public void open(int book){
        if(!isOpen&&bookCount == book){
        newCanvas();
        backgroundToggle(false);
        Transform transform = this.GetComponent<Transform>();
        Vector3 phonePos = transform.localPosition;
        phonePos = new Vector3 (phonePos.x, phonePos.y+590, phonePos.z);
        transform.localPosition = phonePos;
        isOpen = true;
        for(int i = 0; i < sceneObjects.Length; i++){
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
            sceneObjects[i].color *= new Color(0.5f,0.5f,0.5f);
            //Debug.Log (sceneObjects[i].sprite+", Item "+i+" is now "+sceneObjects[i].color);
        }
        this.GetComponent<Image>().color = new Color (1.0f,1.0f,1.0f);
        makePage(allTutorials[book][0]);
        }
    }

    public void close(){
        if(isOpen){
        newCanvas();
        backgroundToggle(true);
        Transform transform = this.GetComponent<Transform>();
        Vector3 phonePos = transform.localPosition;
        phonePos = new Vector3 (phonePos.x, phonePos.y-590, phonePos.z);
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
    private void backgroundToggle(bool light){
        if(background.Length > 0){
            Color pigment;
            if(light) pigment = new Color (2.0f,2.0f,2.0f,1.0f);
            else pigment = new Color (0.5f,0.5f,0.5f,1.0f);
            for (int i = 0; i < background.Length; i++)
                background[i].GetComponent<SpriteRenderer>().color *= pigment;
        }    
    }

    private void makePage(TutorialSegment page){
        for(int i = 0; i < page.highlights.Length; i++)
        HighlightObject(page.highlights[i] ,true);
        this.GetComponent<textClusterHelpers>().createCluster(page.text);
    }

    private void breakPage(TutorialSegment page){
        for(int i = 0; i < page.highlights.Length; i++)
        HighlightObject(page.highlights[i] ,false);
        //text code;
    }

    private void HighlightObject(int cat, bool light){
        Color shade;
        if(light) shade = new Color (2.0f,2.0f,2.0f,1.0f);
        else shade = new Color (0.5f,0.5f,0.5f,1.0f);
        if(scene == "nodeMap"){
            highlightObjects = this.GetComponent<mapTutorialScript>().sceneItems; //flag
            for (int i = 0; i < highlightObjects[cat].Length;i++){
                highlightObjects[cat][i].GetComponent<Image>().color *= shade;
                Image[] temp;
                temp = highlightObjects[cat][i].GetComponentsInChildren<Image>();
                for (int j = 0; j < temp.Length; j++){
                    temp[j].color *= shade;
                }
            }
        }
        else if (scene == "combat"){
            highlightObjects = this.GetComponent<comTutorialScript>().sceneItems; 
            for (int i = 0; i < highlightObjects[cat].Length;i++){
                if(cat != 7){
                    highlightObjects[cat][i].GetComponent<Image>().color *= shade;
                    if(cat == 2){
                            Color tempCol = highlightObjects[cat][i].GetComponent<Image>().color;
                            if(light)tempCol.a = 1.0f;
                            else tempCol.a = 0.5f;
                            highlightObjects[cat][i].GetComponent<Image>().color = tempCol;
                    }
                }
                Image[] temp;
                temp = highlightObjects[cat][i].GetComponentsInChildren<Image>();
                for (int j = 0; j < temp.Length; j++){
                    temp[j].color *= shade;
                }
            }
        }
    }
}
