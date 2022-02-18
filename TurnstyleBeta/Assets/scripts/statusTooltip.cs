using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class statusTooltip : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public nameTag parentNameTag;

    public string text;
    public Dictionary<string, string> statusEffectDescriptions = new Dictionary<string, string>();

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ :-)
    //
    //                              PLAN                             
    //
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ :-)
    // 1. have this object set as inactive in the inspector and have it be shown in the nameTag or in the status object
    // 2. make a dictionary in start() that gets all the descriptions for each status                                           DONE
    // 3. use that PLUS turns left and magnitude to build a string                                                              DONE
    // 4. have the tooltip show with a coroutine that is called when you hover over the status with a .5 or 1 second delay      DONE
    //    ONLY IF the status obejct has a status displayed
    // 5. cancel that coroutine if the player hovers off of the status                                                          DONE
    // 6. hide the tooltip when the player stops hovering over the status                                                       DONE

    // Start is called before the first frame update
    void Start()
    {
        

        statusEffectDescriptions["aegis"] = "Takes 50% less damage.";
        statusEffectDescriptions["burn"] = "Takes *N* damage each turn.";
        statusEffectDescriptions["distracted"] = "Takes 100% more damage.";
        statusEffectDescriptions["encumbered"] = "Gains 1 extra fatigue each turn.";
        statusEffectDescriptions["enraged"] = "Deals 100% more damage.";
        statusEffectDescriptions["flinch"] = "Can't take an action this turn.";
        statusEffectDescriptions["haste"] = "Gets +*N* speed.";
        statusEffectDescriptions["null"] = "Cannot get another debuff.";
        statusEffectDescriptions["regen"] = "Heals *N* HP each turn.";
        statusEffectDescriptions["strungOut"] = "Deals 50% less damage.";
        statusEffectDescriptions["none"] = "";

        hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show(string statusEffect, int turnsLeft, int magnitude)
    {
        if (statusEffect != "none")
        {

            string turnsLeftText;

            gameObject.SetActive(true);

            if (turnsLeft > 1)
            {
                turnsLeftText = turnsLeft.ToString() + " Turns";
            }
            else
            {
                turnsLeftText = turnsLeft.ToString() + " Turn";
            }

            text =
            // first line:
            // status effect name (in bold, upper case). the status effect name is split between two expressions 
            // the turns left, as a string so that if there is one turn left, it cuts the "s"
                "<size=14px><font=\"Staatliches-Regular SDF\"><b>" + char.ToUpper(statusEffect[0]) + statusEffect.Substring(1) + ": " + turnsLeftText + "</b></font></size>" + "\n" +

                // second line:
                // the description of the current status. 
                // i use a replacement function to get the magnitude, in the case of regen, haste, and burn
                statusEffectDescriptions[statusEffect].Replace("*N*", magnitude.ToString());

            displayText.text = text;
        }  
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }
}
