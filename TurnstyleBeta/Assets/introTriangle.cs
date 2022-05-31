using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introTriangle : MonoBehaviour
{

    public float animationDuration;
    
    public bool isGoingIn = true;

    private Vector3 oldPos = Vector3.zero;
    private Vector3 newPos = Vector3.zero;

    public Vector3 outPos;
    private Vector3 inPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAnimation(float duration)
    {
        StartCoroutine(animate(duration));
    }

    IEnumerator animate(float duration)
    {
        float time = 0f;

        if (isGoingIn)
        {
            oldPos = outPos;
            newPos = inPos;
        }
        else
        {
            oldPos = inPos;
            newPos = outPos;
        }

        while (time < duration)
        {

            float t = time / duration;

            t = easeInQuint(t);

            transform.localPosition = Vector3.Lerp(oldPos, newPos, t);

            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = newPos;
    }

    // from easings.net
    float easeInQuint(float x)
    {
        return x * x * x * x;

    }
}
