using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDestroyer : MonoBehaviour
{
    public float timeOut;

    private float animationOneLength;

    private float animationTwoLength;

    public float animationRatio;

    // Start is called before the first frame update
    void Start()
    {

        animationOneLength = (1 - animationRatio) * timeOut;

        animationTwoLength = animationRatio * timeOut;

        

        gameObject.transform.localScale = new Vector3(.5f, .5f, 1f);

        //gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        StartCoroutine(animationOne());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator animationOne()
    {
        float time = 0f;
        float duration = animationOneLength;

        while (time < duration)
        {
            float t = time / duration;

            t = t * t * t * t;

            gameObject.transform.localScale = Vector3.Lerp(new Vector3(5, 5, 1), new Vector3(1, 1, 1), t);

            gameObject.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, t));

            time += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.localScale = new Vector3(1, 1, 1);

        gameObject.GetComponent<CanvasRenderer>().SetAlpha(1);

        StartCoroutine(animationTwo());
    }

    IEnumerator animationTwo()
    {
        float time = 0f;
        float duration = animationTwoLength/2;

        Vector3 oldPos = gameObject.transform.localPosition;
        Vector3 newPos = gameObject.transform.localPosition;

        newPos[1] += 200;

        Debug.Log("beginning of second animation");

        yield return new WaitForSeconds(duration);

        while (time < duration)
        {
            float t = time / duration;

            gameObject.transform.localPosition = Vector3.Lerp(oldPos, newPos, t);

            gameObject.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1, 0, t));

            time += Time.deltaTime;

            // Debug.Log("middle of second animation");

            yield return null;
        }

        gameObject.transform.localPosition = newPos;

        gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);

        Debug.Log("end of second animation");

        Destroy(gameObject);
    }
}
