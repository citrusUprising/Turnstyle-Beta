using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1b : MonoBehaviour
{

    public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=(new overallDialogue("","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Having defeated the monsters and arrived at their destination, the team waits at the subway station for Amery to finish his interview.",false,"Happy","Happy","Cutscene/subway_ambience") //ambientSubway
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Jade","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Can’t believe Amery did that…sometimes I really think he doesn’t care about anyone other than himself.",false,"Angry","Happy","null"),
            new dialogueEntry("And waking all of us up unannounced isn’t even the end of it. If Amery's interview drags on any longer, I’m gonna be late for class!",false,"Angry","Happy","null"),
            new dialogueEntry("I’ll admit that I feel the same way. Amery is my friend and all, but I cannot bear to miss the game design club meeting today!",true,"Angry","Angry","null"),
            new dialogueEntry("We’re voting on club t-shirt designs today and their mascot is ever so cute…",true,"Angry","Shy","null"),
            new dialogueEntry("Oh! I’ve come up with an absolutely GENIUS solution!",true,"Angry","Happy","null"),
            new dialogueEntry(" Let’s simply tell Amery to pay for the premium subway without monsters! That way we can all get on with our days and head to campus!",true,"Angry","Happy","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Tsk. Really?",false,"Sad","Happy","null"),
            new dialogueEntry("Hm…? What’s wrong?",true,"Sad","Angry","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Jade","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("You gotta remember, Koralie, not everyone has the privilege of being able to afford that.",false,"Shy","Angry","null"),
            new dialogueEntry("Oh…right. Of course. Forgot about that.",true,"Shy","Shy","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Imagine having the luxury of being able to forget something like that…",false,"Sad","Shy","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("*footsteps*",true,"Sad","Happy","Cutscene/footsteps")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Amery","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Complaining as usual, are we, Seraphim?",true,"Sad","Happy","Music/AmeryTheme"),
            new dialogueEntry("I…uhm…",false,"Shy","Happy","null"),
            new dialogueEntry("Well, no matter, hello everyone! We meet again!",true,"Shy","Happy","null"),
            new dialogueEntry("The internship interview was a complete success! I have the utmost confidence that I’ll be accepted!",true,"Shy","Happy","null"),
            new dialogueEntry("You say that after all your interviews and those never worked out so…",false,"Sad","Happy","null"),
            new dialogueEntry("Oh? Care to speak up, Seraphim?",true,"Sad","Angry","null"),
            new dialogueEntry("I uh…I didn’t say anything.",false,"Shy","Angry","null"),
            new dialogueEntry("I heard you muttering something. What. Did. You. Say?",true,"Shy","Angry","null"),
            new dialogueEntry("Nothing, just let it go! We all gotta head to campus now, you’ve made us late enough already…",false,"Angry","Angry","null"),
            new dialogueEntry("Don’t just walk away from me! I won’t just let you get away wi—",true,"Angry","Angry","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Amery","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Ok you two, just stop! You’ve been at each other’s throats since we first woke up!",false,"Angry","Angry","stop"), // stop music
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("But I just—",true,"Angry","Shy","null"),
            new dialogueEntry("No buts! This is something we can all work through together at home. I’m tired of hearing you tear each other down like you’ve been doing all day!",false,"Angry","Shy","null"),
            new dialogueEntry("I get you, Bev…sorry.",true,"Angry","Shy","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Amery","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Ha, she showed you.",true,"Angry","Happy","null"),
            new dialogueEntry("That wasn’t meant for just them, Amery. You too. There’s no need to say all that mean stuff to them. They’re trying their best, you know.",false,"Angry","Happy","null"),
            new dialogueEntry("Alright, alright. …I understand.",true,"Angry","Sad","null"),
            new dialogueEntry("Good. Well then, let’s get going everyone!",false,"Happy","Sad","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Sounds good to me!",true,"Happy","Happy","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Jade","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Way ahead of you.",true,"Happy","Shy","null")
            }

        ));
        dialogueVarietyCount++;

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
