using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class statusTooltip : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public nameTag parentNameTag;

    public string[] statusEffectDescriptions = new string[11];

    private string[] statusStringArray = new string[11];

    public string text;

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
        statusEffectDescriptions[(int)StatusName.Shielded] = "Takes 50% less damage.";
        statusEffectDescriptions[(int)StatusName.Burn] = "Takes *N* damage each turn.";
        statusEffectDescriptions[(int)StatusName.Vulnerable] = "Takes 100% more damage.";
        statusEffectDescriptions[(int)StatusName.FatigueUP] = "Gains 1 extra fatigue each turn.";
        statusEffectDescriptions[(int)StatusName.Strengthened] = "Deals 100% more damage.";
        statusEffectDescriptions[(int)StatusName.Flinch] = "Can't take an action this turn.";
        statusEffectDescriptions[(int)StatusName.Haste] = "Gets +*N* speed.";
        statusEffectDescriptions[(int)StatusName.Null] = "Cannot get another debuff.";
        statusEffectDescriptions[(int)StatusName.Regeneration] = "Heals *N* HP each turn.";
        statusEffectDescriptions[(int)StatusName.Weakened] = "Deals 50% less damage.";
        statusEffectDescriptions[(int)StatusName.None] = "";

        statusStringArray[(int)StatusName.Shielded] = "Shielded"; //flag
        statusStringArray[(int)StatusName.Burn] = "Burn";
        statusStringArray[(int)StatusName.Vulnerable] = "Vulnerable"; //flag
        statusStringArray[(int)StatusName.FatigueUP] = "Fatigue Up"; //flag
        statusStringArray[(int)StatusName.Strengthened] = "Strengthened"; //flag
        statusStringArray[(int)StatusName.Flinch] = "Flinch";
        statusStringArray[(int)StatusName.Haste] = "Haste";
        statusStringArray[(int)StatusName.Null] = "Null";
        statusStringArray[(int)StatusName.Regeneration] = "Regeneration";
        statusStringArray[(int)StatusName.Weakened] = "Weakened"; //flag
        statusStringArray[(int)StatusName.None] = "";

        hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show(int statusEffect, int turnsLeft, int magnitude)
    {
        if (statusEffect != 0)
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
                "<size=14px><font=\"Staatliches-Regular SDF\"><b>" + statusStringArray[statusEffect] + ": " + turnsLeftText + "</b></font></size>" + "\n" +

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
