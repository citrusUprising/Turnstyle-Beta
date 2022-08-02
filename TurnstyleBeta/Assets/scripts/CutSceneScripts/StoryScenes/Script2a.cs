using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script2a : ScriptTemplate
{
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=new overallDialogue("","","ApartmentDark",
        new dialogueEntry[]{
            new dialogueEntry("It’s bright and early, the sun softly shining through the apartment curtains.",false,"null","null"),
            new dialogueEntry("It is calm and quiet, four college students sleeping soundly. A rare moment in an otherwise loud, often arguing household of five",false,"null","null"),
            new dialogueEntry("…it’d be a shame if someone were to disturb this peacef—",false,"null","null"),
            new dialogueEntry("*alarm clock ringing*",false,"stop","Cutscene/alarm"), //alarm
            }
        );
        dialogueVarietyCount++;
        
        temp[dialogueVarietyCount]=new overallDialogue("Amery","","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Wakey wakey everyone!! We’re already running late, let’s get a move on!",false,"Happy","Angry","Music/AmeryTheme","stop")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Jade","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Goddamnit Amery…what do you mean ‘we’re already running late’? Late for what?",true,"Happy","Angry","null","null"),
            new dialogueEntry("Why, I’ve landed yet another internship interview. And it’s at 8 a.m. sharp!",false,"Happy","Sad","null","null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Beverly","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Oh, that’s great, I’m so happy for you!",true,"Happy","Happy","null","null"),
            new dialogueEntry("But uhm…what are you waking up the rest of us for exactly…?",true,"Happy","Sad","null","null"),
            new dialogueEntry("Well, Beverly, you all have to help me fight the subway monsters, of course!",false,"Happy","Sad","null","null"),
            new dialogueEntry("Can’t have my outfit getting messed up before the big interview.",false,"Happy","Sad","null","null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Ugh…are you serious…? I’m too tired for this…just let me sleep…",true,"Happy","Sad","null","null"),
            new dialogueEntry("Oh Seraphim, of course you’re tired and don’t want to get up, you swear that anytime before 3 pm is morning. Quit being lazy and let's get ready to go.",false,"Happy","Sad","null","null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("","Seraphim","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Hey, wait! I’m not just gonna— ",true,"Happy","Angry","null","null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Seraphim","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Helloooo!! Is everyone else ready to head to the subway?",false,"Happy","Sad","null","null"),
            new dialogueEntry("Just when I thought this couldn't get any worse.",true,"Happy","Sad","null","null"),
            new dialogueEntry("We get it. You're already all done up! The princex of our house is perfect as usual.",true,"Happy","Sad","null","null"),
            new dialogueEntry("Well, of course! A dashing princex like myself has to be ready for anything teehee.",false,"Happy","Sad","null","null"),
            new dialogueEntry("Geez…your voice is more annoying than the alarm.",true,"Happy","Sad","null","null"),
            new dialogueEntry("Excuse me, what did you just say? Come here, you little—",false,"Angry","Sad","null","null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Jade","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Woah woah, wait a minute, Koralie…I’m sure Seraphim is just tired, they probably didn’t mean it. I think you should just let them get ready.",true,"Angry","Shy","null","null"),
            new dialogueEntry("Ugh, you’re always making excuses for them, Jade…but fine. I’ll let them be…for now.",false,"Angry","Shy","null","null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Beverly","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Okay! I’m all ready to go! You’re ready too, aren’t you, Koralie? Do you want to wait outside with me for everyone else?",true,"Angry","Happy","null","null"),
            new dialogueEntry("Ah uhm…no, I’ll just go outside with the others when they’re ready.",false,"Shy","Shy","null","null"),
            new dialogueEntry("Oh…well that’s cool! See you out there, then!",true,"Shy","Sad","null","Cutscene/door_close") //doorClosing
        }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("Alright, I’ve gathered all my papers! What’s the hold up?",false,"Happy","Sad","null","null"),
            new dialogueEntry("We’ll be ready in a second…but god, why do you have to spring this on us without any notice at all? We have lives too, don’t you get that?",true,"Happy","Angry","null","null")
            }
        );
        dialogueVarietyCount ++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Jade","ApartmentLight",
        new dialogueEntry[]{
            new dialogueEntry("They’re right, you know. Why can’t you get your actual friends to help you with this? Why us?",true,"Happy","Angry","null","null"),
            new dialogueEntry("Oh well…they’re very busy people, yeah, really busy…they simply didn’t have time to help me out today. The next best thing was you guys!",false,"Sad","Angry","null","null"),
            new dialogueEntry("Right…uh huh. Well, let’s head out, since you’re in such a hurry and all.",true,"Sad","Angry","null","null")
            }
        );
        dialogueVarietyCount ++;

        script= new overallDialogue[dialogueVarietyCount];
        for(int i =0; i<dialogueVarietyCount;i++){
            script[i] = temp[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}