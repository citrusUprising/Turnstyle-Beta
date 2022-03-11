using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenuTextSpeed : MonoBehaviour
{
    public pauseMenu pauseMenu;

    private bool isShowing = false;

    public GameObject pointer;

    public GameObject labelCombat;
    public GameObject labelDialogue;

    public GameObject sliderCombat;
    public GameObject sliderDialogue;

    private GameObject selectedLabel;

    public float combatTextSpeed;
    public float combatTextSpeedPercent;

    private float combatTextSpeedMaxValue = 2f;
    private float combatTextSpeedMinValue = .25f;

    public float dialogueTextSpeed;
    public float dialogueTextSpeedPercent;

    private float dialogueTextSpeedMaxValue = .1f;
    private float dialogueTextSpeedMinValue = .01f;

    // Start is called before the first frame update
    void Start()
    {
        selectedLabel = labelCombat;

        combatTextSpeedPercent = PlayerPrefs.GetFloat("combatTextSpeedPercent", .5f);
        combatTextSpeed = PlayerPrefs.GetFloat("combatTextSpeed", getCombatTextSpeed());
        sliderCombat.GetComponent<Image>().fillAmount = combatTextSpeedPercent;

        dialogueTextSpeedPercent = PlayerPrefs.GetFloat("dialogueTextSpeedPercent", .5f);
        dialogueTextSpeed = PlayerPrefs.GetFloat("dialogueTextSpeed", getDialogueTextSpeed());
        sliderDialogue.GetComponent<Image>().fillAmount = dialogueTextSpeedPercent;
    }

    // Update is called once per frame
    void Update()
    {
        isShowing = pauseMenu.pauseMenuItemsShowing[3];

        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                toggleSelectedLabel();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                editSelectedLabel(0.05f);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                editSelectedLabel(-0.05f);
            }

            realignPointer();
        } 
        else
        {
            movePointerOffscreen();
        }
        
        
    }

    void toggleSelectedLabel()
    {
        if (selectedLabel == labelCombat)
        {
            selectedLabel = labelDialogue;
        }
        else if (selectedLabel == labelDialogue)
        {
            selectedLabel = labelCombat;
        }
    }

    void editSelectedLabel(float change)
    {
        if (selectedLabel == labelCombat)
        {
            sliderCombat.GetComponent<Image>().fillAmount += change;
            combatTextSpeedPercent = sliderCombat.GetComponent<Image>().fillAmount;
            combatTextSpeed = getCombatTextSpeed();

            PlayerPrefs.SetFloat("combatTextSpeedPercent", combatTextSpeedPercent);
            PlayerPrefs.SetFloat("combatTextSpeed", combatTextSpeed);
        }
        else if (selectedLabel == labelDialogue)
        {
            sliderDialogue.GetComponent<Image>().fillAmount += change;
            dialogueTextSpeedPercent = sliderDialogue.GetComponent<Image>().fillAmount;
            dialogueTextSpeed = getDialogueTextSpeed();

            PlayerPrefs.SetFloat("dialogueTextSpeedPercent", dialogueTextSpeedPercent);
            PlayerPrefs.SetFloat("dialogueTextSpeed", dialogueTextSpeed);

            Debug.Log(dialogueTextSpeed);
        }
    }

    void realignPointer()
    {
        Vector3 pointerPos = selectedLabel.GetComponent<RectTransform>().position;

        var rect = gameObject.GetComponent<RectTransform>().rect;

        pointerPos[0] += (float) rect.left;

        pointer.GetComponent<RectTransform>().position = pointerPos;
    }

    void movePointerOffscreen()
    {
        Vector3 pointerPos = new Vector3(-Screen.width * 2, -Screen.height * 2, 0);

        pointer.GetComponent<RectTransform>().position = pointerPos;
    }

    float getCombatTextSpeed()
    {
        return Mathf.Lerp(combatTextSpeedMinValue, combatTextSpeedMaxValue, 1 - combatTextSpeedPercent);
    }

    float getDialogueTextSpeed()
    {
        return Mathf.Lerp(dialogueTextSpeedMinValue, dialogueTextSpeedMaxValue, 1 - dialogueTextSpeedPercent);
    }
}
