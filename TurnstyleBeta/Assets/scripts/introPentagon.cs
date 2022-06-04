using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introPentagon : MonoBehaviour
{

    public introTriangle[] triangles;

    public float triAnimationDuration;
    public bool triIsGoingIn;

    public Vector3 onScreen;
    public Vector3 offScreen;
    public bool isGoingOnScreen;

    private bool isFirstRotation = true;

    public introAnimationController animationController;

    // Start is called before the first frame update
    void Start()
    {
        foreach (introTriangle tri in triangles)
        {
            tri.animationDuration = triAnimationDuration;
            tri.isGoingIn = triIsGoingIn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTriAnimation(float duration, bool isGoingIn)
    {
        foreach (introTriangle tri in triangles)
        {
            tri.isGoingIn = isGoingIn;
            tri.startAnimation(duration);
        }
    }

    public void startRotateAnimation(float duration, int numOfRotations)
    {
        StartCoroutine(rotate(duration, numOfRotations));
    }

    public void startMoveAnimation(float duration)
    {
        StartCoroutine(move(duration));
    }

    IEnumerator rotate(float duration, int numOfRotations)
    {
        float rotateTime = 0f;

        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f * numOfRotations;

        while (rotateTime < duration)
        {

            float t = rotateTime / duration;

            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

            rotateTime += Time.deltaTime * animationController.timeMultiplier;

            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRotation);
    }

    IEnumerator move(float duration)
    {
        float time = 0f;

        Vector3 oldPos = Vector3.zero;
        Vector3 newPos = Vector3.zero;

        if (isGoingOnScreen)
        {
            oldPos = offScreen;
            newPos = onScreen;
        }
        else 
        {
            oldPos = onScreen;
            newPos = offScreen;
        }

        Debug.Log("old pos: " + oldPos);
        Debug.Log("new pos: " + newPos);

        while (time < duration)
        {
            float t = time / duration;

            t = t * t * (3f - 2f * t);

            time += Time.deltaTime * animationController.timeMultiplier;

            transform.localPosition = Vector3.Lerp(oldPos, newPos, t);

            yield return null;
        }

        transform.localPosition = newPos;
    }
}
