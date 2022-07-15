using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class nameTag : MonoBehaviour
{
    private GameObject passiveSprite;
    private GameObject fatigue;
    private GameObject healthBar;
    private GameObject hpText;

    public int fatigueValue;
    public int hpValue;
    public int hpValueMax;
    private string hpValueString;

    public Vector3 previousPosition;
    public Vector3 nextPosition;
    private bool isPassiveHidden = true;
    private bool passiveIsAnimating = false;

    private Vector3 passiveHiddenLocation;
    private Vector3 passiveShownLocation;
    private float t = 0.0f;
    // private statusTooltip tooltipA;

    public GameObject character;

    public GameObject healthStatusTracker;
    public GameObject buffStatusTracker;
    public GameObject debuffStatusTracker;

    public Sprite speedSelectImage;
    public Sprite totalSpeedImage;

    public Color confirmColor;
    public bool isDead = false;
    public bool isGone = false;

    // Start is called before the first frame update
    void Start()
    {
        passiveSprite = transform.GetChild(0).gameObject;
        fatigue = transform.GetChild(3).gameObject;
        healthBar = transform.GetChild(4).gameObject;
        hpText = transform.GetChild(5).gameObject;

        Friendly test = character.GetComponent<Friendly>();
        if(test){
            Debug.Log("Got character info"+test);
            hpValueMax = test.maxHP;   
            hpValue = test.hp;     
        }
        else{
            Debug.Log("Character object error.");
        }
       
        passiveHiddenLocation = new Vector3(passiveSprite.transform.localPosition[0], passiveSprite.transform.localPosition[1], 0);
        passiveShownLocation = new Vector3(passiveSprite.transform.localPosition[0], passiveSprite.transform.localPosition[1] - 96, 0); 

        // tooltipA = transform.GetChild(9).statusTooltip;
        // tooltipA.hide();
        adjustHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //adjustHealth();
        animatePassive();
    }

    public void adjustHealth(){
        hpValue = character.GetComponent<Friendly>().hp;
        fatigue.GetComponent<TextMeshProUGUI>().text = character.GetComponent<Friendly>().fatigue.ToString();

        hpValueString = "HP " + hpValue.ToString() + "/" + hpValueMax.ToString();

        hpText.GetComponent<TextMeshProUGUI>().text = hpValueString;

        healthBar.GetComponent<Image>().fillAmount = (float)hpValue / (float)hpValueMax;
    }
    
    public void togglePassiveShowing()
    {
        if (isPassiveHidden)
        {
            isPassiveHidden = false;
        } 
        else
        {
            isPassiveHidden = true;
        }

        passiveIsAnimating = true;

        if (t != 0f)
        {
            t = 1f - t;
        }
    }

    public void showPassive()
    {
        if (isPassiveHidden)
        {
            isPassiveHidden = false;

            passiveIsAnimating = true;

            if (t != 0f)
            {
                t = 1f - t;
            }
        }
    }
        

    public void hidePassive()
    {
        if (!isPassiveHidden)
        {
            isPassiveHidden = true;

            passiveIsAnimating = true;

            if (t != 0f)
            {
                t = 1f - t;
            }
        }
    }

    void animatePassive()
    {
        if (passiveIsAnimating)
        {

            if (isPassiveHidden)
            {
                passiveSprite.transform.localPosition = Vector3.Lerp(passiveShownLocation, passiveHiddenLocation, t);
            }
            else 
            {
                passiveSprite.transform.localPosition = Vector3.Lerp(passiveHiddenLocation, passiveShownLocation, t);
            }

            t += 2.5f * Time.deltaTime;

            if (t > 1.0f)
            {
                t = 1f;

                if (isPassiveHidden)
                {
                    passiveSprite.transform.localPosition = Vector3.Lerp(passiveShownLocation, passiveHiddenLocation, t);
                }
                else
                {
                    passiveSprite.transform.localPosition = Vector3.Lerp(passiveHiddenLocation, passiveShownLocation, t);
                }

                t = 0f;
                passiveIsAnimating = false;
            }
        }
    }

    public void KillGray(){
        isDead = true;
        Image[] temp = this.GetComponentsInChildren<Image>();
        for (int i = 0; i < temp.Length; i++){
            Debug.Log(temp[i]+" is now changing colors");
            temp[i].color = new Color (0.35f,0.35f,0.35f,1.0f);
        }
    }

    public void Gone(){
        if(isGone){
            isDead = true;
            character.GetComponent<Unit>().dead = true;
            Image[] tempImage = this.GetComponentsInChildren<Image>();
            TextMeshProUGUI[] tempText = this.GetComponentsInChildren<TextMeshProUGUI>();
            Color tempColor;
            for (int i = 0; i < tempImage.Length; i++)
            tempImage[i].color = new Color (1f,1f,1f,0f);
            for (int i = 0; i < tempText.Length; i++){
            tempColor = tempText[i].color;
            tempColor.a = 0f;
            tempText[i].color = tempColor;
            }
        }
    }

    public void updateAllStatuses()
    {
        // this was the code i was using to test the duration on the status effects before
        // they get sent to the status effect contorller
        // SPECIFICALLY this chunk of commented out code logs the value of the duration of the health status effects on every ally
        // it does it very often, so it logs a lot of the same number
        /*
        if ((int)character.GetComponent<Unit>().statuses[(int)StatusType.Health].name != 0)
        {
            int duration = (int)character.GetComponent<Unit>().statuses[(int)StatusType.Health].duration;

            Debug.LogWarning(duration.ToString());
        }
        */


        healthStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Health].name,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Health].duration,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Health].magnitude);

        buffStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Buff].name,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Buff].duration,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Buff].magnitude);

        debuffStatusTracker.GetComponent<statusEffectController>().updateStatus(
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Debuff].name,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Debuff].duration,
            (int)character.GetComponent<Unit>().statuses[(int)StatusType.Debuff].magnitude);
    }
}
