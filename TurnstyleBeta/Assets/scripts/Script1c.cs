using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1c : MonoBehaviour
{

    public overallDialogue[] script;
    public int dialogueVarietyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        overallDialogue[] temp = new overallDialogue [100];

        temp[dialogueVarietyCount]=(new overallDialogue("","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Koralie patiently waits at the subway station for the rest of the team. They zone out on their phone and are currently in the middle of a fierce gacha pull!",false,"Happy","Happy","Cutscene/subway_ambience","null") //gacha sounds
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Hm…nothing good yet…how unlucky…",false,"Angry","Happy","null","null"), //gacha sounds
            new dialogueEntry("Oh…? A gold card?!? YES! I wonder who it’ll be…",false,"Happy","Happy","null","null"), //gacha sounds
            new dialogueEntry("Oh no! It’s only the Super Rare card for this event…hmph!",false,"Angry","Happy","null","null"), //gacha sounds
            new dialogueEntry("Well, at least she’s cute…and, if I keep rolling, eventually I’ll get an Ultra Rare…",false,"Happy","Happy","null","null"), //gacha sounds
            new dialogueEntry("*footsteps*",true,"Happy","Happy","null","Cutscene/footsteps")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Beverly","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Hey Koralie! Didn’t expect you to be at the subway station so early! Whatcha playing? Seems like you’re really into that game.",true,"Happy","Happy","null","stop"),
            new dialogueEntry("Oh! Wow, didn’t notice you there…it’s just a silly mobile game, haha. Do you uhm…always get here this early?",false,"Shy","Happy","null","null"),
            new dialogueEntry("Yeah! My last class ends pretty early, so I usually just stick around here until the rest of y’all show up.",true,"Happy","Happy","null","null"),
            new dialogueEntry("I see…I didn’t realize that you always come at this time…I wouldn’t have come if I knew that.",false,"Angry","Happy","null","null"),
            new dialogueEntry("Oh…do you uhm…not want me to be here…?",true,"Angry","Sad","null","null"),
            new dialogueEntry("I mean like, I keep trying to hang out with you but…it seems like you’re always avoiding me…do you not like me or…?",true,"Angry","Sad","null","null"),
            new dialogueEntry("Ah…sorry, I really didn’t mean to give that impression…I don’t dislike you at all!",false,"Happy","Sad","null","null"),
            new dialogueEntry("Quite the opposite in fact…",false,"Shy","Sad","null","null"),
            new dialogueEntry("I’d like to make it up to you so…maybe we could go out somewhere?",false,"Happy","Sad","null","null"),
            new dialogueEntry("Wh…huh? You…you want to go out with me?",true,"Happy","Shy","null","null"),
            new dialogueEntry("N-no!! I mean…no, just a casual hangout, we could go to the cat cafe or something…",false,"Shy","Shy","null","null"),
            new dialogueEntry("I know that place is expensive but I’m willing to pay! How does Wednesday sound?",false,"Happy","Shy","null","null"),
            new dialogueEntry("Yeah, I would totally love that! It’s a date!",true,"Happy","Happy","null","null"),
            new dialogueEntry("A…date. Yeah…",false,"Shy","Happy","null","null"),
            new dialogueEntry("…",true,"Shy","Shy","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Jade","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Hey you two, how’s it going?",true,"Shy","Happy","Music/MapTheme","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Are we uhm…interrupting something?",true,"Shy","Shy","null","null"),
            new dialogueEntry("Ah, no! Nothing at all! …But yes, I’m fine, although I suppose I’m a bit tired. ",false,"Shy","Shy","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Jade","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Tell me about it…we’re all tired cuz of Amery’s stupid internship interview. He really woke us all up for something that he’s not even gonna get accepted for.",true,"Shy","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Jade","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Now, wait just a second, Jade! Amery is just very…driven. We should all support him in achieving his goals…even if they likely won’t work out.",false,"Angry","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Look…I know I said I’d quit it earlier, but do you mind hearing me out for a bit, Bev?",true,"Angry","Shy","null","null"),
            new dialogueEntry("Yeah, of course. Go ahead, Seraphim.",false,"Sad","Shy","null","null"),
            new dialogueEntry("Well…we’re not trying to stop Amery from doing anything. We just want him to be considerate of everyone else’s feelings and schedules for once! ",true,"Sad","Angry","null","null"),
            new dialogueEntry("I get that he’s career focused and all, and I don’t mean to make fun of him for it. He’s just a frustrating person to deal with sometimes…",true,"Sad","Shy","null","null"),
            new dialogueEntry("Yeah…that’s fair. I def understand why you were upset earlier.",false,"Sad","Shy","null","null"),
            new dialogueEntry("Gotta admit…it was pretty annoying of Amery to do that. I didn’t even get to make pancakes for everyone like I had planned today.",false,"Angry","Shy","null","null"),
            new dialogueEntry("We missed out on pancakes cause of him???",true,"Angry","Shy","null","null"),
            new dialogueEntry("It was supposed to be a surprise…",false,"Sad","Shy","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Well, in any case, I agree. Amery’s my friend and all, but that whole thing was completely unnecessary. It cut into my well-earned beauty sleep, and for wh—?",false,"Angry","Shy","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("*footsteps*",true,"Angry","Happy","null","Cutscene/footsteps")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Amery","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Aha! I knew it! You’re all badmouthing me! The others I expected, but even you two? Bev, Koralie, how could you?!",true,"Angry","Angry","stop","stop"), // stop music for dramatic effect
            new dialogueEntry("I know that you all have been talking behind my back since this morning! You never show me the respect that I deserve!",true,"Angry","Angry","null","null"),
            new dialogueEntry("You all think I’m a failure, don’t you? You think I’m incompetent because I can’t land an internship despite all my hard work, huh? Well, fine!",true,"Angry","Angry","null","null"),
            new dialogueEntry("I don’t need your help! I don’t need anyone else AT ALL! I can fight off the subway monsters just fine by myself!",true,"Angry","Angry","null","null"),
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("*Amery ran away*",true,"Angry","Angry","null","Cutscene/footsteps")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Wait, Amery!",false,"Sad","Happy","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("He…he really went on the subway alone!",true,"Sad","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Koralie","Seraphim","Station", 
        new dialogueEntry[]{
            new dialogueEntry("Well, don’t just stand there! We have to go help him!",false,"Angry","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","","Train", 
        new dialogueEntry[]{
            new dialogueEntry("We barely made it…do y’all see where he went?",false,"Sad","Happy","null","Cutscene/footsteps")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Koralie","Train", 
        new dialogueEntry[]{
            new dialogueEntry("There he is!",true,"Sad","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("","Amery","Train", 
        new dialogueEntry[]{
            new dialogueEntry("Wh…why are you all here?!",true,"Happy", "Angry","null","null"),
            new dialogueEntry("Agh! ",true,"Happy", "Shy","null","Battle/damage"),
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Beverly","Amery","Train", 
        new dialogueEntry[]{
            new dialogueEntry("We’re here to help you! Now, come on, let’s fight these things!",false,"Happy","Shy","Music/BattleTheme","null"),
            new dialogueEntry("NO! What don’t you understand? I can do this just fine by myself! I don’t need your pity! And I definitely don’t need help from people who hate me!",true,"Happy","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Jade","Amery","Train", 
        new dialogueEntry[]{
            new dialogueEntry("...Listen, Amery. It’s true that I think you’re kind of an ass sometimes, I’ll admit it. But, I speak for all of us when I say that we’re sorry and we still care about you dude.",false,"Shy","Angry","null","null"),
            new dialogueEntry("We don’t hate you, and we’re not just gonna leave you to deal with these monsters on your own!",false,"Shy","Angry","null","null")
            }

        ));
        dialogueVarietyCount++;

        temp[dialogueVarietyCount]=(new overallDialogue("Seraphim","Amery","Train", 
        new dialogueEntry[]{
            new dialogueEntry("It’s hard for me to admit…but I feel like I should apologize too. I shouldn’t have said all those things about you. ",false,"Shy","Angry","null","null"),
            new dialogueEntry("It’s just the way you treat us sometimes…it feels like you don’t care about our feelings. Please, just let us help you, and we can work the rest of this out at home!",false,"Shy","Angry","null","null"),
            new dialogueEntry("I…Fine…Sorry. For the time being, I’ll let you fight with me…and we can talk more about this later.",true,"Shy","Sad","null","null"),
            new dialogueEntry("Now, let’s go!",true,"Sad","Angry","null","null")
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
