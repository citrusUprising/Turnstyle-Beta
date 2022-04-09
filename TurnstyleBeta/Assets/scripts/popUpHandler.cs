using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popUpHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {   
        Debug.Log("Popups should now be visible");
        damagePopUp.Create(30, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
