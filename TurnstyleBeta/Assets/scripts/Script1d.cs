using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Script1d : MonoBehaviour
{

     public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=new overallDialogue("","","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("",false,"null","null")
            }

        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("Let's see, I need to add the egg to the batter now...",false,"Happy","Sad", "null", "null"),
            new dialogueEntry("Hrm, some of the egg shell got in there…",false,"Sad", "Sad", "null","null"),
            new dialogueEntry("Well, no matter, I’ll crush it up while I mix it in and no one will notice!",false,"Happy", "Sad", "null", "null"),
            new dialogueEntry("Alright, looks ready to me!",false,"Happy", "Sad", "null","null"),
            new dialogueEntry("Hm, this is so inefficient. I’ll turn the heat u-",false,"Angry", "Sad", "null", "null")
            }

        );
        dialogueVarietyCount++;
        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("Oh hey! Didn’t know you were up already.", true,"Angry", "Happy", "null","null"),
            new dialogueEntry("Are you…cooking?", true,"Angry", "Shy", "null","null"),
            new dialogueEntry("Yes, I am!", false,"Happy", "Shy", "null","null"),
            new dialogueEntry("I didn’t mean to wake you though, my apologies.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("Nah, you aren’t being loud or anything. I just had trouble sleeping.", true,"Sad", "Shy", "null","null"),
            new dialogueEntry("Aw, sorry to hear that. But, good news, you’re just in time to try the first pancake!", false,"Happy", "Shy", "null","null"),
            new dialogueEntry("Oh! Uhm…wow! That looks…", true,"Happy", "Shy", "null","null"),
            new dialogueEntry("Hm…what do you think? Did it turn out well?", false,"Happy", "Shy", "null","null"),
            new dialogueEntry("Y..yeah! It’s great! ", true,"Happy", "Happy", "null","null"),
            new dialogueEntry("Do you need any help making the pancakes? I mean, not uhm, cause it looks like you need help or anything, but—", true,"Happy", "Shy", "null","null"),
            new dialogueEntry("*Sigh* I know. You can be honest with me. I know I ruined them.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("The truth is, I don’t know how to cook pancakes…or any food, for that matter.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("I just…I know that everyone missed out on pancakes because of my interview and I wanted to do something to make up for that…", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("Hey, you know that’s…really nice of you to do that for us.", true,"Sad", "Happy", "null","null"),
            new dialogueEntry("This honestly wasn’t bad for your first try! I’m down to help you out with cooking the rest of these. Mind if I show you the ropes?", true,"Sad", "Happy", "null","null"),
            new dialogueEntry("Not at all!", false,"Happy", "Happy", "null","null"),
            new dialogueEntry("Okay! Yeah, that’s really all there is to it. Wanna give it a go?", true,"Happy", "Happy", "null","null"),
            new dialogueEntry("Absolutely! I’ll do my best.", false,"Happy", "Happy", "null","null"),
            new dialogueEntry("Hey, I think I’m really starting to get the hang o-", false,"Happy", "Happy", "null","null"),
            new dialogueEntry("Ack! It got in my eye!", false,"Shy", "Happy", "null","null"),
            new dialogueEntry("Ah yeah, you gotta be careful with stir—", true,"Shy", "Happy", "null","null"),
            new dialogueEntry("Wait…got in your eye? But, you’re wearing glasses…", true,"Shy", "Sad", "null","null"),
            new dialogueEntry("Yes…normally, that would shield my eyes, wouldn’t it…?", false,"Sad", "Sad", "null","null"),
            new dialogueEntry("I’ll let you in on a secret, Seraphim…I don’t actually need glasses! They’re just fashion frames. I think they give me a more…professional feel, wouldn’t you agree?", false,"Happy", "Sad", "null","null"),
            new dialogueEntry("…Please don’t tell the others about this, though. That’d be really embarrassing.", false,"Sad", "Sad", "null","null"),
            new dialogueEntry("Ha! That’s just like you!", true,"Sad", "Happy", "null","null"),
            new dialogueEntry("…", false,"Sad", "Happy", "null","null"),
            new dialogueEntry("…", true,"Sad", "Shy", "null","null"),
            new dialogueEntry("Seraphim… about everything that happened a while back.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("I’ve been avoiding talking to you about it but…you didn’t deserve all the insults I threw at you. ", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("I used to berate you because it made me feel better about my own insecurities…but that’s no excuse for my behavior.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("I want you to know that I…I’m truly sorry.", false,"Sad", "Shy", "null","null"),
            new dialogueEntry("Mhm…you hurt me a lot…so it’s nice to hear a sincere apology from you.", true,"Sad", "Shy", "null","null"),
            new dialogueEntry("I know I said a lot of hurtful things to try and get back at you too. I’m also really sorry about that…I mean it.", true,"Sad", "Shy", "null","null"),
            new dialogueEntry("And…it’ll take a while to get used to…", true,"Sad", "Shy", "null","null"),
            new dialogueEntry("But, I think I’d like to get along better from now on.", true,"Sad", "Happy", "null","null"),
            new dialogueEntry("Yeah.…I would like that as well.", false,"Happy", "Happy", "null","null")
            }

        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Koralie","Beverly","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("I knew I smelled pancakes!",false,"Happy", "Happy","null","null"),
            new dialogueEntry("Looks like your pancakes turned out great as always, Sera!", true, "Happy", "Happy","null","null")
            }

        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Amery","Seraphim","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("Oh, I can’t take full credit for these! It was Amery’s idea to make pancakes, I just helped him out a little.", true, "Happy", "Happy","null","null"),
            new dialogueEntry("Heh, that’s true! Couldn’t have done it without your help though.", false,"Happy", "Happy", "null","null")
            }
        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Jade","Seraphim","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("You worked together to make these? Wow, it looks like you two are really getting along now.", false,"Happy", "Happy", "null","null"),
            new dialogueEntry("Yeah…guess we are, huh?", true,"Happy", "Happy", "null","null")
            }

        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("Jade","Koralie","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("Well, come on, let’s eat! I’m gonna absolutely drench these pancakes in syrup!", true,"Happy", "Happy", "null","null"),
            new dialogueEntry("Hell yeah! I’m starving.", false,"Happy", "Happy", "null","null")
            }

        );
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=new overallDialogue("","","ApartmentLight", 
        new dialogueEntry[]{
            new dialogueEntry("It’s bright and early, the sun softly shining through the apartment blinds. A warm, friendly air fills the kitchen as five college students enjoy their pancake breakfast…",false,"null","null")
            }

        );
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
