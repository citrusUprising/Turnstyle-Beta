using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMapsHolder : MonoBehaviour
{
    public stationDataHolder[][] masterlist;
    // Start is called before the first frame update
    void Start()
    {
        masterlist = new stationDataHolder[4][];

        //-----------------------------MAP FOR DAY 2-----------------------------------//
        masterlist[0] = new stationDataHolder[30]{
            /*Yellow1*/ new stationDataHolder(),
            /*Yellow2*/ new stationDataHolder(),
            /*Yellow3*/ new stationDataHolder(),
            /*Yellow4*/ new stationDataHolder(),
            /*Yellow5*/ new stationDataHolder(),
            /*Orange1*/ new stationDataHolder(), //College Station
            /*Blue1*/   new stationDataHolder(),
            /*Blue2*/   new stationDataHolder(),
            /*Blue3*/   new stationDataHolder(),
            /*Blue4*/   new stationDataHolder(),
            /*Pink1*/   new stationDataHolder(),
            /*Pink2*/   new stationDataHolder(),
            /*Pink3*/   new stationDataHolder(),
            /*Pink4*/   new stationDataHolder(),
            /*Pink5*/   new stationDataHolder(),
            /*Pink6*/   new stationDataHolder(),
            /*Green1*/  new stationDataHolder(), //Starting Station
            /*Green2*/  new stationDataHolder(),
            /*Green3*/  new stationDataHolder(),
            /*Green4*/  new stationDataHolder(),
            /*Green5*/  new stationDataHolder(),
            /*Green6*/  new stationDataHolder(),
            /*Green7*/  new stationDataHolder(),
            /*Green8*/  new stationDataHolder(),
            /*Green9*/  new stationDataHolder(),
            /*Red1*/    new stationDataHolder(),
            /*Red2*/    new stationDataHolder(),
            /*Red3*/    new stationDataHolder(),
            /*Red4*/    new stationDataHolder(),
            /*Red5*/    new stationDataHolder()
        };

        //-----------------------------MAP FOR DAY 3-----------------------------------//
        masterlist[1] = new stationDataHolder[30]{
            /*Yellow1*/ new stationDataHolder(),
            /*Yellow2*/ new stationDataHolder(),
            /*Yellow3*/ new stationDataHolder(),
            /*Yellow4*/ new stationDataHolder(),
            /*Yellow5*/ new stationDataHolder(),
            /*Orange1*/ new stationDataHolder(), //College Station
            /*Blue1*/   new stationDataHolder(),
            /*Blue2*/   new stationDataHolder(),
            /*Blue3*/   new stationDataHolder(),
            /*Blue4*/   new stationDataHolder(),
            /*Pink1*/   new stationDataHolder(),
            /*Pink2*/   new stationDataHolder(),
            /*Pink3*/   new stationDataHolder(),
            /*Pink4*/   new stationDataHolder(),
            /*Pink5*/   new stationDataHolder(),
            /*Pink6*/   new stationDataHolder(),
            /*Green1*/  new stationDataHolder(), //Starting Station
            /*Green2*/  new stationDataHolder(),
            /*Green3*/  new stationDataHolder(),
            /*Green4*/  new stationDataHolder(),
            /*Green5*/  new stationDataHolder(),
            /*Green6*/  new stationDataHolder(),
            /*Green7*/  new stationDataHolder(),
            /*Green8*/  new stationDataHolder(),
            /*Green9*/  new stationDataHolder(),
            /*Red1*/    new stationDataHolder(),
            /*Red2*/    new stationDataHolder(),
            /*Red3*/    new stationDataHolder(),
            /*Red4*/    new stationDataHolder(),
            /*Red5*/    new stationDataHolder()
        };

        //-----------------------------MAP FOR DAY 4-----------------------------------//
        masterlist[2] = new stationDataHolder[30]{
            /*Yellow1*/ new stationDataHolder(),
            /*Yellow2*/ new stationDataHolder(),
            /*Yellow3*/ new stationDataHolder(),
            /*Yellow4*/ new stationDataHolder(),
            /*Yellow5*/ new stationDataHolder(),
            /*Orange1*/ new stationDataHolder(), //College Station
            /*Blue1*/   new stationDataHolder(),
            /*Blue2*/   new stationDataHolder(),
            /*Blue3*/   new stationDataHolder(),
            /*Blue4*/   new stationDataHolder(),
            /*Pink1*/   new stationDataHolder(),
            /*Pink2*/   new stationDataHolder(),
            /*Pink3*/   new stationDataHolder(),
            /*Pink4*/   new stationDataHolder(),
            /*Pink5*/   new stationDataHolder(),
            /*Pink6*/   new stationDataHolder(),
            /*Green1*/  new stationDataHolder(), //Starting Station
            /*Green2*/  new stationDataHolder(),
            /*Green3*/  new stationDataHolder(),
            /*Green4*/  new stationDataHolder(),
            /*Green5*/  new stationDataHolder(),
            /*Green6*/  new stationDataHolder(),
            /*Green7*/  new stationDataHolder(),
            /*Green8*/  new stationDataHolder(),
            /*Green9*/  new stationDataHolder(),
            /*Red1*/    new stationDataHolder(),
            /*Red2*/    new stationDataHolder(),
            /*Red3*/    new stationDataHolder(),
            /*Red4*/    new stationDataHolder(),
            /*Red5*/    new stationDataHolder()
        };

        //-----------------------------MAP FOR DAY 5-----------------------------------//
        masterlist[3] = new stationDataHolder[30]{
            /*Yellow1*/ new stationDataHolder(),
            /*Yellow2*/ new stationDataHolder(),
            /*Yellow3*/ new stationDataHolder(),
            /*Yellow4*/ new stationDataHolder(),
            /*Yellow5*/ new stationDataHolder(),
            /*Orange1*/ new stationDataHolder(), //College Station
            /*Blue1*/   new stationDataHolder(),
            /*Blue2*/   new stationDataHolder(),
            /*Blue3*/   new stationDataHolder(),
            /*Blue4*/   new stationDataHolder(),
            /*Pink1*/   new stationDataHolder(),
            /*Pink2*/   new stationDataHolder(),
            /*Pink3*/   new stationDataHolder(),
            /*Pink4*/   new stationDataHolder(),
            /*Pink5*/   new stationDataHolder(),
            /*Pink6*/   new stationDataHolder(),
            /*Green1*/  new stationDataHolder(), //Starting Station
            /*Green2*/  new stationDataHolder(),
            /*Green3*/  new stationDataHolder(),
            /*Green4*/  new stationDataHolder(),
            /*Green5*/  new stationDataHolder(),
            /*Green6*/  new stationDataHolder(),
            /*Green7*/  new stationDataHolder(),
            /*Green8*/  new stationDataHolder(),
            /*Green9*/  new stationDataHolder(),
            /*Red1*/    new stationDataHolder(),
            /*Red2*/    new stationDataHolder(),
            /*Red3*/    new stationDataHolder(),
            /*Red4*/    new stationDataHolder(),
            /*Red5*/    new stationDataHolder()
        };
    }
}
