using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popUpHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform popUpLoc;
    private void Start()
    {
        Instantiate(popUpLoc, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
