using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    public string whichController;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		device = SteamVR_Controller.Input((int)trackedObj.index);

        if (whichController == "right"){
             if (device.GetPressDown(triggerButton)) {
                Debug.Log("Right trigger pressed.");
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
                Debug.Log ("right grip button pressed.");
			    Rotator.speed += 2 * Rotator.direction;
            }
        } else if (whichController == "left") {
            if (device.GetPressDown(triggerButton)) {
                Debug.Log("Left trigger pressed.");
                Rotator.direction *= -1;
                Rotator.speed *= -1;
            }
            if (device.GetPressDown(gripButton)) {
                Rotator.speed -= 2 * Rotator.direction;
            }
        }
	}
}