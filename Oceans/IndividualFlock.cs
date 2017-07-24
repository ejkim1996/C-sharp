/* This script controls the alignment, cohesion, and
 * separation of the flocking/schooling fish. It also
 * keeps the fish within the boundaries and makes them
 * run away from danger.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualFlock : MonoBehaviour {
    //public variables
    public float speed;
    public int runAwayTime = 50;
    public float rotationSpeed = 4.0f;
    public float neighborDistance = 3.0f;
    public float tankSize = 15;

    //private variables
    private Vector3 averageDirection;
    private Vector3 averagePosition;
    private bool turning;
    private bool collision;
    private bool running = false;
    private float runAwayCurrent = -1;
    private Transform collisionTransform;
    

	// Use this for initialization
	void Start () {
        speed = Random.Range(0.5f, 1);
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void Update () {
        //collision = false;
        //if the fish is outside of the tank then make it swim back inside
        if (Vector3.Distance(transform.position, Vector3.zero) >= tankSize)
        {
            turning = true;
        }
        else
            turning = false;

        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        //if the fish is inside the tank, apply rules of flocking/schooling
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }

        //make fish run away from danger for # of frames set by runAwayTime
        if (runAwayCurrent >= 0 && runAwayCurrent <= runAwayTime)
        {
            RunAway();
            runAwayCurrent++;
            running = true;
        }
        else if (runAwayCurrent > runAwayTime)
        {
            speed = Random.Range(0.5f, 1);
            runAwayCurrent = -1;
            running = false;
        }

        //make fish move forward with specified speed
        transform.Translate(0, 0, Time.deltaTime * speed);           
	}

    //check for collision with danger layer to start run away sequence
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11)
        {
            //collision = true;
            if (runAwayCurrent == -1)
                runAwayCurrent = 0;
            collisionTransform = other.transform;
        }
    }

    //increase speed and start rotation for running away
    void RunAway()
    {
        Vector3 direction = (this.transform.position - collisionTransform.position);
        speed = 4f;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(direction),
                                 rotationSpeed * Time.deltaTime);
    }

    //Rules for fish flocking/schooling
    void ApplyRules()
    {
        //store all fish instances in a GameObject array
        GameObject[] gos;
        gos = GlobalFlock.allFish;

        //set up variables
        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        Vector3 goalPos = GlobalFlock.goalPos;
        float gSpeed = speed;
        float dist;
        
        int groupSize = 1;
        /* For each of the other fish, if they are not running from danger check
         * if they are close enough to be in a group, and if they are too close to
         * each other and define variables
         */ 
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject && go.GetComponent<IndividualFlock>().running == false)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighborDistance)
                {
                    vCenter += go.transform.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    IndividualFlock anotherFlock = go.GetComponent<IndividualFlock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        /* If fish are in a group, then give them the same average speed and direction
         * (unless they are too close) and rotate them towards the direction.
         */
        if (groupSize > 1)
        {
            vCenter = vCenter / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                     Quaternion.LookRotation(direction),
                                     rotationSpeed * Time.deltaTime);
        }
    }
}
