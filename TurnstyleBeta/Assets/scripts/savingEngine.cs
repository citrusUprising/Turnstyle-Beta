using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
class SaveData
{
    public Station currentStation;
    public int currentCutscene;
}

public class savingEngine : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
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
    data.currentStation = GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation;
    data.currentCutscene = GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene;
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
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = 
                   File.Open(Application.persistentDataPath 
                   + "/MySaveData.dat", FileMode.Open);
		SaveData data = (SaveData)bf.Deserialize(file);
		file.Close();
        GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation = data.currentStation;
        GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene = data.currentCutscene;
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
    GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentStation = GameObject.Find("Green Station 1").GetComponent<Station>();
    GameObject.Find("NodeMapCamera").GetComponent<CameraController>().currentCutScene = 0;
		Debug.Log("Data reset complete!");
	}
	else
		Debug.LogError("No save data to delete.");
    }
}
