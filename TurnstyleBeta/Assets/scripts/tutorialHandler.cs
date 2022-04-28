using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialHandler : MonoBehaviour
{
    
    public TutorialSegment[][] allTutorials;
    private GameObject Glossary;
    public bool isOpen;
    public int pageCount;
    public int bookCount;

    void Awake(){
        isOpen = false;
        bookCount =0;
        pageCount = 0;
        populateScript(3);
        Glossary = GameObject.Find("glossary");
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
            Glossary.GetComponent<glossaryScript>().setPage(2);
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
        //Instnatiate first page
        }
    }

    public void close(){
        if(isOpen){
        Transform transform = this.GetComponent<Transform>();
        Vector3 phonePos = transform.localPosition;
        phonePos = new Vector3 (phonePos.x, phonePos.y-490, phonePos.z);
        transform.localPosition = phonePos;
        isOpen = false;
        bookCount++;
        }
    }
    
    private void addTutorialText(TutorialSegment[] temp, int book, int pageNumber){
        allTutorials[book] = new TutorialSegment[pageNumber];
        for(int i = 0; i < pageNumber; i++){
            allTutorials[book][i] = temp[i];
        }
    }

    private void populateScript(int books){
        allTutorials = new TutorialSegment[books][];
        int bookFiller = 0;
        int pageFiller = 0;
        TutorialSegment[] temp = new TutorialSegment[50];

        //-------------------------------------------------------//
        //The following is the format for a tutorial section
        /*
         temp[pageFiller] = new TutorialSegment(
            new string[2]{"",""},      Array of all Displayed Text
            new string[1]{""},   Array of highlighted objects
            "X"                     Condition to move to next page
        );
        pageFiller++;
        */
        //
        //-------------------------------------------------------//

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"",""},
            new string[1]{""},
            "xDown"
        );
        pageFiller++;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;
    }

}
