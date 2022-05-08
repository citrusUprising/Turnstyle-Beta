using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyPromptManager : MonoBehaviour
{

    public GameObject[] prompts;

    public GameObject currentPrompt;

    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePrompt(int index)
    {
        Destroy(currentPrompt);

        if (index >= 0)
        {
            currentPrompt = Instantiate(prompts[index], canvas.transform);
        }
    }
}
