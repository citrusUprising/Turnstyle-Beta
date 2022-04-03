using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textPreviewCombat : MonoBehaviour
{

    public pauseMenuTextSpeed controls;

    public float textSpeed;

    private string shownText = "";

    private string[] exampleText = new string[10];

    private int numOfLinesShown = 0;

    // Start is called before the first frame update
    void Start()
    {
        exampleText[0] = "Beverly used Smolder!";
        exampleText[1] = "Amery used Unionize!";
        exampleText[2] = "Koralie used Crush!";
        exampleText[3] = "Jade used Rally!";
        exampleText[4] = "Seraphim used Scry!";
        exampleText[5] = "Beverly used Dazzle!";
        exampleText[6] = "Amery used Mitigate!";
        exampleText[7] = "Koralie used Repel!";
        exampleText[8] = "Jade used Motivate!";
        exampleText[9] = "Seraphim used Slump!";
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
    }

    public void resetText(bool skip)
    { 
        numOfLinesShown = -1;
        shownText = "";
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
        StopAllCoroutines();

        if (skip == true)
        {
            StartCoroutine(showNextLine());
        }
        else if (skip == false)
        {
            StartCoroutine(beginShowingText());
        }
    }

    public void deleteText()
    {
        numOfLinesShown = -1;
        shownText = "";
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
        StopAllCoroutines();
    }

    IEnumerator beginShowingText()
    {

        yield return new WaitForSeconds(.25f);

        StartCoroutine(showNextLine());
    }

    IEnumerator showNextLine()
    {

        numOfLinesShown++;

        if (numOfLinesShown < exampleText.Length)
        {
            shownText += exampleText[numOfLinesShown] + "\n";

            yield return new WaitForSeconds(textSpeed);

            StartCoroutine(showNextLine());
        }
        else
        {
            yield break;
        }
        
    }
}
