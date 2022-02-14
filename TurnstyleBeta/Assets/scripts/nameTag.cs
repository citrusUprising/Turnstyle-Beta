using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class nameTag : MonoBehaviour
{
    private GameObject fatigue;
    private GameObject healthBar;
    private GameObject hpText;
    public int fatigueValue;
    public int hpValue;
    public int hpValueMax;
    private string hpValueString;
    public Vector3 previousPosition;
    public Vector3 nextPosition;

    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        fatigue = transform.GetChild(1).gameObject;
        healthBar = transform.GetChild(2).gameObject;
        hpText = transform.GetChild(3).gameObject;
        Friendly test = character.GetComponent<Friendly>();
        if(test){
            Debug.Log("Got character info"+test);
            hpValueMax = test.maxHP;   
            hpValue = test.hp;     
        }
        else{
            Debug.Log("Character object error.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        fatigue.GetComponent<TextMeshProUGUI>().text = fatigueValue.ToString();

        hpValueString = "HP " + hpValue.ToString() + "/" + hpValueMax.ToString();

        hpText.GetComponent<TextMeshProUGUI>().text = hpValueString;

        healthBar.GetComponent<Image>().fillAmount = (float)hpValue / (float)hpValueMax;
    }

}
