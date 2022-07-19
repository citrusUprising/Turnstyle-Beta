using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class totalSpeed : MonoBehaviour
{
    public GameObject textObject;
    public int currentDisplayedSpeed;

    public Sprite blueSprite;
    public Sprite greenSprite;
    public Sprite pinkSprite;
    public Sprite yellowSprite;
    public Sprite redSprite;

    // Start is called before the first frame update
    void Start()
    {
        textObject = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        textObject.GetComponent<TextMeshProUGUI>().text = currentDisplayedSpeed.ToString();
    }

    public void changeSprite(Sprite spriteToChangeTo)
    {
        GetComponent<Image>().sprite = spriteToChangeTo;
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
