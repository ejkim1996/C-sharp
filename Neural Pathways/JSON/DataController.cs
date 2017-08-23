using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour {

    private AnimalData[] allAnimalData;
    private AnimalData coyoteData;
    private AnimalData humanData;
    private AnimalData dolphinData;

    private string animalDataFileName = "data.json";

    public Canvas coyoteCameraCanvas;
    public Canvas coyoteWorldSpaceCanvas;
    private Text[] coyoteTitle;
    private Text[] coyoteActivation;

    public Canvas humanCameraCanvas;
    public Canvas humanWorldSpaceCanvas;
    private Text[] humanTitle;
    private Text[] humanActivation;

    public Canvas dolphinCameraCanvas;
    public Canvas dolphinWorldSpaceCanvas;
    private Text[] dolphinTitle;
    private Text[] dolphinActivation;

    // Use this for initialization
    void Start () {
        LoadGameData();

        coyoteData = allAnimalData[0];
        humanData = allAnimalData[1];
        dolphinData = allAnimalData[2];

        coyoteTitle = coyoteCameraCanvas.GetComponentsInChildren<Text>();
        coyoteActivation = coyoteWorldSpaceCanvas.GetComponentsInChildren<Text>();

        humanTitle = humanCameraCanvas.GetComponentsInChildren<Text>();
        humanActivation = humanWorldSpaceCanvas.GetComponentsInChildren<Text>();

        dolphinTitle = dolphinCameraCanvas.GetComponentsInChildren<Text>();
        dolphinActivation = dolphinWorldSpaceCanvas.GetComponentsInChildren<Text>();

        coyoteTitle[0].text = coyoteData.name;
        humanTitle[0].text = humanData.name;
        dolphinTitle[0].text = dolphinData.name;

        UnloadRuntimeData("coyote");
        UnloadRuntimeData("human");
        UnloadRuntimeData("dolphin");


    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey("q"))
        {
            LoadRuntimeData("coyote", "visual");
            LoadRuntimeData("human", "visual");
            LoadRuntimeData("dolphin", "visual");
        }
        if (Input.GetKey("w"))
        {
            LoadRuntimeData("coyote", "auditory");
            LoadRuntimeData("human", "auditory");
            LoadRuntimeData("dolphin", "auditory");
        }
        if (Input.GetKey("e"))
        {
            LoadRuntimeData("coyote", "olfactory");
            LoadRuntimeData("human", "olfactory");
            LoadRuntimeData("dolphin", "olfactory");
        }
    }
    
    public void LoadRuntimeData(string animal, string sense)
    {
        SensesData senseData = new SensesData();
        AnimalData animalData = new AnimalData();
        Text[] activationData = new Text[4];
        Text[] titleData = new Text[2];

        if (animal.Equals("coyote"))
        {
            animalData = coyoteData;
            titleData = coyoteTitle;
            activationData = coyoteActivation;
        }
        else if (animal.Equals("human"))
        {
            animalData = humanData;
            titleData = humanTitle;
            activationData = humanActivation;
        }
        else if (animal.Equals("dolphin"))
        {
            animalData = dolphinData;
            titleData = dolphinTitle;
            activationData = dolphinActivation;
        }

        if (sense.Equals("visual"))
        {
            senseData = animalData.senses[0];
        }
        else if (sense.Equals("auditory"))
        {
            senseData = animalData.senses[1];
        }
        else if (sense.Equals("olfactory"))
        {
            senseData = animalData.senses[2];
        }

        titleData[1].text = senseData.name;
        activationData[0].text = senseData.activation;
        activationData[1].text = senseData.facts[0];
        activationData[2].text = senseData.facts[1];
        activationData[3].text = senseData.facts[2];
    }

    public void UnloadRuntimeData(string animal)
    {
        Text[] activationData = new Text[4];
        Text[] titleData = new Text[2];

        if (animal.Equals("coyote"))
        {
            activationData = coyoteActivation;
            titleData = coyoteTitle;
        }
        else if (animal.Equals("human"))
        {
            activationData = humanActivation;
            titleData = humanTitle;
        }
        else if (animal.Equals("dolphin"))
        {
            activationData = dolphinActivation;
            titleData = dolphinTitle;
        }

        titleData[1].text = "";
        activationData[0].text = "";
        activationData[1].text = "";
        activationData[2].text = "";
        activationData[3].text = "";
    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, animalDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            InteractiveData loadedData = JsonUtility.FromJson<InteractiveData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allAnimalData = loadedData.allAnimalData;

            /*
            Debug.Log(loadedData.allAnimalData[0].name);
            for (int i = 0; i < allAnimalData.Length; i++)
            {
                Debug.Log(allAnimalData[i].name);
                for (int j = 0; j < allAnimalData[i].senses.Length; j++)
                {
                    Debug.Log(allAnimalData[i].senses[j].name);
                }
            }
            */
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }

    }
}
