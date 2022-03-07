using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSmallPentagon : MonoBehaviour
{
    public Vector3 rotationDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection);
    }
}
