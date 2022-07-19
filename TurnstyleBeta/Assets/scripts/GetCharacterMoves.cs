using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetCharacterMoves : MonoBehaviour
{

    public string characterName;
    public PlayerAbilities characterProfiles;
    public TextMeshProUGUI label;

    private Ability[] abilities = new Ability[3];

    private string labelText = "";

    // Start is called before the first frame update
    void Start()
    {

        selectCharacter();

        for (int i = 0; i < 3; i++)
        {
            labelText += abilities[i].name;
            labelText += "<font=\"Roboto-Regular SDF\"><size=24>\n";
            labelText += abilities[i].text;

            // if this is the third move being written, we don't want to add any spaces on the end
            if (i != 2)
            {
                labelText += "<line-height=150%>\n</line-height>";
            }

            labelText += "</font></size>";
        }

        label.text = labelText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void selectCharacter()
    {
        if (characterName == "koralie")
        {
            abilities = characterProfiles.koralie.abilities;
        }
        else if (characterName == "beverly")
        {
            abilities = characterProfiles.beverly.abilities;
        }
        else if (characterName == "amery")
        {
            abilities = characterProfiles.amery.abilities;
        }
        else if (characterName == "jade")
        {
            abilities = characterProfiles.jade.abilities;
        }
        else if (characterName == "seraphim")
        {
            abilities = characterProfiles.seraphim.abilities;
        }
    }
}
