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

    private Color topColor0 = new Color(188, 63, 38, 1);
    private Color topColor12 = new Color(123, 32, 145, 1);

    private Color bottomColor0 = new Color(221, 142, 111, 1);
    private Color bottomColor12 = new Color(191, 99, 224, 1);

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
        float speedPercent = speed / 12;

        currentTopColor = Color.Lerp(topColor0, topColor12, speedPercent);
        currentBottomColor = Color.Lerp(bottomColor0, bottomColor12, speedPercent);

        topColorSprite.GetComponent<Image>().color = topColor12;
        bottomColorSprite.GetComponent<Image>().color = bottomColor12;

        textObject.text = speed.ToString();
    }
}
