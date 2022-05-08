using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
class SaveData
{
    public int currentStation;
    public int currentCutscene;
    public int currentTutorial;
}

public class savingEngine : MonoBehaviour
{

public bool load;
    
    // Start is called before the first frame update
    void Start()
    {
     load = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //----------------------------------------------------------------------------------------------//
    //code borrowed from Lance Talbert at Redgate
    //https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
    //----------------------------------------------------------------------------------------------//
    public void checkpoint(){ 
        BinaryFormatter bf = new BinaryFormatter(); 
	FileStream file = File.Create(Application.persistentDataPath 
                 + "/MySaveData.dat"); 
	SaveData data = new SaveData();
    data.currentStation = 
    Array.IndexOf(GameObject.Find("NodeMapCamera").GetComponent<CameraController>().allStations,
    GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation);
    //Debug.Log("the current saved station is #"+data.currentStation);
    data.currentCutscene = GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene;
    //Debug.Log("the current saved cutscene is #"+data.currentCutscene);
    data.currentTutorial = GameObject.Find("CurrentStats").GetComponent<CurrentStats>().currentTutorial;
	bf.Serialize(file, data);
	file.Close();
	Debug.Log("Game data saved!");
    }

    //----------------------------------------------------------------------------------------------//
    //code borrowed from Lance Talbert at Redgate
    //https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
    //----------------------------------------------------------------------------------------------//
    public void retry(){
        if (File.Exists(Application.persistentDataPath 
                   + "/MySaveData.dat"))
	{
        Debug.Log("Save data found");
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = 
                   File.Open(Application.persistentDataPath 
                   + "/MySaveData.dat", FileMode.Open);
		SaveData data = (SaveData)bf.Deserialize(file);
		file.Close();
        GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation = GameObject.Find("NodeMapCamera").GetComponent<CameraController>().allStations[data.currentStation];
        //Debug.Log("the current loaded station is #"+data.currentStation);
        GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene = data.currentCutscene;
        //Debug.Log("the current loaded cutscene is #"+data.currentCutscene);
        GameObject.Find("CurrentStats").GetComponent<CurrentStats>().currentTutorial = data.currentTutorial;
		Debug.Log("Game data loaded!");
	}
	else
		Debug.LogError("There is no save data!");
    }

    //----------------------------------------------------------------------------------------------//
    //code borrowed from Lance Talbert at Redgate
    //https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
    //----------------------------------------------------------------------------------------------//
    public void reset (){
	if (File.Exists(Application.persistentDataPath 
                  + "/MySaveData.dat"))
	{
		File.Delete(Application.persistentDataPath 
                          + "/MySaveData.dat");
    //GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation = GameObject.Find("Green Station 1").GetComponent<Station>();
    //GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene = 0;
		Debug.Log("Data reset complete!");
	}
	else
		Debug.LogError("No save data to delete.");
    }
}