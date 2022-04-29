using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class comTutorialScript : MonoBehaviour
{

[SerializeField] private GameObject[] pentagon;
[SerializeField] private GameObject[] activeChars;
[SerializeField] private GameObject[] passiveChars;
[SerializeField] private GameObject[] activeNames;
[SerializeField] private GameObject[] passiveNames;
[SerializeField] private GameObject[] speedSelect;
[SerializeField] private GameObject[] totalSpeed;
[SerializeField] private GameObject[] statusBar;
    public enum highlightList
    {
        pentagon = 0,
        activeChar = 1,
        passiveChar = 2,
        activeNames = 3,
        passiveNames = 4,
        speedSelect = 5,
        totalSpeed = 6,
        statusBar = 7

    }

    public TutorialSegment[][] allTutorials;
    public GameObject[][] sceneItems = new GameObject[8][];

    

    void Awake(){
        sceneItems[0] = pentagon;
        sceneItems[1] = activeChars;
        sceneItems[2] = passiveChars;
        sceneItems[3] = activeNames;
        sceneItems[4] = passiveNames;
        sceneItems[5] = speedSelect;
        sceneItems[6] = totalSpeed;
        sceneItems[7] = statusBar;
        populateScript(3);
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
            new string[2]{"Testing","Hey we're testing over here"},
            new string[1]{""},
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"try Rotating?"},
            new string[1]{""},
            "ArrowKeys"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"Okay that worked","open the glossary"},
            new string[1]{""},
            "G"
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

        Debug.Log ("Tutorial: Array is full");
    }
}