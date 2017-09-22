using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhidgetsPathwaysAppear : MonoBehaviour
{
    //public variables to be set on Unity inspector
    public int senseCount;
    public bool human;
    public string animalName;
    public Material[] materials; //drag and drop textures of pathways
    //change in Inspector during play mode to get accurate values for displaying
    public float zCutOff;
    public float speed = 1.5f;
    public float zStart = -0.3f;
    public float zEnd = 0.3f;
    //drag and drop GameObjects with appropriate helper scripts
    public DataController DataController;
    public ZoomHelperScript ZoomHelperScript;
    public PhidgetsRotator rotator;
    //timeout value for resetting to default
    public float timeout = 5.0f;
    //set debug keys in Inpector
    public string[] DEBUG_KEYS;

    //private variables
    private float[] t;
    private int focusedSense = -1;
    private string[] senseName = new string[3];
    private bool[] inputs = null;
    private float lastPressed = 0.0f;
    private bool isBlank = false;
    private bool firstStart = true;
    private float firstStartTime;
    private float[] currentZs;

    void Start()
    {
        t = new float[senseCount];
        firstStartTime = Time.time;
        senseName[0] = "visual";
        senseName[1] = "olfactory";
        senseName[2] = "auditory";

        currentZs = new float[senseCount];

        //get touch sensor data from appropriate Phidgets ifKit
        if (animalName.Equals("human"))
        {
            inputs = PhidgetsManager.Instance.ifKit1Inputs;
        }
        else if (animalName.Equals("coyote"))
        {
            inputs = PhidgetsManager.Instance.ifKit2Inputs;
        }
        else if (animalName.Equals("dolphin"))
        {
            inputs = PhidgetsManager.Instance.ifKit3Inputs;
        }
    }

    void Update()
    {
        //display pathways on startup
        if (firstStart == true)
            ResetData();

        if (Time.time - firstStartTime > 3.0)
            firstStart = false;

        //get focused sense index when a sense is pressed
        if (inputs[0] == true || inputs[1] == true || inputs[2] == true)
            focusedSense = CheckFocusedSense();

        //light corresponding pathway based on focusedSense value
        LightPathways(focusedSense);

        //check how long it's been since head was rotated
        if (rotator.rotating == true)
            lastPressed = Time.time;

        //reset to default after set time
        if (lastPressed + timeout <= Time.time)
            ResetData();

        //set up arrays for displaying pathways
        SetUpArrays();

        //show pathways using debug keys (set on Unity inspector)
        for (int i = 0; i < DEBUG_KEYS.Length; i++)
        {
            if (Input.GetKeyDown(DEBUG_KEYS[i]))
            {
                Blank();
                focusedSense = i;
            }

        }
    }

    //make pathway blank
    void Blank()
    {
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = 0.0f;
        }
        lastPressed = Time.time;
    }

    //return an int based on which sense was pressed
    int CheckFocusedSense()
    {
        if (isBlank == false)
        {
            Blank();
            isBlank = true;
        }
        isBlank = false;

        if (inputs[0])
        {
            return 0;
        }
        else if (inputs[1])
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    //light pathway according to focusedSense value
    void LightPathways(int focusedSense)
    {
        if (focusedSense != -1)
        {
            firstStart = false;
            if (focusedSense <= senseCount - 1)
            {
                //light up pathway
                t[focusedSense] += 0.5f * Time.deltaTime * speed;

                //for the human model, visual pathways are two different textures
                if (human == true && focusedSense == 0)
                {
                    t[3] += 0.007f;
                    t[3] += 0.5f * Time.deltaTime * speed;
                }
                //zoom into the animal
                ZoomHelperScript.Zoom(animalName);
            }
            //load text data
            DataController.LoadRuntimeData(animalName, senseName[focusedSense]);
        }
    }

    void ResetData()
    {
        focusedSense = -1;

        //make all pathways reappear
        for (int i = 0; i < senseCount; i++)
        {
            t[i] += 0.5f * Time.deltaTime * speed;
        }

        //unload text data and zoom out
        DataController.UnloadRuntimeData(animalName);
        ZoomHelperScript.ZoomOut(animalName);
    }

    void SetUpArrays()
    {
        for (int i = 0; i < senseCount; i++)
        {
            currentZs[i] = Mathf.Lerp(zStart, zEnd, t[i]);
        }

        for (int i = 0; i < senseCount; i++)
        {
            materials[i].SetFloat("_MovingZ", currentZs[i]);
        }
    }
}

