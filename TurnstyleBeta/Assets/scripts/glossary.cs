using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glossary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            transform.localPosition = new Vector3(-144, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(2000, 0, 0);
        }
    }
}
