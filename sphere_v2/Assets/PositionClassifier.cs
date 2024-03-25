using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class PositionClassifier : MonoBehaviour
{

    //declaring struct that matches the db.json structure
    [System.Serializable] //ensures that content is serialized (converted into something unity knows)
    public class emgData
    {
        public int id;
        public string name;
        public int classification;
    }

    public class ClassificationData
    {
        public Vector3 position;
        public int classification;
    }


    //???????????
    /*
    [System.Serializable]
    public class RootObject 
    {
        public List<emgData> emgList;
    }
    */

    public List<ClassificationData> classy = new List<ClassificationData>();

    // Start is called before the first frame update
    private void Start()
    {
        LoadClassificationData("/Users/arianasun/Desktop/research/bioAR/unity_json/db2.json");
        MapPositionToClassification(transform.position);
    }
    

    //method to read classification data from JSON file
    public void LoadClassificationData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            
            classy.Clear();
            //using JsonUtility (UNITY)
            //RootObject rootObject = JsonUtility.FromJson<RootObject>(json);
            //List<ClassificationData> classificationData = new List<ClassificationData>();

            //using SimpleJSON
            JSONNode rootNode = JSON.Parse(json);

            Debug.Log(json);
            
            //SimpleJSON version
            //since json file has various objects with arrays in each
            if (rootNode.IsObject)
            {
                Debug.Log("inside if statement for rootNode.IsObject");
                //have to get "emgData" array from root object
                JSONNode emgDataArray = rootNode["emg data"];

                Debug.Log("entering for loop for jsonnode node in emgdataarray");
                foreach (JSONNode node in emgDataArray)
                {
                    emgData data = new emgData
                    {
                        id = node["id"].AsInt,
                        name = node["name"].Value,
                        classification = node["classification"].AsInt
                    };

                    Debug.Log("printing data");
                    Debug.Log(data.id);
                    Debug.Log(data.name);
                    Debug.Log(data.classification);

                    ClassificationData classData = new ClassificationData
                    {
                        position = Vector3.zero,
                        classification = data.classification
                    };

                    classy.Add(classData);
                }
                Debug.Log("exiting for loop from jsonnode node in emgdataaraay");
            } 
            else
            {
                Debug.LogError("Invalid JSON format. Expected an array. so sad :(((");
            }
        }
        else
        {
            Debug.LogError("awww JSON file not found at path: " + filePath);
            return;
        }
    }

    private void MapPositionToClassification(Vector3 position)
    {
        if (classy == null)
        {
            Debug.LogWarning("classy is null. Make sure to load data before calling MapPositionToClassification.");
            return;
        }

        //Debug.Log("starting for loop shit");
        foreach (var data in classy)
        {
            Debug.Log("entered for loop for data in classy yay");
            if (Vector3.Distance(data.position, position) < 0.1f)
            {
                Debug.Log("Classification: " + data.classification);
                data.classification = data.classification;
                
                if (data.classification == 1){
                    //code to rescale sphere to be bigger
                    GameObject sphere = GameObject.Find("SphereInteractable");
                    //private Vector3 scaleChange = new Vector(0.1f, 0.1f, 0.1f);
                    //Vector3 newScale = transform.localScale;
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Debug.Log("scale increased yay");
                    //transform.localScale = newScale;
                    
                }
                
                //break;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (classy != null)
        {
            MapPositionToClassification(transform.position);
        }
        
    }
}
