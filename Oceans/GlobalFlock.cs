/* This script determines the boundaries of the space the
 * fish can swim around in, the point the fish swim towards,
 * and creates the specified number of fish into the tank.
 * Add empty GameObjects to the "path" in the Inspector 
 * after moving them to desired locations, and the fish will
 * swim to one of these locations.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {
    //declare variables
    public Transform[] path;
    public GameObject fishPrefab;
    public GameObject tankCorners;
    public static int tankSize = 5;

    static int numFish = 100;
    public static GameObject[] allFish = new GameObject[numFish];

    public static Vector3 goalPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numFish; i++)
        {
            //choose a random Vector3 position and instantiate the fish prefab
            Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize),
                                      Random.Range(-tankSize, tankSize),
                                      Random.Range(-tankSize, tankSize));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
        }
	}

    // Update is called once per frame
    void Update()
    {
        //every 1 out of 200 frames change where the fish are swimming to
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = path[Random.Range(0, path.Length)].transform.position;
        }
    }

    //draw spheres on the corners of the tank
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(-tankSize, -tankSize, -tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(tankSize, -tankSize, -tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(-tankSize, tankSize, -tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(-tankSize, -tankSize, tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(tankSize, tankSize, tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(-tankSize, tankSize, tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(tankSize, -tankSize, tankSize), 0.5f);
        Gizmos.DrawSphere(new Vector3(tankSize, tankSize, -tankSize), 0.5f);
    }
}
