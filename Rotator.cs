using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public static int speed = 0;
	public static bool begin = true;
	public static int speedAtStop = 0;
	public static bool rotate = false;
	public static int direction = 1;
	
	// Update is called once per frame
	void Update () {
		//rotate the object based on speed and direction
		transform.Rotate(new Vector3(speed, 0, 0) * Time.deltaTime);

		//if the up key is pressed, increase speed
		if (Input.GetKeyDown("up")) {
			Debug.Log ("up key pressed.");
			speed += 2 * direction;
		}

		//if the down key is pressed, decrease speed
		if (Input.GetKeyDown("down")) {
			Debug.Log ("down key pressed.");
			speed -= 2 * direction;
		}

		//if the x key is pressed, change direction
		if (Input.GetKeyDown("x")) {
			Debug.Log("x key pressed.");
			direction *= -1;
			speed *= -1;
		}

		/*if the s key is pressed, start or stop.
		speed is 20 when object first starts.
		Otherwise speed and direction is same as
		when the rotator last stopped */
		if (Input.GetKeyDown("s")) {
			Debug.Log("s key pressed.");
			if (rotate == false && begin) {
				speed = 20 * direction;
				rotate = true;
				begin = false;
			}
			else if (rotate == false && !begin) {
				speed = speedAtStop * direction;
				rotate = true;
			}
			else {
				speedAtStop = speed/direction;
				speed = 0;
				rotate = false;
			}
			
		}
	}
}
