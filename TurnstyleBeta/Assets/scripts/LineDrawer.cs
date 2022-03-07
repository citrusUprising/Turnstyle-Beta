using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CoreModule;

public class LineDrawer : MonoBehaviour
{

    public Station[] stationsInLine;
    LineRenderer render;
    public Vector3 offset;
    public Color lineColor;


    // Start is called before the first frame update
    void Start()
    {
      render = GetComponent<LineRenderer>();
      render.startColor = lineColor;
      render.endColor = lineColor;

        // set width of the renderer
        render.startWidth = 0.3f;
        render.endWidth = 0.3f;
        DrawLine(stationsInLine);
    }

    void DrawLine(Station[] stations)
    {
        render.positionCount = stations.Length;
        for(int i = 0; i < stations.Length; i++)
        {
            Debug.Log(lineColor + " Station " + i + ": " + stations[i].transform.position);
            Vector3 vector = stations[i].transform.position + offset;
            render.SetPosition(i, vector);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
