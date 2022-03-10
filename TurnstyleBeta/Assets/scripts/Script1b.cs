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
            new dialogueEntry("Having defeated the monsters and arrived at their destination, the team waits at the subway station for Amery to finish his interview.",false,"null") //ambientSubway
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Jade","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Can’t believe Amery did that…sometimes I really think he doesn’t care about anyone other than himself.",false,"null"),
            new dialogueEntry("And waking all of us up unannounced isn’t even the end of it. If Amery's interview drags on any longer, I’m gonna be late for class!",false,"null"),
            new dialogueEntry("I’ll admit that I feel the same way. Amery is my friend and all, but I cannot bear to miss the game design club meeting today!",true,"null"),
            new dialogueEntry("We’re voting on club t-shirt designs today and their mascot is ever so cute…",true,"null"),
            new dialogueEntry("Oh! I’ve come up with an absolutely GENIUS solution!",true,"null"),
            new dialogueEntry(" Let’s simply tell Amery to pay for the premium subway without monsters! That way we can all get on with our days and head to campus!",true,"null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Tsk. Really?",false,"null"),
            new dialogueEntry("Hm…? What’s wrong?",true,"null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Jade","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("You gotta remember, Koralie, not everyone has the privilege of being able to afford that.",false,"null"),
            new dialogueEntry("Oh…right. Of course. Forgot about that.",true,"null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Koralie","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Imagine having the luxury of being able to forget something like that…",false,"null")
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

/*


(*approaching footsteps sfx*)

Amery: Complaining as usual, are we, Seraphim?

Seraphim: I…uhm…

Amery: Well, no matter, hello everyone! We meet again!

Amery: The internship interview was a complete success! I have the utmost confidence that I’ll be accepted!

Seraphim: You say that after all your interviews and those never worked out so…

Amery: Oh? Care to speak up, Seraphim?

Seraphim: I uh…I didn’t say anything.

Amery: I heard you muttering something. What. Did. You. Say?

Seraphim: Nothing, just let it go! We all gotta head to campus now, you’ve made us late enough already…

Amery: Don’t just walk away from me! I won’t just let you get away wi—

Beverly: Ok you two, just stop! You’ve been at each other’s throats since we first woke up!

Seraphim: But I just—

Beverly: No buts! This is something we can all work through together at home. I’m tired of hearing you tear each other down like you’ve been doing all day!

Seraphim: I get you, Bev…sorry. 

Amery: Ha, she showed you.

Beverly: That wasn’t meant for just them, Amery. You too. There’s no need to say all that mean stuff to them. They’re trying their best, you know.

Amery: Alright, alright. …I understand.

Beverly: Good. Well then, let’s get going everyone!

Koralie: Sounds good to me!

Jade: Way ahead of you.
*/