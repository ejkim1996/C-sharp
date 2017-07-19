using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
    //set up variables
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public string whichController; //set after the script is attached to assign two of four methods in this script

	// Use this for initialization
	void Start () {
        //set trackedObj from the SteamVR_TrackedObject component of the controller
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        //set the device reference using the trackedObj index.
		device = SteamVR_Controller.Input((int)trackedObj.index);

        //make the "right" controller's trigger button start/stop the rotation and its
        //grip button to increase the speed
        if (whichController == "right"){
             if (device.GetPressDown(triggerButton)) {
                if (Rotator.rotate == false && Rotator.begin) {
                    Rotator.speed = 20 * Rotator.direction;
                    Rotator.rotate = true;
                    Rotator.begin = false;
			    }
			    else if (Rotator.rotate == false && !(Rotator.begin)) {
                    Rotator.speed = Rotator.speedAtStop * Rotator.direction;
                    Rotator.rotate = true;
			    }
			    else {
                    Rotator.speedAtStop = Rotator.speed/Rotator.direction;
                    Rotator.speed = 0;
                    Rotator.rotate = false;
			    }
            }
            if (device.GetPressDown(gripButton)) {
			    Rotator.speed += 2 * Rotator.direction;
            }
        }

        //make the "left" controller's trigger button change the direction of rotation and its
        //grip button to decrease the speed
        else if (whichController == "left") {
            if (device.GetPressDown(triggerButton)) {
                Rotator.direction *= -1;
                Rotator.speed *= -1;
            }
            if (device.GetPressDown(gripButton)) {
                Rotator.speed -= 2 * Rotator.direction;
            }
        }
	}
}