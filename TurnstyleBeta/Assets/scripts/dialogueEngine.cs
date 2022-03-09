using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueEntry{
	string line;
	bool speaker; // = false if left speaker, true if right
}

public class dialogueEngine : MonoBehaviour
{
	int currentLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WriteLine(){
    	yield return new WaitForSeconds(1);
    }
}
