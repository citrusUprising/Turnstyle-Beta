using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetCharacterPassive : MonoBehaviour
{
    public string characterName;
    public PlayerAbilities characterProfiles;
    public TextMeshProUGUI label;

    private Passive passive;

    private string labelText = "";

    public bool isPassiveEnabled;

    // Start is called before the first frame update
    void Start()
    {

        selectCharacter();

        labelText += passive.name;
        labelText += "<font=\"Roboto-Regular SDF\"><size=24>\n";
        labelText += passive.description;
        labelText += "</font></size>";

        label.text = labelText;

        setPassiveEnabled(isPassiveEnabled);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void selectCharacter()
    {
        if (characterName == "koralie")
        {
            passive = characterProfiles.koralie.passive;
        }
        else if (characterName == "beverly")
        {
            passive = characterProfiles.beverly.passive;
        }
        else if (characterName == "amery")
        {
            passive = characterProfiles.amery.passive;
        }
        else if (characterName == "jade")
        {
            passive = characterProfiles.jade.passive;
        }
        else if (characterName == "seraphim")
        {
            passive = characterProfiles.seraphim.passive;
        }
    }

    public void setPassiveEnabled(bool state)
    {
        isPassiveEnabled = state; 

        if (isPassiveEnabled)
        {
            GetComponent<CanvasRenderer>().SetAlpha(1);
            transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(1);
            transform.GetChild(1).GetComponent<CanvasRenderer>().SetAlpha(1);
        }
        else
        {
            GetComponent<CanvasRenderer>().SetAlpha(0);
            transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0);
            transform.GetChild(1).GetComponent<CanvasRenderer>().SetAlpha(0);
        }
    }
}
