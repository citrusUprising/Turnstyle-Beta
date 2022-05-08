using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class keyPromptGetTextColor : MonoBehaviour
{
    public string currentScene;

    private Color nodeMapColor = new Color(.098f, .098f, .098f, 1);
    private Color combatColor = new Color(.8f, .8f, .8f, 1);
    private Color pauseColor = new Color(.2f, .2f, .2f, 1);

    private Color currentColor;

    private TextMeshProUGUI[] texts;

    // Start is called before the first frame update
    void Start()
    {
        if (currentScene == "node map" || currentScene == "main menu")
        {
            currentColor = nodeMapColor;
        }
        else if (currentScene == "combat" || currentScene == "cutscene")
        {
            currentColor = combatColor;
        }
        else if (currentScene == "pause")
        {
            currentColor = pauseColor;
        }

        texts = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = currentColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
