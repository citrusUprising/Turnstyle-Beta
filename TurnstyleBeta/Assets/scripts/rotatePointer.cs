using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePointer : MonoBehaviour
{

    public Transform[] targets;
    public GameObject cam;
    private Transform currentLoc;
    private float size;
    private float scale;
    private float prevScale = 0;
    [SerializeField] private float xMinJump;
    [SerializeField] private float  xMaxJump;
    [SerializeField] private float yMinJump;
    [SerializeField] private float  yMaxJump;
    private bool rotating = true;
    private bool onScreen;
    private int destination = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentLoc = cam.GetComponent<Transform>();
        size = cam.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        currentLoc = cam.GetComponent<Transform>();
        size = cam.GetComponent<Camera>().orthographicSize;
        scale = size/19f;
        this.transform.localScale = new Vector3(0.8f*scale, 0.8f*scale, scale);
        //determines arrow position
        if (
            targets[destination].position.x - currentLoc.position.x >= xMinJump*scale &&
            targets[destination].position.x - currentLoc.position.x <= xMaxJump*scale &&
            targets[destination].position.y - (currentLoc.position.y+8f) >= yMinJump*scale &&
            targets[destination].position.y - (currentLoc.position.y+8f) <= yMaxJump*scale
        ){onScreen = true;}
        else onScreen = false;

        if(onScreen){
            transform.position = new Vector2 (
            targets[destination].position.x+1f*scale,
            targets[destination].position.y-4f*scale);
            transform.rotation = Quaternion.Euler(new Vector3(0f,0f,90f));
        }else{
            transform.position = new Vector2 (
            currentLoc.position.x,
            currentLoc.position.y+(12f*scale));
        
            //determines arrow rotation if destination isn't close
            Vector2 targeting = targets[destination].position - transform.position;
            float zRot = (Mathf.Atan2(targeting.x,targeting.y)*Mathf.Rad2Deg*-1)+90f;
            /*
            Flips the arrow so it's always right-side-up, currently not working properly

            float yRot;
            if(zRot<-90f ||zRot > 90f){
                yRot = 180f;
            }else{
                yRot = 0.0f;
            }
            Vector3 direction = new Vector3(0.0f,yRot,zRot);
            */
            Vector3 direction = new Vector3(0.0f,0.0f,zRot);
            
            transform.rotation = Quaternion.Euler(direction);
        }
    }
}
