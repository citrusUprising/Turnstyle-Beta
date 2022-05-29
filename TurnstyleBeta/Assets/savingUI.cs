using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class savingUI : MonoBehaviour
{

    public float animationTime = 2f;
    public float lerpDuration = .5f;

    public Vector3 firstPosition; 
    public Vector3 secondPosition;

    public GameObject pentagon;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lerpOnScreen());
    }

    // Update is called once per frame
    void Update()
    {
        pentagon.transform.Rotate(0, 0, -.5f);
    }

    IEnumerator lerpOnScreen()
    {
        float time = 0f;
        float duration = lerpDuration;

        while (time < duration)
        {

            float t = time / duration;

            t = t * t * (3f - 2f * t);

            gameObject.GetComponent<RectTransform>().position = Vector3.Lerp(firstPosition, secondPosition, t);

            time += Time.deltaTime;

            yield return null;
        }

        transform.position = secondPosition;

        yield return new WaitForSeconds(animationTime);

        time = 0f;

        while (time < duration)
        {

            float t = time / duration;

            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(secondPosition, firstPosition, t);

            time += Time.deltaTime;

            yield return null;
        }

        transform.position = firstPosition;

        Destroy(gameObject);
    }
}
