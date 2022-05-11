using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textPreviewDialogue : MonoBehaviour
{

    public pauseMenuTextSpeed controls;

    public float textSpeed;

    private string shownText = "";

    private string exampleText;

    private int numOfCharsShown = 0;

    // Start is called before the first frame update
    void Start()
    {
        exampleText = "It’s bright and early, the sun softly shining " +
                      "through the apartment curtains. It is calm and quiet, " +
                      "four college students sleeping soundly. A rare " +
                      "moment in an otherwise loud, often arguing " +
                      "household of five...";
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
    }

    public void resetText(bool skip)
    {
        numOfCharsShown = -1;
        shownText = "";
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
        StopAllCoroutines();

        if (skip == true)
        {
            StartCoroutine(showNextChar());
        }
        else if (skip == false)
        {
            StartCoroutine(beginShowingText());
        }
    }

    public void deleteText()
    {
        numOfCharsShown = -1;
        shownText = "";
        gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
        StopAllCoroutines();
    }

    IEnumerator beginShowingText()
    {

        yield return new WaitForSeconds(.25f);

        StartCoroutine(showNextChar());
    }

    IEnumerator showNextChar()
    {

        numOfCharsShown++;

        if (numOfCharsShown < exampleText.Length)
        {
            shownText = exampleText.Remove(numOfCharsShown);

            yield return new WaitForSeconds(textSpeed);

            StartCoroutine(showNextChar());
        }
        else if (numOfCharsShown == exampleText.Length)
        {
            shownText = exampleText;

            yield break;
        }
        else
        {
            yield break;
        }

    }
}
