using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statusPopUp : MonoBehaviour
{
    private Image display;
    private void Awake(){
        display = transform.GetComponent<Image>();
    }
    public void Setup(StatusName inflict){
        switch(inflict){
            case StatusName.None: default:
            display.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
            break;

            case StatusName.Regeneration: case StatusName.Burn: case StatusName.FatigueUP: case StatusName.Flinch:
            case StatusName.Haste: case StatusName.Null: case StatusName.Shielded: case StatusName.Strengthened:
            case StatusName.Vulnerable: case StatusName.Weakened:
            display.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
            Sprite temp = Resources.Load<Sprite>("StatusIcons/icon"+inflict.ToString());
            display.sprite = temp;
            break;

        }
    }

    public static statusPopUp Create(StatusName inflict, Vector3 origin){
        Transform stts = Instantiate(AssetCarry.i.status, origin, Quaternion.identity);  
        statusPopUp sttsEvent = stts.GetComponent<statusPopUp>();
        sttsEvent.Setup(inflict);
        
        return sttsEvent;
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
