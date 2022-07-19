using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enemySpeedController : MonoBehaviour
{
    public GameObject topColorSprite;
    public GameObject bottomColorSprite;

    public Enemy enemy;

    private Color topColor0 = new Color((float)188/255, (float)63/255, (float)38/255, 1);
    private Color topColor12 = new Color((float)123/255, (float)32/255, (float)145/255, 1);

    private Color bottomColor0 = new Color((float)221/255, (float)142/255, (float)111/255, 1);
    private Color bottomColor12 = new Color((float)191/255, (float)99/255, (float)224/255, 1);

    private Color currentTopColor;
    private Color currentBottomColor;

    public int speed;

    public TextMeshProUGUI textObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSpeed()
    {
        // this is all broken right now and i don't know why T-T
        float speedPercent = (float) speed / 12f;

        Debug.Log("speed: " + speed);
        Debug.Log("speed percent: " + speedPercent);

        currentTopColor = Color.Lerp(topColor0, topColor12, speedPercent);
        currentBottomColor = Color.Lerp(bottomColor0, bottomColor12, speedPercent);

        topColorSprite.GetComponent<Image>().color = currentTopColor;
        bottomColorSprite.GetComponent<Image>().color = currentBottomColor;

        textObject.text = speed.ToString();
    }
}
