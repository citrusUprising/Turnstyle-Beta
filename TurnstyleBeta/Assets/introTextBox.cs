using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introTextBox : MonoBehaviour
{

    public float animationDuration;

    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;

    public introAnimationController animationController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAnimation(float duration, int animationNumber)
    {
        StartCoroutine(lerp(duration, animationNumber));
    }

    IEnumerator lerp(float duration, int animationNumber)
    {
        float time = 0f;

        Vector3 oldPos = Vector3.zero;
        Vector3 newPos = Vector3.zero;

        if (animationNumber == 0)
        {
            oldPos = pos1;
            newPos = pos2;
        }
        else if (animationNumber == 1)
        {
            oldPos = pos2;
            newPos = pos3;
        }

        while (time < duration)
        {

            float t = time / duration;

            t = easeInQuint(t);

            transform.localPosition = Vector3.Lerp(oldPos, newPos, t);

            time += Time.deltaTime * animationController.timeMultiplier;

            yield return null;
        }

        transform.localPosition = newPos;
    }

    // from easings.net
    float easeInQuint(float x) 
    {
        return x* x * x* x;

    }
}
