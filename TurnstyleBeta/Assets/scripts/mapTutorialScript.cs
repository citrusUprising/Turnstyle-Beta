using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mapTutorialScript : MonoBehaviour
{

[SerializeField] private GameObject[] money;
[SerializeField] private GameObject[] lines;

    public TutorialSegment[][] allTutorials;
    public GameObject[][] sceneItems = new GameObject[2][];

    void Awake(){
        sceneItems[0] = money;
        sceneItems[1] = lines;
        populateScript(1);
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
            new string[2]{"",""},       // Array of all Displayed Text
            new int[1]{""},             //Array of highlighted objects
            "X"                         // Condition to move to next page
        );
        pageFiller++;
        */
        //
        //-------------------------------------------------------//

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"The <b>Subway</b> costs <b>$1</b> every time you <b>transfer lines.</b>"},
            new int[1]{0},
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"If you <b>run out of Money,</b> the Party will have to <b>walk on the tracks</b> to get to their destination.",
                        "You may run into <b>dangerous monsters</b> more often when <b>walking on the tracks.</b>"},
            new int[1]{1},
            "xDown"
        );
        pageFiller++;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        Debug.Log ("Tutorial: Array is full");
    }
}