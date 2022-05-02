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
            new string[2]{"",""},       // Array of all Displayed Text
            new int[1]{""},             //Array of highlighted objects
            "X"                         // Condition to move to next page
        );
        pageFiller++;
        */
        //
        //-------------------------------------------------------//

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"Each <b>Party Member</b> has their own spot on the <b>Pentagon.</b> Each turn, you can <b>Rotate</b> it however you like."},
            new int[1]{0},
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"Each <b>Party Member</b> has a <b>Name Tag</b> that shows you their <b>HP,</b> <b>Fatigue, Statuses,</b> and <b>Passive.</b>"},
            new int[2]{3, 4},
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"The three <b>Party Members</b> on the <b>right</b> side of the <b>Pentagon</b> are <b>in combat.</b> "
                        ,"They get <b>1 Fatigue</b> each turn."},      
            new int[4]{1, 2, 3, 4},   
            "xDown"                     
        );
        pageFiller++;
        
        temp[pageFiller] = new TutorialSegment(
            new string[2]{"The two <b>Party Members</b> on the <b>left</b> side of the <b>Pen/tagon<b> are <b>out of combat.</b>"
                        ,"They <b>aren’t affected</b> by any <b>moves</b> and <b>lose 2 Fatigue</b> each turn. "},      
            new int[2]{2, 4},   
            "xDown"                     
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"At the beginning of <b>each Turn,</b> you can <b>rotate</b> the <b>Pentagon</b> by pressing the <b>arrow keys.</b> Try it!"},
            new int[1]{0},
            "ArrowKeys"                         
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"You can view each <b>Party Member’s Moves</b> and <b>Passive</b> in the <b>Pause Menu</b> by pressing <b>Escape.</b>"}, 
            new int[0]{}, 
            "xDown"
        );
        pageFiller++;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"Each <b>Turn,</b> you assign each <b>Party Member</b> a <b>Speed</b> value."
                        , "Characters with <b>high Speed act first.</b>"}, 
            new int[1]{5}, 
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[2]{"You get <b>12 Speed</b> each turn to <b>distribute</b> between your three <b>active Party Members.</b>"
                        , "This is <b>refilled</b> at the beginning of <b>each turn.</b>"}, 
            new int[2]{5, 6}, 
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"A <b>Party Member’s Speed</b> is <b>lowered</b> by one for each point of <b>Fatigue</b> they have."}, 
            new int[3]{3, 4, 5}, 
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"If a <b>Party Member’s Speed</b> is lower than <b>0,</b> they take <b>20%</b> of their <b>HP</b> in damage."}, 
            new int[1]{5}, 
            "xDown"
        );
        pageFiller++;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"This is where you can see someone’s <b>Status Effects.</b>"}, 
            new int[1]{7}, 
            "xDown"
        );
        pageFiller++;

        temp[pageFiller] = new TutorialSegment(
            new string[1]{"These are all the <b>Status Effects.</b> You can see this in the <b>Glossary</b> by pressing <b>G.</b>"}, 
            new int[0]{}, 
            "G"
        );
        pageFiller++;

        //ends Current tutorials section and writes to allTutorials
        addTutorialText(temp,bookFiller,pageFiller);
        bookFiller++;
        pageFiller = 0;

        Debug.Log ("Tutorial: Array is full");
    }
}