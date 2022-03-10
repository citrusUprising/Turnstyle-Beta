using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1a : MonoBehaviour
{

    public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=new overallDialogue("","","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("It’s bright and early, the sun softly shining through the apartment curtains.",false,"null"),
            new dialogueEntry("It is calm and quiet, four college students sleeping soundly. A rare moment in an otherwise loud, often arguing household of five",false,"null"),
            new dialogueEntry("…it’d be a shame if someone were to disturb this peacef—",false,"null"),
            new dialogueEntry("*alarm clock ringing*",false,"null"), //alarm
            }
        );
        dialogueVarietyCount++;
        
        temp[dialogueVarietyCount]=new overallDialogue("Amery","","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Wakey wakey everyone!! We’re already running late, let’s get a move on!",false,"null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Jade","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Goddamnit Amery…what do you mean ‘we’re already running late’? Late for what?",true,"null"),
            new dialogueEntry("Why, I’ve landed yet another internship interview. And it’s at 8 a.m. sharp!",false,"null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Beverly","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Oh, that’s great, I’m so happy for you! But uhm…what are you waking up the rest of us for exactly…?",true,"null"),
            new dialogueEntry("Well, Beverly, you all have to help me fight the subway monsters, of course!",false,"null"),
            new dialogueEntry("Can’t have my outfit getting messed up before the big interview.",false,"null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Ugh…are you serious…? I’m too tired for this…just let me sleep…",true,"null"),
            new dialogueEntry("Oh Seraphim, of course you’re tired and don’t want to get up, you swear that anytime before 3 pm is morning.",false,"null"),
            new dialogueEntry("Gosh, I just don’t understand why you’re all being so unreasonable about this! Come on, let’s get ready to go, I’ll go pack my briefcase.",false,"null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("","Seraphim","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Hey, wait! I’m not just gonna— and he’s already gone. Great, great…Well it’s not like this morning can get any worse, right?",true,"null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Seraphim","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Helloooo!! Is everyone else ready to head to the subway?",false,"null"),
            new dialogueEntry("Guess I spoke too soon…",true,"null"),
            new dialogueEntry("Well, wouldn’t cha know? Look who’s ready to go! The princex of our house is perfect as usual.",true,"null"),
            new dialogueEntry("Well, of course! A dashing princex like myself has to be ready for anything teehee.",false,"null"),
            new dialogueEntry("I think you could learn a bit from me actually…would you like princex lessons from THE Koralie themselves?",false,"null"),
            new dialogueEntry("Nah…no thanks.",true,"null"),
            new dialogueEntry("Geez…your voice is more annoying than the alarm.",true,"null"),
            new dialogueEntry("Excuse me, what did you just say? Come here, you little—",false,"null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Jade","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Woah woah, wait a minute, Koralie…I’m sure Seraphim is just tired, they probably didn’t mean it. I think you should just let them get ready.",true,"null"),
            new dialogueEntry("Ugh, you’re always making excuses for them, Jade…but fine. I’ll let them be…for now.",false,"null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Beverly","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Okay! I’m all ready to go! You’re ready too, aren’t you, Koralie? Do you want to wait outside with me for everyone else?",true,"null"),
            new dialogueEntry("Ah uhm…no, I’m alright. I’ll go outside with the others when they’re ready.",false,"null"),
            new dialogueEntry("Oh…well that’s cool! See you out there, then!",true,"null") //doorClosing
        }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("Alright, I’ve gathered all my papers! I see that Bev and Koralie are ready, what’s the hold up for the rest of you?",false,"null"),
            new dialogueEntry("We’ll be ready in a second…but god, why do you have to spring this on us without any notice at all? We have lives too, don’t you get that?",true,"null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Jade","Apartment",
        new dialogueEntry[]{
            new dialogueEntry("They’re right, you know. Why can’t you get your actual friends to help you with this? Why us?",true,"null"),
            new dialogueEntry("Oh well…they’re very busy people, yeah, really busy…they simply didn’t have time to help me out today. The next best thing was you guys!",false,"null"),
            new dialogueEntry("Right…uh huh. Well, let’s head out, since you’re in such a hurry and all.",true,"null"),
            new dialogueEntry("Now you’re talking! Everyone, let’s go!",false,"null")
            }
        );
        dialogueVarietyCount ++;

        script= new overallDialogue[dialogueVarietyCount];
        for(int i =0; i<dialogueVarietyCount;i++){
            temp[i] = script[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}