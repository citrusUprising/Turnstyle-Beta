using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagePopUp : MonoBehaviour
{
    private TextMeshPro readOut;
    private void Awake(){
        readOut = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int health){
        readOut.SetText(health.ToString());
    }


    public static damagePopUp Create(int dmg, Vector3 origin){
        Debug.Log("Starting Popup creation");
        Transform nmbr = Instantiate(AssetCarry.i.popUp, origin, Quaternion.identity);
        damagePopUp nmbrEvent = nmbr.GetComponent<damagePopUp>();
        nmbrEvent.Setup(dmg);
        return nmbrEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
