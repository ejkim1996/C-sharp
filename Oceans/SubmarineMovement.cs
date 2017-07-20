/* Script for submarine movement controls.
 * Use UP/DOWN arrows for forward and backward movement.
 * Use LEFT/RIGHT arrows for moving left or right.
 * Use W/S keys for up and down movement.
 * Joystick controls also supported.
 * Bobbing up and down, with subtle z-axis rotation implemented
 *      to simulate effects of underwater currents.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    // public variables
    public float movementSpeed = 10f;
    public float rotationSpeed = 50f;
    public float maxSpeed = 10f;
    public float floatStrength = 0.025f;
    public float maxRotation = 2;
    public float rotationStrength = 0.25f;

    // private variables
    private float originalY;
    private float rotationY;
    private float lastPos;
    private float curPos;
    private bool isMoving = false;
    private float startTime = 0;
    private float surfaceHeight = 450.2f;


    void Start()
    {
        //save defaults
        originalY = this.transform.position.y;
        rotationY = this.transform.rotation.y;
        lastPos = this.transform.position.y;
    }


    void FixedUpdate()
    {
        //input to move submarine forwards and backwards
        if (Input.GetKey("up") || Input.GetAxis("Vertical") > 0.0f)
        {
            forward();
        }

        if (Input.GetKey("down") || Input.GetAxis("Vertical") < 0.0f)
        {
            backward();
        }

        //input to move submarine up or down
        //isMoving set to true since the submarine should only bob up
        //and down when it isn't moving up or down
        if (Input.GetKey("w") || Input.GetAxis("Depth") > 0.0f) 
        {
            isMoving = true;
            //only go up if the submarine is underwater
            if (transform.position.y < surfaceHeight)
            {
                up();
            }
        }

        if (Input.GetKey("s") || Input.GetAxis("Depth") < 0.0f)
        {
            isMoving = true;
            down();
        }

        //check if submarine is moving or not by comparing position to
        //position in previous frame.
        curPos = this.transform.position.y;
        if (curPos == lastPos) {
            originalY = this.transform.position.y;

            //if the submarine was just moving, start timer over so that
            //the submarine bobs startin from the new position
            if (isMoving == true)
            {
                startTime = Time.time;
            }
            isMoving = false;
        }
        lastPos = curPos;
        
        //use Mathf.Sin() function to make the submarine bob up and down
        if (isMoving == false)
        {
            transform.position = new Vector3(transform.position.x,
                originalY + (Mathf.Sin(Time.time - startTime) * floatStrength),
                transform.position.z);
        }

        //input for submarine rotation
        if (Input.GetKey("left") || Input.GetKey("right") || Input.GetAxis("Horizontal") < 0.0f || Input.GetAxis("Horizontal") > 0.0f)
        {
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            rotate(rotation);
            rotationY = this.transform.eulerAngles.y;
        }

        //rotate the submarine in the z-axis very slowly and subtly to enhance bobbing
        transform.rotation = Quaternion.Euler(0f, rotationY, maxRotation * Mathf.Sin(Time.time * rotationStrength));

        // Keep in water
        if (transform.position.y >= surfaceHeight)
        {
            Vector3 temp = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
            GetComponent<Rigidbody>().velocity = temp;
        }

    }


    /*
	 Control Functions
	*/
    void up()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * movementSpeed, ForceMode.Acceleration);
    }

    void down()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * movementSpeed, ForceMode.Acceleration);
    }

    void forward()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            GetComponent<Rigidbody>().AddForce(transform.forward * movementSpeed, ForceMode.Acceleration);

    }

    void backward()
    {
        GetComponent<Rigidbody>().AddForce(-1 * transform.forward * (movementSpeed / 3), ForceMode.Acceleration);
    }

    void rotate(float rotation)
    {
        transform.Rotate(0, rotation, 0);
    }
}
