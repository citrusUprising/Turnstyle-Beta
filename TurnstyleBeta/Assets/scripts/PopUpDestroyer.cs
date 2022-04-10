using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDestroyer : MonoBehaviour
{
    public float timeOut;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeOut);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
