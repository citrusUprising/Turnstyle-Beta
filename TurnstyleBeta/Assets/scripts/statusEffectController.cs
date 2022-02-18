using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// TODO:
// add functionality in the nameTag script (or gameLoop?) that calls these functions at the appropriate times
// add tooltip popups that tell you what the status does
public class statusEffectController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // these are all the possible sprites that can be displayed in the left side of this object
    public Sprite aegisSprite;
    public Sprite burnSprite;
    public Sprite distractedSprite;
    public Sprite encumberedSprite;
    public Sprite enragedSprite;
    public Sprite flinchSprite;
    public Sprite hasteSprite;
    public Sprite nullSprite;
    public Sprite regenSprite;
    public Sprite strungOutSprite;

    // this sprite is literally a white dot, designed to appear invisible
    public Sprite noneSprite;

    // represents the current status. it can be any of the names of the sprites (minus "sprite" ofc)
    public string currentStatus;

    // represents how many turns are left in the current status. 
    public int turnsLeft;

    // represents how much for burn, regen, and haste
    public int magnitude;

    // a string that represents how many turns are left. this is updated with the function "updateTurnsLeftString()"
    // that function makes sure the string is "" instead of "0" if there are zero turns left
    private string turnsLeftString;

    // what kind of status does this represent? the options are "health" "buff" and "debuff"
    // this is set in the inspector and should be used in the nameTag script (or whatever updates statuses) to see if that player can get
    // a status of that kind
    // the idea is that you can't get a new status of a kind that you already have until the old one runs out,
    // so you can't get burn if you already have regen, for example
    public string kindOfStatus;

    // converts currentStatus into the respective sprite
    private Dictionary<string, Sprite> statusSpriteDict = new Dictionary<string, Sprite>();

    // the text object that displays how many turns are left
    private GameObject textObject;

    // the image object that displays what the current status is
    private GameObject imageObject;

    // this is set in the inspector and never changed
    // this class uses slightly different logic for enemies and allies
    // because monsters don't have a name tag, they don't have a permanent spot for status effects
    // that means that when a status effect is over, it should be destroyed 
    public string friendOrFoe;

    public statusTooltip tooltip;

    // Start is called before the first frame update
    void Start()
    {
        // yeah just filling up the dictionary
        statusSpriteDict["aegis"] = aegisSprite;
        statusSpriteDict["burn"] = burnSprite;
        statusSpriteDict["distracted"] = distractedSprite;
        statusSpriteDict["encumbered"] = encumberedSprite;
        statusSpriteDict["enraged"] = enragedSprite;
        statusSpriteDict["flinch"] = flinchSprite;
        statusSpriteDict["haste"] = hasteSprite;
        statusSpriteDict["null"] = nullSprite;
        statusSpriteDict["regen"] = regenSprite;
        statusSpriteDict["strungOut"] = strungOutSprite;
        statusSpriteDict["none"] = noneSprite;

        // getting the correct children
        imageObject = transform.GetChild(0).gameObject;
        textObject = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // stuff that i was using to debug. uncomment it and play with it if ur not sure how it all works together!

        /* if (Input.GetKeyDown(KeyCode.S))
        {
            updateStatus("burn", 2 3);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changeTurnCount(1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changeTurnCount(-1);
        } */

        updateStatus("burn", 2, 3);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(startTooltipTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide();
        StopAllCoroutines();
    }

    public IEnumerator startTooltipTimer()
    {
        yield return new WaitForSeconds(.5f);

        tooltip.show(currentStatus, turnsLeft, magnitude);
    }

    // this changes the status, the sprite, the turnsLeft, and the text displayed
    // the default values are used in changeTurnCount() to reset the object to a blank state, with no status or turns left
    public void updateStatus(string newStatus = "none", int newTurnCount = 0, int newMagnitude = 0)
    {
        // update the status variable
        currentStatus = newStatus;

        // update the image based on the dictionary
        imageObject.GetComponent<Image>().sprite = statusSpriteDict[currentStatus];

        // update turnsLeft
        turnsLeft = newTurnCount;

        magnitude = newMagnitude;

        // make sure the string version is right
        updateTurnsLeftString();

        // update the displayed text
        textObject.GetComponent<TextMeshProUGUI>().text = turnsLeftString;
    }

    // changes the turnsLeft
    public void changeTurnCount(int change)
    {
        // changes the turnsLeft
        turnsLeft += change;

        // updates the string
        updateTurnsLeftString();

        // if the turnsleft is above 0, it's fine
        if (turnsLeft > 0)
        {
            textObject.GetComponent<TextMeshProUGUI>().text = turnsLeftString;
        }
        
        // if it's below zero, that means we need to
        // for FRIENDS:
        //      change the display so the sprite is "none"
        // for FOES:
        //      destroy this object
        else
        {
            
            if (friendOrFoe == "friend")
            {
                // the default values are "none" and 0
                updateStatus();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    //updates the turnsLeftString variable
    void updateTurnsLeftString()
    {
        // casts it from int to string
        turnsLeftString = turnsLeft.ToString();

        // but if it's 0, instead we change turnsLeftString to ""
        if (turnsLeft == 0)
        {
            turnsLeftString = "";
        }
    }
}
